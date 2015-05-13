using UnityEngine;
using System.Collections;
using System.IO;
using System;
//using System.Xml;
using System.ComponentModel;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Net;
using System.Net.Sockets;

//using FluorineFx.IO.Readers;
//using FluorineFx;
//using FluorineFx.IO;
//
using System.Timers;
using System.Collections.Generic;

/**
 * HaConnector负责连接网络，并在出错的时候，抛出错误事件(event)
 * 
 * client连接ha网络的次序是：
 * 1. 接入ha.net (系统会初始化沙箱请求)
 * 2. 发送 SSL_OPEN_REQ，建立加密通道
 * 3. 发送 JOIN，顺利接入到HA网络
 * 4. join以后，可以发送以下指令
 *    a. 查询service所在节点
 *    b. 发送信息到某个节点上
 *    c. get/set节点的属性
 *    d. 离开ha.net
 */
public class HaConnector : MonoBehaviourBase
{
	protected Socket 	_socket;
	protected uint	 	_state; 		// 自己的当前state
	protected Timer	 	_timer; 			// for connect timeout
	protected ProtoByteArray _bytes;			// 数据的缓冲区
	protected ProtoByteArray _receiveBytes;	// 数据的缓冲区
	protected int		_receiveLen;
	protected ARC4 		_erc4; 			// rc4支持(for encode)
	protected ARC4 		_drc4; 			// rc4支持(for decode)
	protected ProtoByteArray _rc4key; 		// rc4 encrypt key
	protected Timer		_keepaliveTimer;

	public event Action<string> OnLeaveEvent;
	public event Action<ProtoByteArray> OnMessageEvent;
	public event Action OnJoinEvent;
	public event Action<ArrayList> OnServiceEvent;
	public event Action OnTimeOutEvent;
	public event Action OnCloseEvent;
	public event Action OnLoginState;
	public event Action<uint> OnStateEvent;

	private Queue<EventObject> _eventObjQueue;

    private List<SendObject> _sendObjPoolList;

	private static readonly int RECEIVE_BUFFER_SIZE = 8192;	
	
	public static long TotalReceiveBytes=0;
	public static long TotalSendBytes=0;
	
	public HaConnector()
	{
		HaApplicationContext.setConnector(this);
		_state = (uint)HaStage.DISCONNECT;		// bug?
		_bytes = new ProtoByteArray(); // 系统缓冲buffer
		_receiveBytes = new ProtoByteArray();
        _sendObjPoolList = new List<SendObject>();
		_eventObjQueue = new Queue<EventObject>();
		setup();
	}

	private int maxRunCount = 4;

	void Update()
	{
		int runCount = 0;
		while (_eventObjQueue.Count > 0)
		{
			EventObject obj = null;
			
			lock (_eventObjQueue)
			{
				obj = _eventObjQueue.Dequeue();
			}
			
			if (obj != null)
			{
				HanderEventObj(obj);
			}
			
			runCount ++;
			
			if (runCount >= maxRunCount)
			{
				break;
			}
		}
	}

	private void HanderEventObj(EventObject obj)
	{
		switch (obj.type)
		{
		case EventObject.Event_Close:
			if (OnCloseEvent != null)
			{
				OnCloseEvent();
			}
			break;
		case EventObject.Event_Joined:
			if (OnJoinEvent != null)
			{
				OnJoinEvent();
			}
			break;
		case EventObject.Event_Leave:
			if (OnLeaveEvent != null)
			{
				OnLeaveEvent(obj.msg);
			}
			break;
		case EventObject.Event_Message:
			if (OnMessageEvent != null)
			{
				OnMessageEvent(obj.protoByteArray);
			}
			break;
		case EventObject.Event_Service:
			if (OnServiceEvent != null)
			{
				OnServiceEvent(obj.services);
			}
			break;
		case EventObject.Event_State:
			if (OnStateEvent != null)
			{
				OnStateEvent(obj.state);
			}
			break;
		case EventObject.Event_Timeout:
			if (OnTimeOutEvent != null)
			{
				OnTimeOutEvent();
			}
			break;
		}
	}

