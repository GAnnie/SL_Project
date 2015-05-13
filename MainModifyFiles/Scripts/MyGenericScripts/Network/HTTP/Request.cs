#define DONT_USE_WEB_LIB


using System;

#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)
using System.Web;
#endif
using System.Threading;
using System.Net;
using System.IO;

using UnityEngine;
using System.Collections.Generic;

public class Request
{
    //HTTP 获取方式
    public static string HTTP_GET_METHOD  = "GET";

    public  static string HTTP_POST_METHOD = "POST";


    //当前使用的方法
    private string httpMethod = HTTP_GET_METHOD;

    //URL
    private string url;
    public string requestURL { get { return url; } }
    //是否正在传输
    private bool sent = false;

    //是否完成
    private bool _isDone = true;
    public bool isDone { get { return _isDone; } }

    //加载进度
    private float _progress = 0.0f;
    public float progress { get { return _progress; } }


    //是否需要LZMA解压
    private bool lzma_uncompress = false;

    //异常
    public Exception exception = null;

    //最多重连数量
    private int MAXIMUMREDIRECTS = 10;

    //获取字节数
    private byte[] _bytes = null;

    //程序退出
    public bool isAppQuit = false;
    public byte[] bytes
    {
        get
        {
            if (isDone)
            {
                return _bytes;
            }
            else
            {
                return null;
            }
        }
        set
        {
            _bytes = value;
        }
    }

    public Request(string method, string url)
    {
        this.httpMethod = method;
        this.url = url;
    }

    /// <summary>
    /// 解压数据
    /// </summary>
    public void Uncompress()
    {
        if (sent)
        {
            throw new InvalidOperationException("Request has already completed.");
        }

        sent = true;
        _isDone = false;

        //对象池进行接受
        ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object t)
        {
            try
            {
                MemoryStream uncompressStream = new MemoryStream();
                LZMA_Util.DecompressFileLZMA(_bytes, uncompressStream, false, false);

                _bytes = uncompressStream.ToArray();
                uncompressStream.Close();
            }
            catch (Exception e)
            {
                exception = e;
            }

            _isDone = true;

        }));
    }

    /// <summary>
    /// 接收数据
    /// Unity_IPHONE版本去除web库的使用
    /// </summary>
    public void Receive( bool UnCompress = false )
    {
        if (sent)
        {
            throw new InvalidOperationException("Request has already completed.");
        }
		
#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)	
        sent = true;
        _isDone = false;
        lzma_uncompress = UnCompress;
		
        //对象池进行接受
        ThreadPool.QueueUserWorkItem (new WaitCallback (delegate(object t) 
            {
                int retry = 0;
                while (++retry < MAXIMUMREDIRECTS)
                {
                    long contentLen = 0;
                    Stream sIn = null;
                    HttpWebResponse wr = null;
                    MemoryStream mmStream = null;

                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Timeout   = Timeout.Infinite;

                        wr = (HttpWebResponse)request.GetResponse();
                        sIn = wr.GetResponseStream();
                        mmStream = new MemoryStream();

                        contentLen = wr.ContentLength;
                        int readByteLen = 0;
                        int totalDownByteLen = 0;
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
							readByteLen = sIn.Read(buffer, 0, buffer.Length);
						
							if( contentLen != -1 )
							{
	                            if (totalDownByteLen >= contentLen || isAppQuit)
	                            {
	                                break;
	                            }
							}
							else
							{
								if( (totalDownByteLen >= contentLen && readByteLen == 0) || isAppQuit )
								{
									break;
								}
							}

                            totalDownByteLen += readByteLen;

                            if (contentLen == -1)
                                _progress = 1.0f;
                            else
                                _progress = ((float)totalDownByteLen / contentLen);

                            mmStream.Write(buffer, 0, readByteLen);
                        }

                        if (isAppQuit) return;

                        exception = null;
                        if (lzma_uncompress)
                        {
                            mmStream.Position = 0;
                            MemoryStream uncompressStream = new MemoryStream();
                            LZMA_Util.DecompressFileLZMA(mmStream, uncompressStream, false, false);

                            _bytes = uncompressStream.ToArray();
                            uncompressStream.Close();
                        }
                        else
                        {
                            _bytes = mmStream.ToArray();
                        }

                        break;
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(3000);
                        exception = e;
                    }
                    finally
                    {
                        if (sIn != null) sIn.Close();
                        if (wr != null) wr.Close();
                        if (mmStream != null ) mmStream.Close();
                    }
                }
			
				_progress = 1.0f;
                _isDone = true;

            }));
