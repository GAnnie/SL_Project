#define DONT_USE_WEB_LIB
#define LOG_RETRY_MESSAGE


using System;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using System.IO;

public class SimpleWWW : MonoBehaviour
{
	/// <summary>
	/// HTTP连接模式
	/// 	Continued_Connect :　 持续的连接
	/// 	Short_Connect	  :   短连接
	/// </summary>
	public enum ConnectionType
	{
		Continued_Connect = 0,
		Short_Connect     = 1
	}
	
    public void ReceiveAnyThread(Request request, System.Action<byte[]> bytesDlegate, System.Action<float> progressDlegate = null, System.Action<Exception> errorDlegate = null, bool uncompress = false, ConnectionType connetType = ConnectionType.Continued_Connect)
    {
        StartCoroutine ( _ReceiveAnyThread(request, bytesDlegate, progressDlegate, errorDlegate, uncompress, connetType));
    }

    public void Receive(Request request, System.Action<byte[]> bytesDlegate, System.Action<float> progressDlegate = null, System.Action<Exception> errorDlegate = null, bool uncompress = false,ConnectionType connetType = ConnectionType.Continued_Connect)
    {
        StartCoroutine(_Receive(request, bytesDlegate, progressDlegate, errorDlegate, uncompress, connetType));
    }

    public void Receive(Request request, System.Action<byte[], object> bytesDlegate, object obj, System.Action<float> progressDlegate = null , System.Action<Exception> errorDlegate = null,bool uncompress = false,ConnectionType connetType = ConnectionType.Continued_Connect)
    {
        StartCoroutine(_Receive(request, bytesDlegate, obj, progressDlegate, errorDlegate, uncompress , connetType));
    }

    public void ReceiveFragments( Request request, System.Action finishDlegate, int receiveLen , System.Action< byte[] , bool > bytesDlegate, System.Action<float> progressDlegate = null , System.Action<Exception> errorDlegate = null,ConnectionType connetType = ConnectionType.Continued_Connect )
    {
        StartCoroutine(_ReceiveFragments(request, finishDlegate, receiveLen, bytesDlegate, progressDlegate, errorDlegate, connetType));
    }


	public void Send( Request request , byte[] data, System.Action action = null, System.Action<Exception> errorDlegate = null)
	{
        StartCoroutine(_Sent(request, data, action, errorDlegate));
	}
	
	//最多重连数量
    private readonly int MAXIMUMREDIRECTS = 10;
	
    IEnumerator _ReceiveAnyThread(Request request, System.Action<byte[]> bytesDelegate, System.Action<float> progressDlegate = null, System.Action<Exception> errorDlegate = null, bool uncompress = false, ConnectionType connetType = ConnectionType.Continued_Connect)
    {
        if (request != null)
        {
            int index = AddRequstList(request);
			
            //如果是加载本来的， 需要使用WWW来加载
            if (request.requestURL.ToLower().StartsWith("http://"))
            {
#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)					
                request.Receive(uncompress);
                while (!request.isDone)
                {
                    if (progressDlegate != null) progressDlegate(request.progress);
                }
				yield return null;
				
                if (request.exception != null)
                {
                    if (errorDlegate != null) errorDlegate(request.exception);
                    throw request.exception;
                }
                else
                {
					if (progressDlegate != null) progressDlegate(1.0f);
                    if (bytesDelegate   != null) bytesDelegate(request.bytes);
                }
#else
				WWW www = null;  
				int retry = 0;
                while (retry++ < MAXIMUMREDIRECTS)
                {
					if( connetType == ConnectionType.Continued_Connect )
					{
						www = new WWW( request.requestURL );
					}
					else
					{
						Hashtable postHeader = new Hashtable();
						postHeader.Add("Connection", "close");
						www = new WWW( request.requestURL, null, postHeader );
					}
					
					
					while (!www.isDone)
	                {
	                    if (progressDlegate != null) progressDlegate(www.progress);
						yield return new WaitForEndOfFrame();
	                }
					
					if( www.isDone && www.error == null )
					{
						request.bytes = www.bytes;
						
		                if (uncompress)
		                {
		                    request.Uncompress();
		                    while (!request.isDone)
		                    {
		                        yield return new WaitForEndOfFrame();
		                    }
		                }					
						
						if (progressDlegate != null) progressDlegate(1.0f);
	                    if (bytesDelegate   != null) bytesDelegate(request.bytes);
						
						break;
					}
					else
					{
						if( retry >= MAXIMUMREDIRECTS)
						{
							if (errorDlegate != null) errorDlegate( new Exception( www.error ));
						}
						else
						{
#if LOG_RETRY_MESSAGE					
							Debug.Log( string.Format( "Try again Link url : {0} , time : {1}", request.requestURL, retry ));
#endif							
							yield return new WaitForSeconds( 3.0f );
						}
					}					
				}
		
				if( www != null )
				{
					www.Dispose();
					www = null;				
				}

#endif
            }
            ClearRequstList(index);
			
        }
    }