	// C#  -----------------------------------------------------------------------------------------
    public delegate void CallBack_Connect(bool success,Erro_Socket error,string exception);
    public delegate void CallBack_Send(bool success, Erro_Socket error, string exception);
    public delegate void CallBack_Receive( bool success);
    public delegate void CallBack_Disconnect(bool success, Erro_Socket error, string exception);

    public CallBack_Connect      callBack_Connect;
    public CallBack_Send         callBack_Send;
    public CallBack_Receive      callBack_Receive;
    public CallBack_Disconnect   callBack_Disconnect;
	
	private Erro_Socket error_Socket = Erro_Socket.SUCCESS;
	
	public enum Erro_Socket
    {
        SUCCESS = 0,                    //成功
        TIMEOUT = 1,                    //超时
        SOCKET_NULL = 2,                //套接字为空
        SOCKET_UNCONNECT = 3,           //套接字未连接
        CONNECT_UNSUCCESS_UNKNOW = 4,   //连接失败未知错误
        CONNECT_CONNECED_ERROR = 5,     //重复连接错误
        SEND_UNSUCCESS_UNKNOW = 6,      //fa送失败未知错误
        RECEIVE_UNSUCCESS_UNKNOW = 7,   //收消息未知错误
        DISCONNECT_UNSUCCESS_UNKNOW = 8,//断开连接未知错误
    }
	
	private int receiveDataLength = 0;//当前接收长度
    private int allDataLengh = 0; //这类数据总长度
    private byte[] receiveDataAL;//数据拼接用

    private int responseCount = 0;
    private int sendCount = 0;
    private Socket clientSocket;
    private string addressIP;
    private int port;
    private int countTmp = 0;
    private bool isReceiveHalfHead = false;
	
	//---------------------
	
	private void setup()
	{
	}
	
	//--------------------------------------------------------------
	// 属性访问接口
	//--------------------------------------------------------------
	public uint getState()
	{
		return _state;
	}

	public void setState( uint state )
	{
		_state = state;
		
		if (_state == HaStage.LOGINED){
			// 启动定时器
			startKeepalive();		
		}

		EventObject obj = new EventObject(EventObject.Event_State);
		obj.state = _state;
		AddEventObj(obj);

		GameDebuger.Log("HA setState=" + ((int)state).ToString() );
	}

	public void setRc4key( ProtoByteArray k )
	{
		if ( k == null )
		{
//			GameDebuger.Log("setRc4key null");
			return;
		}
		
		_rc4key = k;
		
		_erc4 = new ARC4();
		_drc4 = new ARC4();
		
		_erc4.init(_rc4key);
		_drc4.init(_rc4key);
	}
	
	public ProtoByteArray getRc4key()
	{
		return _rc4key;
	}
	
	public Socket getSocket()
	{
		return _socket;
	}

	public ProtoByteArray getBuffByteArray()
	{
		return _bytes;
	}
	
	public void close()
	{
		if ( _socket != null )
		{
			int handle = _socket.Handle.ToInt32 ( );
			//the handle has been closed.
			if ( handle == -1 )
			{
				Debug.LogWarning ( "the socket's handle has been closed" );
				return;
			}

			if ( getState ( ) == (uint)HaStage.DISCONNECT )
			{
				Debug.LogWarning ( "the network has been disconnected." );
				return;
			}

			//Debug.LogError ( "socket handle = " +   _socket.Handle == null ? "null" :  _socket.Handle.ToInt32().ToString() );
			_socket.Close();
			//Debug.LogError ( "socket handle = " + _socket.Handle == null ? "null" :  _socket.Handle.ToInt32().ToString() );

			Debug.Log ("socket status = " + getState() );
			setState( (uint)HaStage.DISCONNECT );
			close2();
		}
	}
	