#else
		Debug.LogError( " The app RuntimePlatfrom is UNITY_IPHONE, it can not use lib of web " );
		_isDone = false;
#endif

		
    }

    /// <summary>
    /// 片段接收处理函数
    /// <param bytes[]>接受到的信息</param>
    /// <param bool >是否接受完毕</param>
    /// </summary>
    private System.Action<byte[], bool> _dataReceiveHander = null;
    private long _receiveLen = 1024; //1KB


    /// <summary>
    /// 分片加载模式， 
    /// </summary>
    public void ReceiveFragments( System.Action<byte[], bool> dataReceiveHander, long lenLimit )
    {
        if (sent)
        {
            _isDone = true;
            throw new InvalidOperationException("Request has already completed.");
        }
		
		
#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)
        sent = true;
        _isDone = false;

        _dataReceiveHander = dataReceiveHander;
        _receiveLen = lenLimit;

        if (_dataReceiveHander == null)
        {
            _isDone = true;
            throw new InvalidOperationException("DataReceiveHander is Null");
        }


        //对象池进行接受
        ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object t)
        {
            int retry = 0;
            while (++retry < MAXIMUMREDIRECTS)
            {
                long contentLen = 0;
                Stream sIn = null;
                HttpWebResponse wr = null;
                MemoryStream mmStream = null;

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = Timeout.Infinite;

                    wr = (HttpWebResponse)request.GetResponse();
                    sIn = wr.GetResponseStream();
                    mmStream = new MemoryStream();

                    contentLen = wr.ContentLength;

                    int readByteLen       = 0;
                    long totalDownByteLen = 0;
                    long framentsLen      = 0;
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        readByteLen = sIn.Read(buffer, 0, buffer.Length);
    
						if( contentLen != -1 )
						{
                            if (totalDownByteLen >= contentLen || isAppQuit)
                            {
                                break;
                            }
						}
						else
						{
							if( (totalDownByteLen >= contentLen && readByteLen == 0) || isAppQuit )
							{
								break;
							}
						}						

                        totalDownByteLen += readByteLen;
                        if (contentLen == -1)
                            _progress = 1.0f;
                        else
                            _progress = ((float)totalDownByteLen / contentLen);

                        framentsLen += readByteLen;
                        mmStream.Write(buffer, 0, readByteLen);

                        if (framentsLen >= _receiveLen)
                        {
                            _dataReceiveHander(mmStream.ToArray(), false);
                            mmStream.Close();
                            mmStream = new MemoryStream();
                            framentsLen = 0;
                        }
                    }

                    if (isAppQuit) return;

                    _dataReceiveHander(mmStream.ToArray(), true);
                    exception = null;

                    break;
                }
                catch (Exception e)
                {
                    Thread.Sleep(3000);
                    exception = e;
                }
                finally
                {
                    if (sIn != null) sIn.Close();
                    if (wr != null) wr.Close();
                    if (mmStream != null) mmStream.Close();
                }
            }
			
			_progress = 1.0f;
            _isDone = true;

        }));
		
#else
	Debug.LogError( " The app RuntimePlatfrom is UNITY_IPHONE, it can not use lib of web " );
	_isDone = false;	
#endif
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void Send( byte[] data )
    {
        if (sent)
        {
            throw new InvalidOperationException("Request has already completed.");
        }
		
#if (!UNITY_IPHONE && !DONT_USE_WEB_LIB)
        sent = true;
        _isDone = false;
        _bytes = data;

        //对象池进行接受
        ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object t)
        {
            int retry = 0;
            while (++retry < MAXIMUMREDIRECTS)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = HTTP_POST_METHOD;
//                    request.Accept = null;
                    request.ContentType = "application/octet-stream";
                    request.ContentLength = _bytes.Length;

                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(_bytes, 0, _bytes.Length);
                    } 
					
					exception = null;
                    break;
                }
                catch (Exception e)
                {
                    Thread.Sleep(3000);
                    exception = e;
                }
            }

            _isDone = true;
        }));
#else
	Debug.LogError( " The app RuntimePlatfrom is UNITY_IPHONE, it can not use lib of web " );
	_isDone = false;	
#endif
		
    }
}
