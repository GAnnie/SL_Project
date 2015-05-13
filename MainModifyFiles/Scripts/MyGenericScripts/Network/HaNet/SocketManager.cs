/**
 *Socket通讯管理
 * @author senkay
 *
 */

using UnityEngine;
using com.nucleus.commons.message;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Collections;

public class SocketManager
{
	private static readonly SocketManager _instance = new SocketManager ();
	
	public static SocketManager Instance {
		get {
			return _instance;
		}
	}

	public event Action OnHAConnected;
	public event Action<string> OnHaError;
	public event Action OnHaCloseed;
	
	public static readonly string ERROR_time_out = "链接超时";
	public static readonly string ERROR_socket_error = "网络错误";
	public static readonly string ERROR_socket_close = "网络已断开";
	public static readonly string ERROR_sid_error = "用户账号错误";
	public static readonly string ERROR_user_invalid = "用户无效";
	
	static public bool IsToolMode = false;
	
	static public bool IsOnLink = false;
	//服务器主动断开
	static public bool HAClose = false;
	
	private HaConnector _haConnector  = null;
	
	private int m_iSerialNum = 1;
	
	private int _heartBeatTime = 0;
	
	private Timer _timer = null;
	
	private string _appServerId="";
	private string _appServerVer="";
	private ServiceInfo _currentServiceInfo;
	private ServerInfo _currentServerInfo;
	
	private Dictionary<string, List<MessageProcessor>> _processorMaps;

	public SocketManager()
	{
		_processorMaps = new Dictionary<string, List<MessageProcessor>>();
	}

	public void Setup()
	{
		if (_timer == null)
		{
			_timer = new Timer(1000);  
			
			_timer.Elapsed += new ElapsedEventHandler( onTimer );  
			_timer.AutoReset = true;  
			_timer.Enabled = true;  
		}
		
		if (_haConnector == null)
		{
			GameObject go = new GameObject("HaConnector");
			GameObject.DontDestroyOnLoad( go );
			_haConnector = go.GetMissingComponent<HaConnector>();
			_haConnector.OnJoinEvent += HandleOnJoinEvent;
			_haConnector.OnServiceEvent += HandleOnServiceEvent;
			_haConnector.OnMessageEvent += HandleOnMessageEvent;
			_haConnector.OnCloseEvent += HandleOnCloseEvent;
			_haConnector.OnLeaveEvent += HandleOnLeaveEvent;
		}
		
		_appServerVer = GameInfo.HA_SERVICE_MAIN_TYPE;
	}

	void HandleOnLeaveEvent (string obj)
	{
		HAClose = true;
	}
	
	void HandleOnCloseEvent ()
	{
		ResetSerialNum();
		IsOnLink = false;
	}
	
	void HandleOnMessageEvent (ProtoByteArray byteArray)
	{
		_heartBeatTime = 0;
		if (_timer.Enabled == false){
			//_timer.start();
			_timer.AutoReset = true;
			_timer.Enabled = true;
		}
		
		OnByteData(byteArray);
	}
	
	void HandleOnJoinEvent ()
	{
		_haConnector.queryServices(_appServerVer);
	}
	
	void HandleOnServiceEvent (ArrayList list)
	{
		if (list == null)
		{
			GameDebuger.Log("onAppServices:" + "null");
		}
		else
		{
			GameDebuger.Log("onAppServices:" + list.Count);
		}
		
		if (list != null && list.Count > 0)
		{
			ServiceInfo selectInfo = null;
			
			if (_currentServerInfo == null)
			{
				selectInfo = list[0] as ServiceInfo;
			}
			else
			{
				foreach (ServiceInfo info in list)
				{
					if (info.id == _currentServerInfo.accessId.ToString())
					{
						selectInfo = info;
						break;
					}
				}
			}
			
			if (selectInfo != null)
			{
				SetCurrentAppServerInfo(selectInfo);
			}
			else
			{
				HandlerHaError(ERROR_socket_close);
			}
		}
		else
		{
			HandlerHaError(ERROR_socket_close);
		}
	}
	