	private void close2()
	{
		//if ( this == null ) { Debug.LogError("close2 return"); return; }
		_erc4 = null;
		_drc4 = null;
		_rc4key = null;
		_bytes = new ProtoByteArray();
		if (_timer != null){

			_timer.Elapsed -= timeEventHandler;

			//_timer.removeEventListener(TimerEvent.TIMER, timeEventHandler);
			_timer.Stop();

			_timer = null;

		}
		if (_keepaliveTimer != null ){
			_keepaliveTimer.Elapsed -= keepaliveHandler;
//			_keepaliveTimer.removeEventListener(TimerEvent.TIMER, keepaliveHandler);
			_keepaliveTimer.Stop();
			_keepaliveTimer = null;
		}
	}		
	
	//--------------------------------------------------------------
	// 把信息写到远端的网络
	//--------------------------------------------------------------
	
	public void writeBytes(ProtoByteArray bytes)
	{
		if (_state == HaStage.DISCONNECT)
		{
//			GameDebuger.Log("writeBytes DISCONNECT");
			return;
		}
		// 首先是根据当前的状态，决定是否加密
		if( _erc4 != null )
		{
//			GameDebuger.Log("encrypted bytes length:" + bytes.Length.ToString() );
			
			_erc4.encrypt(bytes);
		}
		
		// 把信息写到远端的网络，并且flush
		if ( _socket.Connected )
		{
//			_socket.writeBytes(bytes);
//			_socket.flush();
			
			//test
//			string tmp = ""; for ( int i = 0; i < bytes.Length; ++i )	{ tmp += bytes.GetBuffer()[i].ToString("x2") + " "; } GameDebuger.Log( "sent byte:" + tmp );
			//------------
			
//			GameDebuger.Log( "***buffer length:" +  bytes.Length.ToString() );
			
			uint bufSize = bytes.Length;
			Async_Send( bytes.GetBuffer(),  bufSize, null );
			TotalSendBytes += bufSize;
		}
		else
		{
			GameDebuger.Log("[ERROR]往已经关闭的Socket写入");
		}
	}

	//--------------------------------------------------------------------------------------------------------
    private bool _sending = false;

	public void Async_Send(byte[] sendBuffer, uint bufSize, CallBack_Send callBack_Send)
    {
        SendObject sendObject = new SendObject(sendBuffer, bufSize, callBack_Send);

        lock (_sendObjPoolList)
        {
            _sendObjPoolList.Add(sendObject);
        }

        if (_sending == false)
        {
            SendObjectToSocket();
        }
    }

    private void SendObjectToSocket()
    {
        SendObject sendObject = null;
		
		lock (_sendObjPoolList){
	        if (_sendObjPoolList.Count > 0)
	        {
	            sendObject = _sendObjPoolList[0];
	            _sendObjPoolList.RemoveAt(0);
	        }
		}

        if (sendObject == null)
        {
            return;
        }

        error_Socket = Erro_Socket.SUCCESS;
        this.callBack_Send = sendObject.callBack_Send;

        if (_socket == null)
        {
            error_Socket = Erro_Socket.SOCKET_NULL;
            GameDebuger.Log("套接字为空，fa送失败");
            callBack_Send(false, error_Socket, "");
        }
        else if (!_socket.Connected)
        {
            error_Socket = Erro_Socket.SOCKET_UNCONNECT;
            GameDebuger.Log("未连接，fa送失败");
            callBack_Send(false, error_Socket, "");
        }
        else
        {
            _sending = true;
            //IAsyncResult asyncSend = _socket.BeginSend(sendBuffer, 0, (int)bufSize, SocketFlags.None, new AsyncCallback( sendCallback ), _socket );
            _socket.BeginSend(sendObject.sendBuffer, 0, (int)sendObject.bufSize, SocketFlags.None, new AsyncCallback(sendCallback), _socket);
        }
    }
	
