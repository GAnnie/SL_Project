using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ErrorManager : MonoBehaviour 
{
    private static List<ErrorMessage> _errorMessageList = null;

	// Use this for initialization
	void Start () 
    {
	    DontDestroyOnLoad( this.gameObject );

        if( GameSetting.IsCollectDebugInformation()){
			Application.RegisterLogCallback(HandleLog);
		}else{
			#if UNITY_DEBUG_LOG
				Application.RegisterLogCallback(HandleLog);
			#endif
		}
	}


    /// <summary>
    /// Debug的回调函数， 用于记录运行时的信息
    /// </summary>
    /// <param name="logString"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
		//如果调试log模式,则把log收集到内存中
		#if UNITY_DEBUG_LOG
			MachineManager.Instance.AddDebugLog(logString);
		#endif

        //if (type == LogType.Log)
        //{
        //    MachineManager.Instance.DebugLogCollect(logString, stackTrace);
        //}
        if (type == LogType.Error || type == LogType.Exception)
        {
            //MachineManager.Instance.DebugExceptionCollect(logString, stackTrace);
        }
    }
	
    /// <summary>
    /// 主动捕获exception信息
    /// </summary>
    /// <param name="e"></param>
    public static void Debug_Exception( Exception e )
    {
        //MachineManager.Instance.DebugExceptionCollect( e.Message, e.StackTrace );
    }

	// Update is called once per frame
	void Update () 
    {
        
        if (_errorMessageList == null) return;
        if (_errorMessageList.Count > 0)
        {
            foreach( ErrorMessage error in _errorMessageList )
            {
                if (DealWithErrorMessage(error))
                {
                    break;
                }
            }

            lock (_errorMessageList)
            {
                _errorMessageList.Clear();
            }
        }
	}
    
    /// <summary>
    /// Show Error Message 
    /// </summary>
    /// <param name="errorType"> Error type </param>
    /// <param name="errorMessage"></param>
    public static void LogError(ErrorType errorType, string errorMessage, ErrorMessage.ErrorCallBackFunction callBackFunction = null)
    {
        if (_errorMessageList == null)
        {
            _errorMessageList = new List<ErrorMessage>();
        }

        ErrorMessage error = new ErrorMessage(errorType, errorMessage, callBackFunction);
        _errorMessageList.Add(error);
    }

    /// <summary>
    /// Deal With Error Messsage
    /// </summary>
    /// <param name="error"> </param>
    /// <returns> if want to stop application ,  then return true </returns>
    private bool DealWithErrorMessage(ErrorMessage error)
    {
        if ( error == null ) return false;
        ErrorType type = error.errorType;

        if (ErrorDealWithManager.errorHandlerDic.ContainsKey(type))
        {
            ErrorDealWithManager.errorHandlerDic[type](error);
        }

        GameDebuger.Log(error.errorMessage);
        error.Clear();
        return false;
    }
}


public enum ErrorType
{
   AssetsVerDataLoadError= 1,   // 资源版本文件加载错误

   AssetsDownLoadError   = 2,   // 资源文件加载错误

   AssetsFullDiskError   = 3,   // 手机磁盘不足错误

   AssetsDecompressError = 4,   // 解压资源错误

   AssetIOError          = 5,   // IO读写错误

   SeverListError        = 6,   // 获取服务器列表错误

   ApplicationDownLoadError = 7 // 程序加载错误 
   
};


public class ErrorMessage
{
    //回调函数
    public delegate void ErrorCallBackFunction();

    public ErrorType errorType;
    public string    errorMessage;
    public ErrorCallBackFunction errorCallBackFunction = null;   
    public ErrorMessage(ErrorType errorType, string errorMessage, ErrorCallBackFunction callBackFunction = null)
    {
        this.errorType    = errorType;
        this.errorMessage = errorMessage;
        this.errorCallBackFunction = callBackFunction;
    }

    public void Clear()
    {
        errorCallBackFunction = null;
    }
}

public class ErrorDealWithManager
{
    public delegate bool ErrorHander(ErrorMessage errorMsg);
    public static readonly Dictionary<ErrorType, ErrorHander> errorHandlerDic = null;

    static ErrorDealWithManager()
    {
        errorHandlerDic = new Dictionary<ErrorType, ErrorHander>();
        errorHandlerDic.Add(ErrorType.AssetsVerDataLoadError, AssetsDataLoadErrorHandler);
        errorHandlerDic.Add(ErrorType.AssetsDecompressError,  AssetsDecompressErrorHandler);
        errorHandlerDic.Add(ErrorType.AssetsDownLoadError,    AssetsDownLoadErrorHandler);
        errorHandlerDic.Add(ErrorType.AssetsFullDiskError,    AssetsFullDiskErrorHandler);
        errorHandlerDic.Add(ErrorType.AssetIOError,           AssetIOErrorHandler);
        errorHandlerDic.Add(ErrorType.SeverListError,         ServerListErrorHandler);
        errorHandlerDic.Add(ErrorType.ApplicationDownLoadError, ApplicationDownLoadErrorHander);
    }

    private static bool AssetsDataLoadErrorHandler(ErrorMessage errorMsg)
    {
        ShowErrorTipsAndQuit("文件加载错误, 请检查网络状态", errorMsg.errorCallBackFunction);
        return true;
    }

    private static bool AssetsDecompressErrorHandler(ErrorMessage errorMsg)
    {
        ShowErrorTipsAndQuit("文件加载错误, 请重装来修复此错误", errorMsg.errorCallBackFunction);
        return true;
    }
    private static bool AssetsDownLoadErrorHandler(ErrorMessage errorMsg)
    {
        ShowErrorTipsAndQuit("文件加载错误, 请检查网络状态", errorMsg.errorCallBackFunction);
        return true;
    }
    private static bool AssetsFullDiskErrorHandler(ErrorMessage errorMsg)
    {
        ShowErrorTipsAndQuit("游戏可使用空间不足，请检查手机存储空间", errorMsg.errorCallBackFunction);
        return true;
    }

    private static bool AssetIOErrorHandler(ErrorMessage errorMsg)
    {
        ShowErrorTipsAndQuit("文件读写错误。。。", errorMsg.errorCallBackFunction);
        return true;
    }

    private static bool ServerListErrorHandler(ErrorMessage errorMsg)
    {
        ShowErrorTipsAndQuit("服务器列表获取失败， 将重新链接获取", errorMsg.errorCallBackFunction);
        return true;
    }

    private static bool ApplicationDownLoadErrorHander(ErrorMessage errorMsg)
    {
        ShowErrorTipsAndQuit("应用程序包加载错误", errorMsg.errorCallBackFunction);
        return true;
    }

    private static void ShowErrorTipsAndQuit(string errorMessage, ErrorMessage.ErrorCallBackFunction callBackFunction = null)
    {
        WindowManager.openRoundWindowUserDefault( errorMessage,
            delegate()
            {
                if (callBackFunction != null)
                {
                    callBackFunction();
                }
                else
                {
                    ExitGameScript.Instance.HanderExitGame();
                }
            },"确定");
    }
}