    IEnumerator _Receive(Request request, System.Action<byte[]> bytesDelegate, System.Action<float> progressDlegate = null, System.Action<Exception> errorDlegate = null,  bool uncompress = false,ConnectionType connetType = ConnectionType.Continued_Connect )
    {
        if ( request != null )
        {
            int index = AddRequstList(request);

            //如果是加载本来的， 需要使用WWW来加载
            if (!request.requestURL.ToLower().StartsWith("http://"))
            {
                GameDebuger.Log("request.requestURL : " + request.requestURL);
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IPHONE)
                string assetPath = request.requestURL;
                request.bytes = GetAssetData(assetPath);
#elif UNITY_ANDROID
                int resIndex = request.requestURL.IndexOf(PathHelper.RESOURCES_PATH);
                string assetPath = null;
                if (resIndex != -1)
                {
                    assetPath = request.requestURL.Substring(resIndex);
                }
                else
                {
                    assetPath = request.requestURL;
                }


                request.bytes = BaoyugameSdk.readBytes(assetPath);
#endif
                if (request.bytes.Length>0)
                {
                    //request.bytes = www.bytes;
                    //www.Dispose();
                    //www = null;
                    if (uncompress)
                    {
                        request.Uncompress();

                        while (!request.isDone)
                        {
                            if (progressDlegate != null) progressDlegate(request.progress);
                            yield return new WaitForEndOfFrame();
                        } 
                    }
                    else
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    if (request.exception != null)
                    {
                        if (errorDlegate != null) errorDlegate(request.exception);
                        throw request.exception;
                    }
                    else
                    {
						if (progressDlegate != null) progressDlegate(request.progress);
                        if (bytesDelegate   != null) bytesDelegate(request.bytes);
                    }
                }
                else
                {
                    if (errorDlegate != null) errorDlegate(new Exception("SimpleWWW Get Asset " + assetPath + " Error"));
                    //throw new Exception(www.error);
                }
            }
            else
            {
#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)				
                request.Receive(uncompress);
                while (!request.isDone)
                {
                    if (progressDlegate != null) progressDlegate(request.progress);
                    yield return new WaitForEndOfFrame();
                }
                    

                if (request.exception != null)
                {
                    if (errorDlegate != null) errorDlegate(request.exception);
                    throw request.exception;
                }
                else
                {
					if (progressDlegate != null) progressDlegate(1.0f);
                    if (bytesDelegate   != null) bytesDelegate(request.bytes);
                }
#else
				
				WWW www = null;  
				int retry = 0;
                while (retry++ < MAXIMUMREDIRECTS)
                {
//					www = new WWW( request.requestURL );
					if( connetType == ConnectionType.Continued_Connect )
					{
						www = new WWW( request.requestURL );
					}
					else
					{
						Hashtable postHeader = new Hashtable();
						postHeader.Add("Connection", "close");
						www = new WWW( request.requestURL, null, postHeader );
					}					
					
					while (!www.isDone)
	                {
	                    if (progressDlegate != null) progressDlegate(www.progress);
						yield return new WaitForEndOfFrame();
	                }
					
					
					if( www.isDone && www.error == null )
					{
						request.bytes = www.bytes;
						
		                if (uncompress)
		                {
		                    request.Uncompress();
		                    while (!request.isDone)
		                    {
		                        yield return new WaitForEndOfFrame();
		                    }
		                }					
						
						if (progressDlegate != null) progressDlegate(1.0f);
	                    if (bytesDelegate   != null) bytesDelegate(request.bytes);
						
						break;
					}
					else
					{
						if( retry >= MAXIMUMREDIRECTS)
						{
							if (errorDlegate != null) errorDlegate( new Exception( www.error ));
						}
						else
						{
							
#if LOG_RETRY_MESSAGE					
							Debug.Log( string.Format( "Try again Link url : {0} , time : {1}", request.requestURL, retry ));
#endif							
							yield return new WaitForSeconds( 3.0f );
						}
					}
				}
				
				if( www != null )
				{
					www.Dispose();
					www = null;				
				}
#endif
            }
            ClearRequstList(index);
        }
    }

    IEnumerator _Receive(Request request, System.Action<byte[], object> bytesDelegate, object obj, System.Action<float> progressDlegate = null, System.Action<Exception> errorDlegate = null, bool uncompress = false, ConnectionType connetType = ConnectionType.Continued_Connect)
    {
        if (request != null)
        {
            int index = AddRequstList(request);

            //如果是加载本来的， 需要使用WWW来加载
            if (!request.requestURL.ToLower().StartsWith("http://"))
            {
                GameDebuger.Log("request.requestURL : " + request.requestURL);
#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_IPHONE)
                string assetPath = request.requestURL;
                request.bytes = GetAssetData(assetPath);
#elif UNITY_ANDROID
                int resIndex = request.requestURL.IndexOf(PathHelper.RESOURCES_PATH);
                string assetPath = null;
                if (resIndex != -1)
                {
                    assetPath = request.requestURL.Substring(resIndex);
                }
                else
                {
                    assetPath = request.requestURL;
                }


                request.bytes = BaoyugameSdk.readBytes(assetPath);
#endif

                if (request.bytes.Length > 0)
                {
                    if (uncompress)
                    {
                        request.Uncompress();

                        while (!request.isDone)
                        {
                            if (progressDlegate != null) progressDlegate(request.progress);
                            yield return new WaitForEndOfFrame();
                        }

                    }
                    else
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    if (request.exception != null)
                    {
                        if (errorDlegate != null) errorDlegate(request.exception);
                        throw request.exception;
                    }
                    else
                    {
						if (progressDlegate != null) progressDlegate(request.progress);
                        if (bytesDelegate   != null) bytesDelegate(request.bytes, obj);
                    }
                }
                else
                {
                    if (errorDlegate != null) errorDlegate(new Exception("SimpleWWW Get Asset " + assetPath + " Error"));
                    //throw new Exception(www.error);
                }
            }
            else
            {
#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)				
                request.Receive(uncompress);
                while (!request.isDone)
                {
                    if (progressDlegate != null) progressDlegate(request.progress);
                    yield return new WaitForEndOfFrame();
                }
				
                if (request.exception != null)
                {
                    if (errorDlegate != null) errorDlegate(request.exception);
                    throw request.exception;
                }
                else
                {
					if (progressDlegate != null) progressDlegate(1.0f);
                    if (bytesDelegate   != null) bytesDelegate(request.bytes, obj);
                }
#else
				WWW www = null;  
				int retry = 0;
                while (retry++ < MAXIMUMREDIRECTS)
                {								
//					www = new WWW( request.requestURL );
					if( connetType == ConnectionType.Continued_Connect )
					{
						www = new WWW( request.requestURL );
					}
					else
					{
						Hashtable postHeader = new Hashtable();
						postHeader.Add("Connection", "close");
						www = new WWW( request.requestURL, null, postHeader );
					}						
					
					
					while (!www.isDone)
	                {
	                    if (progressDlegate != null) progressDlegate(www.progress);
						yield return new WaitForEndOfFrame();
	                }
					
					
					if( www.isDone && www.error == null )
					{
						request.bytes = www.bytes;
						
		                if (uncompress)
		                {
		                    request.Uncompress();
		                    while (!request.isDone)
		                    {
		                        yield return new WaitForEndOfFrame();
		                    }
		                }					
						
						if (progressDlegate != null) progressDlegate(1.0f);
	                    if (bytesDelegate   != null) bytesDelegate(request.bytes, obj);
						
						break;
					}
					else
					{
						if( retry >= MAXIMUMREDIRECTS)
						{
							if (errorDlegate != null) errorDlegate( new Exception( www.error ));
						}
						else
						{
							
#if LOG_RETRY_MESSAGE					
							Debug.Log( string.Format( "Try again Link url : {0} , time : {1}", request.requestURL, retry ));
#endif							
							yield return new WaitForSeconds( 3.0f );
						}
						
					}
				}
				
				if( www != null )
				{
					www.Dispose();
					www = null;				
				}
#endif				
            }

            ClearRequstList(index);
        }
    }

    IEnumerator _ReceiveFragments(Request request, System.Action finishDlegate, int receiveLen, System.Action<byte[], bool> bytesDlegate, System.Action<float> progressDlegate = null, System.Action<Exception> errorDlegate = null, ConnectionType connetType = ConnectionType.Continued_Connect)
    {
        if (request != null)
        {
            int index = AddRequstList(request);
			
#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)			
            request.ReceiveFragments(bytesDlegate, (long)receiveLen);
            while (!request.isDone)
            {
                if (progressDlegate != null) progressDlegate(request.progress);
                yield return new WaitForEndOfFrame();
            }

            if (request.exception != null)
            {
                if (errorDlegate != null) errorDlegate(request.exception);
                throw request.exception;
            }
            else
            {
				if (progressDlegate != null) progressDlegate(1.0f);
                if ( finishDlegate != null ) finishDlegate();
            }
#else
			WWW www = null;  
			int retry = 0;
            while (retry++ < MAXIMUMREDIRECTS)
            {				
				//www = new WWW( request.requestURL );
				if( connetType == ConnectionType.Continued_Connect )
				{
					www = new WWW( request.requestURL );
				}
				else
				{
					Hashtable postHeader = new Hashtable();
					postHeader.Add("Connection", "close");
					www = new WWW( request.requestURL, null, postHeader );
				}					
				
				
				while (!www.isDone)
	            {
	                if (progressDlegate != null) progressDlegate(www.progress);
					yield return new WaitForEndOfFrame();
	            }
				
				
				if( www.isDone && www.error == null )
				{
					request.bytes = www.bytes;
					
					if (progressDlegate != null) progressDlegate(1.0f);
					if ( bytesDlegate != null ) bytesDlegate( request.bytes, true );
	                if ( finishDlegate != null ) finishDlegate();
					
					break;
				}
				else
				{
					if( retry >= MAXIMUMREDIRECTS)
					{
						if (errorDlegate != null) errorDlegate( new Exception( www.error ));
					}
					else
					{
						
#if LOG_RETRY_MESSAGE					
							Debug.Log( string.Format( "Try again Link url : {0} , time : {1}", request.requestURL, retry ));
#endif						
						yield return new WaitForSeconds( 3.0f );
					}					
				}
			}
			
			if( www != null )
			{
				www.Dispose();
				www = null;				
			}
			
#endif
            ClearRequstList(index);
        }
    }

    IEnumerator _Sent(Request request, byte[] data, System.Action action = null, System.Action<Exception> errorDlegate = null)
	{
        int index = AddRequstList(request);
		
#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)
		request.Send( data );
		while( !request.isDone )
			yield return new WaitForEndOfFrame();
		
		if ( request.exception != null )
		{
            if (errorDlegate != null) errorDlegate(request.exception);
			throw request.exception;
		}
		else
		{
			if ( action != null ) action();
		}		
#else
		Hashtable postHeader = new Hashtable();
		postHeader.Add("Content-Type", "application/octet-stream");
		postHeader.Add("Content-Length", data.Length);
		postHeader.Add("Connection", "close");
		
		WWW sendWWW = new WWW( request.requestURL, data, postHeader);
		
		
		yield return sendWWW;
		
		if( sendWWW.isDone && sendWWW.error == null )
		{
			if ( action != null ) action();
		}
		else
		{
			if (errorDlegate != null) errorDlegate(new Exception( sendWWW.error ));
			throw request.exception;
		}		
#endif

        ClearRequstList(index);
	}
	
	
    List<System.Action> onQuit = new List<System.Action>();
    public void OnQuit(System.Action fn)
    {
        onQuit.Add(fn);
    }

    Request[] requestList = new Request[20];
    public int AddRequstList( Request request )
    {
        for( int i = 0 ; i < requestList.Length ; i ++ )
        {
            if ( requestList[i] == null )
            {
                requestList[i] = request;
                return i;
            }
        }

        return -1;
    }

    public void ClearRequstList( int index )
    {
        if (index == -1) return;
        if (index < requestList.Length)
        {
            requestList[index] = null;
        }
    }

    /// <summary>
    /// 获取资源数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private byte[] GetAssetData(string path)
    {
        byte[] assetData = null;
        if (File.Exists(path))
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read,FileShare.Read);
                BinaryReader reader = new BinaryReader(stream);
                assetData = reader.ReadBytes((int)stream.Length);

                reader.Close();
                stream.Close();
            }
            catch (IOException e)
            {
               GameDebuger.Log(e.Message + " - " + path);
                //Debug.LogException(e);
            }
        }

        return assetData;
    }

    void OnApplicationQuit()
    {
        for (int i = 0; i < requestList.Length; i++)
        {
            if (requestList[i] != null)
            {
                requestList[i].isAppQuit = true;
            }
        }
    }
}