	private void sendCallback(IAsyncResult asyncSend)
    {
        try
        {
            asyncSend.AsyncWaitHandle.Close();

            Socket clientSocket = (Socket)asyncSend.AsyncState;
			
			clientSocket.EndSend(asyncSend);
            //int bytesSent = clientSocket.EndSend(asyncSend);
			//GameDebuger.Log(bytesSent + "bytes sent.");
			
            if (callBack_Send != null)//回调
                callBack_Send(true, error_Socket,"");
        }
		catch (Exception e)
        {
			GameDebuger.Log( e.Message );
        }

        _sending = false;
        SendObjectToSocket();
    }
	
	//--------------------------------------------------------------------------------------------------------
		
	public void startKeepalive()
	{
		HaConfiguration config = HaApplicationContext.getConfiguration();
		
		//GameDebuger.Log("startKeepalive()");
		
		if (_keepaliveTimer == null) {
			_keepaliveTimer = new Timer(config.getIdleInterval());
		}
		
		if (_keepaliveTimer.Enabled) {
			return;
		}
		
		_keepaliveTimer.Elapsed += new ElapsedEventHandler( keepaliveHandler );
		_keepaliveTimer.AutoReset = true;  
		_keepaliveTimer.Enabled = true;
		
		//_keepaliveTimer.addEventListener(TimerEvent.TIMER, keepaliveHandler);
		//_keepaliveTimer.start();
	}
	
	/**
	 * 允许把自己的属性暴露到ha.net中 
	 * 
	 * @param	name
	 * @param	obj
	 * @return
	 */
	public bool linkPropertyObject( string name, IConigurable obj)
	{
		return false;
	}
	
	public void unlinkPropertyObject( string name )
	{
		
	}
	
	/**
	 * 获取某个节点的属性信息，只有接入ha.net中才会生效
	 * 
	 * @param	name
	 * @return
	 */
	public string getProperty( string name)
	{
		return null;			
	}
	
	/**
	 * 设置某个节点的属性信息，如果不是特殊的情况（如服务器赋予权限），
	 * client是不能其他节点的属性的
	 * 
	 * @param	name
	 * @param	value
	 */
	public void setProperty( string name, string value ) {
		
	}
	
	/**
	 * 加入ha.net网络
	 */
	public void join()
	{
        GameDebuger.SetNowTime();
		if ( _socket != null && _socket.Connected )
			_socket.Close(); // 先强行断开，我们不用担心远端的资源回收问题

		HaConfiguration config = HaApplicationContext.getConfiguration();
		
		string ip = config.getHost();
		int port = config.getPort();
		
		Async_Connect( ip, port, connectHandler, dataHandler );
		
		if (_timer == null)
			_timer = new Timer(config.getConnectTimeout());
		
		_timer.Elapsed += new ElapsedEventHandler( timeEventHandler );  
		_timer.Enabled = true;
	}
	
	public void Async_Connect(string ip, int port, CallBack_Connect callback_Connect, CallBack_Receive callBack_Receive)
    {
        error_Socket = Erro_Socket.SUCCESS;
        this.callBack_Connect = callback_Connect;
        this.callBack_Receive = callBack_Receive;
		
        if (_socket != null && _socket.Connected)
        {
			GameDebuger.Log("connect error");
            this.callBack_Connect(false, Erro_Socket.CONNECT_CONNECED_ERROR, "");//重复连接错误
        }
        else if (_socket == null || !_socket.Connected)
        {
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_socket.BeginConnect(ip, port, new AsyncCallback(connectCallback), _socket);
        }
    }
	
