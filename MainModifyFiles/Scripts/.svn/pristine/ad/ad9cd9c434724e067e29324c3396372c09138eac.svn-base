// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  HttpController.cs
// Author   : wenlin
// Created  : 2013/6/8 16:11:18
// Purpose  : 
// **********************************************************************

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

/// <summary>
/// HttpController 尽量在子线程进行调用
/// </summary>

public class HttpController
{
    private static HttpController _instance = null;
    public static HttpController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new HttpController();
            }
            return _instance;
        }
    }

    private HttpController() {}

    //HTTP 获取方式
    private static string HTTP_GET_METHOD  = "Get";
    private static string HTTP_POST_METHOD = "Post"; 


    //Http 控制
    private SimpleWWW _httpController = null;
    public void Setup()
    {
        GameObject go = new GameObject();
        GameObject.DontDestroyOnLoad(go);
		go.name = "SimpleWWW";

        _httpController = go.AddComponent<SimpleWWW>();
    }

    /// <summary>
    /// HTTP 下载资源（ 非主线程版本， 可以在任意线程调用 ）
    /// </summary>
    /// <param name="url"> URL地址 </param>
    /// <param name="httpCallBack"> 下载后的回调 </param>
    /// <param name="isNeedUncompress"> 是否需要解压操作</param>
    public bool DownLoadAnyThread(string uri, 
								   System.Action<byte[]> downLoadFinishCallBack, 
								   System.Action<float> progressCallBack = null, 
								   System.Action<Exception> errorCallBack = null, 
								   bool isNeedUncompress = false,
								   SimpleWWW.ConnectionType type = SimpleWWW.ConnectionType.Continued_Connect
								  )
    {
        if (_httpController == null)
        {
            GameDebuger.Log("Http Controller is NULL ");
            return false;
        }

        Request request = new Request(Request.HTTP_GET_METHOD, uri);
        if (request != null)
        {
            try
            {
                _httpController.ReceiveAnyThread(request, downLoadFinishCallBack, progressCallBack, errorCallBack, isNeedUncompress,type);
            }
            catch (Exception e)
            {
                //                Debug.LogException(e);
                //ErrorManager.Debug_Exception(e);
                throw e;
            }
        }
        return true;
    }

    /// <summary>
    /// HTTP 下载资源 ( 要求主线程调用) 
    /// </summary>
    /// <param name="url"> URL地址 </param>
    /// <param name="httpCallBack"> 下载后的回调 </param>
    /// <param name="isNeedUncompress"> 是否需要解压操作</param>
    public bool DownLoad(string uri, 
						  System.Action<byte[]> downLoadFinishCallBack, 
						  System.Action<float> progressCallBack = null, 
						  System.Action<Exception> errorCallBack = null, 
						  bool isNeedUncompress = false,
						  SimpleWWW.ConnectionType type = SimpleWWW.ConnectionType.Continued_Connect
						)
    {
        if (_httpController == null)
        {
            GameDebuger.Log("Http Controller is NULL ");
            return false;
        }

        Request request = new Request(Request.HTTP_GET_METHOD, uri);
        if ( request != null )
        {
            try
            {
                _httpController.Receive(request, downLoadFinishCallBack, progressCallBack, errorCallBack, isNeedUncompress, type);
            }
            catch (Exception e)
            {
//                Debug.LogException(e);
                //ErrorManager.Debug_Exception(e);
                throw e;
            }
        }
        return true;
    }


    public bool DownLoad(string uri, 
						  System.Action<byte[], object > downLoadFinishCallBack ,
						  object obj ,  
						  System.Action< float > progressCallBack = null, 
						  System.Action<Exception> errorCallBack = null,
						  bool isNeedUncompress = false,
						  SimpleWWW.ConnectionType type = SimpleWWW.ConnectionType.Continued_Connect
						)
    {
        if (_httpController == null)
        {
            GameDebuger.Log("Http Controller is NULL ");
            return false;
        }

        Request request = new Request(Request.HTTP_GET_METHOD, uri);
        if (request != null)
        {
            try
            {
                _httpController.Receive(request, downLoadFinishCallBack, obj, progressCallBack, errorCallBack, isNeedUncompress,type);
            }
            catch (Exception e)
            {
                //ErrorManager.Debug_Exception(e);
//                Debug.LogException(e);
                throw e;
            }
        }
        return true;
    }

    /// <summary>
    /// 分片加载
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="downLoadFinishCallBack"></param>
    /// <param name="obj"></param>
    /// <param name="progressCallBack"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="isNeedUncompress"></param>
    /// <returns></returns>
    public bool DownLoadFragments(string uri, 
								    System.Action downLoadFinishCallBack,  
									System.Action< byte[], bool > dataReciveCallBack , 
									int reciveLen, System.Action<float> progressCallBack = null, 
									System.Action<Exception> errorCallBack = null,
									SimpleWWW.ConnectionType type = SimpleWWW.ConnectionType.Continued_Connect
									)
    {
        if (_httpController == null)
        {
            GameDebuger.Log("Http Controller is NULL ");
            return false;
        }

        Request request = new Request(Request.HTTP_GET_METHOD, uri);
        if (request != null)
        {
            try
            {
                _httpController.ReceiveFragments(request, downLoadFinishCallBack, reciveLen, dataReciveCallBack, progressCallBack, errorCallBack, type);
            }
            catch (Exception e)
            {
                //ErrorManager.Debug_Exception(e);
                //                Debug.LogException(e);
                throw e;
            }
        }
        return true;
    }



    /// <summary>
    /// HTTP 上传操作
    /// </summary>
    /// <param name="uri">URL地址</param>
    /// <param name="uploadFinishCallBack">上传结束后的回调</param>
    /// <returns></returns>
    public bool UpLoad( string url, byte[] dataString, System.Action uploadFinishCallBack = null, System.Action<Exception> errorCallBack = null )
    {
		if ( _httpController == null )
		{
			GameDebuger.Log( "Http Controller is null ");
			return false;
		}
		
		
		Request request = new Request( Request.HTTP_POST_METHOD, url );
		if ( request != null )
		{
			try
			{
                _httpController.Send(request, dataString, uploadFinishCallBack, errorCallBack);
			}
			catch( Exception e)
			{
				Debug.LogException(e);
                ErrorManager.Debug_Exception(e);
                throw e;
			}
		}
        return true;
    }
}