	private void SetCurrentAppServerInfo(ServiceInfo serviceInfo)
	{
		_currentServiceInfo = serviceInfo;
		_appServerId = serviceInfo.id;
		IsOnLink = true;
		
		if (OnHAConnected != null)
		{
			OnHAConnected();
		}
	}
	
	private void HandlerHaError(string msg)
	{
		if (OnHaError != null)
		{
			OnHaError(msg);
		}
	}
	
	public void initHASocket( )
	{
		//		if (_haConnector == null)
		//		{
		//			GameObject go = new GameObject("HaConnector");
		//			GameObject.DontDestroyOnLoad( go );
		//			_haConnector = go.GetMissingComponent<HaConnector>();
		//			_haConnector.OnJoinEvent += HandleOnJoinEvent;
		//			_haConnector.OnServiceEvent += HandleOnServiceEvent;
		//			_haConnector.OnMessageEvent += HandleOnMessageEvent;
		//			_haConnector.OnCloseEvent += HandleOnCloseEvent;
		//			_haConnector.OnLeaveEvent += HandleOnLeaveEvent;
		//		}
	}
	
	public void Destroy()
	{
		//		if (_haConnector != null)
		//		{
		//			_haConnector.OnJoinEvent -= HandleOnJoinEvent;
		//			_haConnector.OnServiceEvent -= HandleOnServiceEvent;
		//			_haConnector.OnMessageEvent -= HandleOnMessageEvent;
		//			_haConnector.OnCloseEvent -= HandleOnCloseEvent;
		//			_haConnector.OnLeaveEvent -= HandleOnLeaveEvent;
		//			_haConnector.destroy();
		//			_haConnector = null;
		//		}
		
		if (_timer != null )
		{
			//			_timer.removeEventListener(TimerEvent.TIMER, onTimer);
			_timer.Stop();
			//			_timer.Elapsed -= ElapsedEventHandler( onTimer );  
			_timer.Enabled = false;
			_timer = null;
		}
		
		ResetSerialNum();
	}
	
	public void Connect( ServerInfo serverInfo)
	{
		_currentServerInfo = serverInfo;

		HAClose = false;

		HaApplicationContext.getConfiguration().setHost(serverInfo.host);
		HaApplicationContext.getConfiguration().setPort(serverInfo.port);
		_haConnector.join();
	}
	
	public HaConnector GetHaConnector()
	{
		return _haConnector;
	}
	
	public int getSerialNum()
	{
		return m_iSerialNum;
	}
	
	public void ResetSerialNum(){
		m_iSerialNum = 1;
	}
	
	public void SendRequest( GeneralRequest request, byte requestType, ActionCallback callback = null)
	{
		if(!IsConnectedToHA()) {
			GameDebuger.Log("have not connect to ha yet!!");
			return;
		}
		if(request == null) {
			GameDebuger.Log("IhomeRequest is null!!");
			return;
		}
		//注册callback
		if(callback != null)
		{
			int currentSerialNum = m_iSerialNum++;
			request.serial = currentSerialNum;
			ActionCallbackManager.put(currentSerialNum, callback);
		}
		ProtoByteArray ba = new ProtoByteArray();
		//ba.objectEncoding = ObjectEncoding.AMF3;
		
		//GameDebuger.Log( request.GetType().ToString() );
		
		//TODO WriteByte with Type
		
		ba.WriteByte( requestType );
		ba.WriteObject(request);
		_haConnector.sendMessage(_appServerId, 0, ba);
		
		//trace("[SEND]"+request.action+"["+request.serial+"]");
	}
	
	public bool IsConnectedToHA()
	{
		return _haConnector.getState() >= HaStage.CONNECTED;
	}
	
	public void addMessageProcessor( MessageProcessor processor)
	{
		string type = processor.getEventType();
		
		List<MessageProcessor> list;
		if (_processorMaps.ContainsKey(type)){
			list = _processorMaps[type];
		}else{
			list = new List<MessageProcessor>();
			_processorMaps.Add(processor.getEventType(), list);
		}
		list.Add(processor);
	}
	
	public void removeMessageProcessor( MessageProcessor processor)
	{
		string type = processor.getEventType();
		
		if (_processorMaps.ContainsKey(type)){
			List<MessageProcessor> list = _processorMaps[type];
			list.Remove(processor);
		}
	}
	