	private void connectCallback(IAsyncResult asyncConnect)
    {
        try 
        {
            Socket _socket = (Socket)asyncConnect.AsyncState;
            _socket.EndConnect(asyncConnect);
			
            if (_socket.Connected == false)
            {
                GameDebuger.Log("connection failed");
                error_Socket = Erro_Socket.CONNECT_UNSUCCESS_UNKNOW;
                
                return;
            }
            else
            {
                GameDebuger.Log("connection succeed");
                _stateObject = new StateObject( RECEIVE_BUFFER_SIZE, _socket);
                Receive();//开始接受消息
            }
      
			if (callBack_Connect != null)//回调
                callBack_Connect(_socket.Connected,error_Socket, "");
        }
		catch(Exception e)
        {
            if (callBack_Connect != null)//回调
            {
                GameDebuger.Log("Connection Exception = " + e.ToString());
                callBack_Connect(false, Erro_Socket.CONNECT_UNSUCCESS_UNKNOW,e.ToString());

                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
        } 
    }

    private StateObject _stateObject;
	
	public void Receive()
    {
        if (_socket != null && _socket.Connected)
        {
            _socket.BeginReceive(_stateObject.sBuffer, 0, RECEIVE_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(receiveCallback), _stateObject);
        }
    }
	
    private void receiveCallback(IAsyncResult asyncReceive)
    {
        try
        {
            asyncReceive.AsyncWaitHandle.Close();

            StateObject stateObject = (StateObject)asyncReceive.AsyncState;
            if (stateObject.sSocket == null)
            {

                GameDebuger.Log("套接字为空，获得消息失败" );
				CloseSocket();
                return;
            }
            else if (!stateObject.sSocket.Connected)
            {

                GameDebuger.Log("connection off");

				CloseSocket();
                return;
            }

            _receiveLen = stateObject.sSocket.EndReceive(asyncReceive); //返回实际获得数据长度length
            
			//GameDebuger.Log("[receiveCallback] Receive Len:" + _receiveLen.ToString() );

            if ( _receiveLen == 0 )
			{
				GameDebuger.Log("server disconnect");
				CloseSocket();
				return;		
			}
			
			_receiveBytes.WriteBytes( stateObject.sBuffer, 0, _receiveLen );
			_receiveBytes.Position = 0;
			
			callBack_Receive( true );
        }
        catch (Exception e)
        {
			GameDebuger.Log("[receiveCallback]catcher exception:" + e.Message );
			CloseSocket();
			return;				
        }
		
        Receive();
    }
	
	private void CloseSocket(){
		
		callBack_Receive( false );
		
		DispatchCloseEvent();		
		close();
	}

	private void DispatchCloseEvent()
	{
		EventObject obj = new EventObject(EventObject.Event_Close);
		AddEventObj(obj);
	}

	//----------------------------------------------------------------------------------------------------------------------
	
	
	/**
	 * 离开ha.net网络，但是还没有disconnect，ha.net会主动disconnect
	 */
	public void leave()
	{
		LeaveInstruction leave = new LeaveInstruction();
		leave.execute(this);
	}
	
	/**
	 * 查询服务所在节点
	 * 
	 * @param	type
	 */
	public void queryServices( string type )
	{
		HaConfiguration config = HaApplicationContext.getConfiguration();
		ServiceQueryRequestInstruction query = new ServiceQueryRequestInstruction();
		
		query.setProperties(config.getClientProperties());
		query.setType(type);
		query.execute(this);
	}

	/**
	 * 发送消息到目标节点
	 * 
	 * @param	id 			目标节点
	 * @param	type 		各种标志位，目前，可以用0x1表示losable，即该消息可丢弃
	 * @param	message		具体的业务信息
	 */
	public void sendMessage( string id, uint type, ProtoByteArray message )
	{
		DirectMessageInstruction send = new DirectMessageInstruction();
		//message.compress();
		send.setTargetId( uint.Parse( id ) );
		send.setRouteType(type);
		send.setMessage(message);
		send.execute(this);
	}

	//--------------------------------------------------------------
	// 事件响应函数
	//--------------------------------------------------------------

	protected void connectHandler(bool s, Erro_Socket errorcode, string exception)
	{
		if (_socket.Connected)
		{
            GameDebuger.LogTime("Connected To HA");
			//requestTGW();
			
			HaConfiguration config = HaApplicationContext.getConfiguration();

			// 这里开始尝试接入ha.net
			SSLOpenRequestInstruction sslopen = new SSLOpenRequestInstruction();

			GameDebuger.Log("SEND SSL_OPEN");

			sslopen.setProperties(config.getCreateChannelProperties());
			sslopen.setSupportEncryptMethods(config.getSupportEncryptMethods());
			sslopen.setAllowedMethod(config.getAllowedEncryptMethod());
			sslopen.setEncryptKey(config.getEncryptKey());
			sslopen.execute(this);

			_timer.Stop();
		}
		else
		{
			DispatchCloseEvent();
		}
	}

	protected void dataHandler( bool success )
	{
		if ( !success )		{	GameDebuger.Log("(dataHandler). receive exception ");	return;		}
		
		int n = _receiveLen;

		//GameDebuger.Log( "Receive Length:" + n.ToString() );

		// 这里需要处理分包组包的情况，并根据解码结果，把消息转换成为有含义的指令执行
		if (n > 0)
		{
			// 首先是解码
			if( _drc4 != null )
			{
				_drc4.encrypt( _receiveBytes, _receiveLen );
				_receiveBytes.Position = 0;
				//string tmp = ""; for ( int i = 0; i < _bytes.Length; ++i )	{ tmp += _bytes.GetBuffer()[i].ToString("x2") + " "; } GameDebuger.Log( "receive byte:" + tmp );
			}
			
//			// 追加到缓存字节流的后面
			///=========================================================================================
			if ( _bytes.Length <= 0 )
			{
				_bytes.WriteBytes( _receiveBytes.GetBuffer(), (int)_bytes.Length,(int) _receiveLen );
			}
			else
			{
				int preSize = (int) _bytes.Length;
				int totalSize = preSize + _receiveLen;
				
				byte[] tmp = new byte[ totalSize ];
				
				System.Buffer.BlockCopy( _bytes.GetBuffer(), 0, tmp, 0, preSize );
				System.Buffer.BlockCopy( _receiveBytes.GetBuffer(), 0, tmp, preSize, _receiveLen );
				//-----------------------------------------------------------				
				_bytes = new ProtoByteArray(tmp);
				
//				GameDebuger.Log( "Append Byte len:" + _bytes.Length.ToString() );
			}
			
			_bytes.Position = 0;
			///=========================================================================================
			
			readByteBuff();
		}
		else
		{
			GameDebuger.Log("dataHandler error!");
		}
	}

	/**
	 *byteBuff读取处理 
	 * 
	 */		
	private void readByteBuff()
	{
		// 当包不够长度的时候，继续等待下一个网络报文的到来
		if (_bytes.bytesAvailable < 4) {
			return;
		}
		
		uint size = _bytes.ReadUnsignedInt();	
		if (size > _bytes.Length) {	
			_bytes.Position = 0;
			return;
		}
		
		// 当内容过多时，会把剩余的内容拷贝到一个临时缓存
		byte[] buffer = new byte[_bytes.Length - size];
		ProtoByteArray bytes2 = new ProtoByteArray(buffer);
		if (size < _bytes.Length){
			_bytes.Position = (int)size;
			_bytes.ReadBytes(bytes2.GetBuffer(), 0, _bytes.Length - size);
		}

        //GameDebuger.Log("ReadByteBuff " + _bytes.bytesAvailable + " byte");
        TotalReceiveBytes += _bytes.bytesAvailable;

		// 根据当前业务，触发调度逻辑
		_bytes.Position = 12;
		InstructionDefine type = (InstructionDefine)_bytes.ReadShort();
		_bytes.Position = 0;

//		GameDebuger.Log("type="+type);
		switch ( type)
		{
			case InstructionDefine.SSL_OPEN_RES:
                GameDebuger.LogTime("SSL_OPEN_RES");
				GameDebuger.Log("SSL_OPEN_RES");
				SSLOpenResponseInstruction sslopen = new SSLOpenResponseInstruction();
				sslopen.fromBytes(_bytes);
				sslopen.execute(this);
				break;
			
			case InstructionDefine.VER:
                GameDebuger.LogTime("VER");
				GameDebuger.Log("VER");
				VerAckInstruction ver = new VerAckInstruction();
				ver.fromBytes(_bytes);
				ver.execute(this);
				break;
			
			case InstructionDefine.JOIN_RES:
                GameDebuger.LogTime("JOIN_RES");
				GameDebuger.Log("JOIN_RES");
				JoinAckInstruction joinack = new JoinAckInstruction();
				joinack.fromBytes(_bytes);
				joinack.execute(this);
				break;
			
			case InstructionDefine.LEAVE_EVENT:
				GameDebuger.Log("LEAVE_EVENT");
				LeaveEventInstruction leave = new LeaveEventInstruction();
				leave.fromBytes(_bytes);
				leave.execute(this);
				break;
			
			case InstructionDefine.SERVICE_QUERY_RES:
				GameDebuger.Log("SERVICE_QUERY_RES");
				ServiceQueryResponseInstruction service = new ServiceQueryResponseInstruction();
				service.fromBytes(_bytes);
				service.execute(this);
				break;
			
			case InstructionDefine.DIRECT_MESSAGE:
				DirectMessageReceivedInstruction direct = new DirectMessageReceivedInstruction();
				direct.fromBytes(_bytes);
				direct.execute(this);
				break;
			
			case InstructionDefine.GROUP_MESSAGE:
				GroupMessageReceivedInstruction group = new GroupMessageReceivedInstruction();
				group.fromBytes(_bytes);
				group.execute(this);
				break;
			
			case InstructionDefine.STATE_EVENT:
				StateEventNotifyInstruction  stateEventNofity = new StateEventNotifyInstruction();
				stateEventNofity.fromBytes(_bytes);
				stateEventNofity.execute(this);
				break;
				
			case InstructionDefine.PING_ACK:
				break;
			case InstructionDefine.KEEPALIVE_ACK: // skip
				break;
			case InstructionDefine.GET_PROPERTY_RES:
				break;
			case InstructionDefine.SET_PROPERTY_RES:
				break;
			
			default:
				GameDebuger.Log("not support instruction");
				break;
		}				
		
		// 处理完逻辑后，把多余的内容保存在缓存中
		_bytes = bytes2;
		
		//再次启动buff读取处理
		readByteBuff();
	}

	public void AddEventObj(EventObject obj)
	{
		_eventObjQueue.Enqueue(obj);
	}

	protected void closeHandler()
	{
		close();
		DispatchCloseEvent();
	}

	protected void ioErrorHandler()
	{
		close();
		DispatchCloseEvent();
	}		

	protected void securityErrorHandler()
	{
		close();
		DispatchCloseEvent();
	}				
	
	protected void timeEventHandler( object source, ElapsedEventArgs e)
	{
		_timer.Stop();
		if (!_socket.Connected)
		{
			close();
		}
	}
	
	protected void keepaliveHandler( object source, ElapsedEventArgs e)
	{
		if (_state == HaStage.LOGINED) {
			KeepaliveInstruction keepalive = new KeepaliveInstruction();
			keepalive.execute(this);
		}
		else
		{
			_keepaliveTimer.Stop();
		}
	}
	
	public void destroy()
	{
		_socket = null;
		HaApplicationContext.setConnector(null);
	}
	
	//---------------------------------------------------------------------------------------------------------------------
	class StateObject
    {
        internal byte[] sBuffer;
        internal Socket sSocket;
        internal StateObject(int size, Socket sock)
        {
            sBuffer = new byte[size];
            sSocket = sock;
        }
    }

    class SendObject
    {
        internal byte[] sendBuffer;
        internal uint bufSize;
        internal CallBack_Send callBack_Send;

        internal SendObject(byte[] _sendBuffer, uint _bufSize, CallBack_Send _callBack_Send)
        {
            sendBuffer = _sendBuffer;
            bufSize = _bufSize;
            callBack_Send = _callBack_Send;
        }
    }
}