	public void OnByteData(ProtoByteArray bytes)
	{
		//Debug.Log("OnByteData Lenght="+bytes.Length);
		
		object readObj = null;
		
		//由于怕服务器在静态数据获取期间， 下发了其它的数据， 所以判断一下数据长度，小于500字节的进行判断
		if (bytes.Length < 500)
		{
			bytes.Position = 0;
			readObj = bytes.ReadObject();
			if (readObj is GeneralResponse)
			{
				//先判断是否静态数据， 如果不是再转到onData处理
			}else{
				onData(readObj);
				return;
			}
		}
		
		int serial = ActionCallbackManager.getNextActionCallbackSerial();
		//Debug.Log("OnByteData serial="+serial);
		ServiceRequestAction callback = ActionCallbackManager.getActionCallback(serial) as ServiceRequestAction;
		if (callback != null && callback.HasCallbackWithBytes())
		{
			ActionCallbackManager.removeActionCallback(serial);
			callback.onSuccessWithBytes(bytes);
		}
		else
		{
			if (readObj == null){
				bytes.Position = 0;
				readObj = bytes.ReadObject();
			}
			onData(readObj);
		}
	}	
	
	public void onData(object message)
	{
		if (message == null){
			Debug.LogError("XSocket onData = null");
			return;
		}
		
		//GameDebuger.Log("message type:" + message.GetType().ToString() );
		
		int serial = getSerial(message);
		if (serial > 0)
		{
			ActionCallback callback = ActionCallbackManager.remove( serial );
			//如果是之前注册的序列号
			if(callback != null)
			{
				if (message is ErrorResponse)
				{
					callback.onError((ErrorResponse)(message));
				}
				else
				{
					callback.onSuccess( (GeneralResponse)(message));
				}
				return;
			}
		}
		
		if (message is ErrorResponse){
			GameDebuger.Log("ErrorResponse = "+((ErrorResponse)message).message);
			////TipManager.AddTip(((ErrorResponse)message).message);
		}
		
		string messageType = message.GetType().ToString();
		List<MessageProcessor> list = null;
		_processorMaps.TryGetValue(messageType, out list);
		if (list != null){
			foreach (MessageProcessor processor in list){
				processor.process(message);
			}
		}
		else
		{
			Debug.LogError("未监听下发 " + messageType);
		}
		
		if ( allEventListner != null )
			allEventListner( message );
	}
	
	public delegate void AllMessageEvent( object message );
	
	private AllMessageEvent allEventListner;
	
	public void AddMessageListener( AllMessageEvent ame )
	{
		if ( ame == null )
			return;
		
		allEventListner += ame;
	}
	
	public void RemoveMessageListener( AllMessageEvent ame )
	{
		if ( ame == null )
			return;
		
		allEventListner -= ame;
	}
	
	private int getSerial( object message )
	{
		if ( message is GeneralResponse )
		{
			return ( (GeneralResponse)( message )).serial;
		}
		return -1;
	}
	
	/**
	 * 用于模拟接受服务器消息
	 * @param message
	 * 
	 */		
	public void testOnData( object message, ActionCallback callback = null)
	{
		if(message is GeneralResponse) 
		{
			GeneralResponse response = message as GeneralResponse;
			if(callback != null) {
				//if(response is Amf3Response) {
				//callback.onError(response as Amf3Response);
				//} else {
				callback.onSuccess(response);
				//}
				return;
			}
		}
		//dispatchEvent(new MessageEvent(getQualifiedClassName(message),message));			
	}
	
	public void onTimer( object source, ElapsedEventArgs e)   
	{   
		_heartBeatTime += 1;
		if (_heartBeatTime > 180)
		{
			_timer.Stop();
			Close(true);
		}
	}
	
	public void Close( bool needDispatcher = false )
	{
		if (_haConnector != null)
		{
			_haConnector.close();
			
			if ( needDispatcher )
			{
				if (OnHaCloseed != null)
				{
					OnHaCloseed();
				}
			}
		}
	}
}
