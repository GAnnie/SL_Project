//	[Event(name="event_success", type = "loader.net.action.ResponseEvent")]
//	[Event(name="event_error", type = "loader.net.action.ResponseEvent")]
//	[Event(name="event_timeout", type = "loader.net.action.ResponseEvent")]
//	[Event(name="event_giveup", type = "loader.net.action.ResponseEvent")]
//	[Event(name="event_finish", type = "loader.net.action.ResponseEvent")]
using UnityEngine;
using System.Collections.Generic;

/**
 * 
 * @author senkay
 */
using com.nucleus.commons.message;
using com.nucleus.commons.dispatcher;
//using LuaInterface;


public class ServiceRequestAction : ActionCallback
{
	public delegate void CallbackDelegate( GeneralResponse e);
	public delegate void CallbackDelegateVoid();
	public delegate void CallbackOnError( ErrorResponse errorResponse );
	public delegate void CallbackDelegateWithBytes(ProtoByteArray bytes);	
	
	CallbackDelegate OnSuccessCallback;
	CallbackOnError OnErrorCallback;
	CallbackDelegateVoid OnTimeOutCallback;
	CallbackDelegateVoid OnGiveUpCallback;
	CallbackDelegateVoid OnFinishCallback;
	CallbackDelegateWithBytes OnSuccessCallbackWithBytes;
	
	
	struct ServieceRequeset
	{
		public ServiceRequestAction action ;
		public GeneralResponse response ;
	}
	
	//private static List< ServieceRequeset > _successList = new List<ServieceRequeset>();
	//private static List< ServieceRequeset > _errorList   = new List<ServieceRequeset>();
	private static List< ServieceRequeset > _timeOutList = new List<ServieceRequeset>();
	
	//--------------------------------------------------------------
	private GeneralRequest _request;
	private byte _requestType;
	
	
	private string _tip;
	
	public ServiceRequestAction( GeneralRequest request , byte requestType)
	{
		_request = request;
		_requestType = requestType;
	}
	
	public void setLoadingTip(string tip)
	{
		this._tip = tip;
			
		if (!string.IsNullOrEmpty(tip)){
			RequestLoadingTip.Show(tip);	
		}else{
			//RequestLoadingTip.Stop();
		}
	}
	
	public void send()
	{
		if (IsSkipAction(_request) == false)
		{
			GameDebuger.Log("ServiceRequestAction _ send " + GetRequestDebugInfo(_request));
		}
		
		if ( OnSuccessCallback != null || OnErrorCallback != null || 
			 OnFinishCallback != null || OnGiveUpCallback != null ||
			OnTimeOutCallback != null )
		{
			SocketManager.Instance.SendRequest(_request, _requestType, this);
		}
		else
		{
			SocketManager.Instance.SendRequest(_request, _requestType, this);
		}
	}

	private string GetRequestDebugInfo(GeneralRequest request)
	{
		string info = "action="+request.action + " type=" + _requestType;
		foreach(object obj in request.xparams)
		{
			info += " " + obj.ToString();
		}
		return info;
	}

	private bool IsSkipAction(GeneralRequest request)
	{
		if (request.action == "/scene/verifyWalk" || request.action == "/scene/planWalk")
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void giveup()
	{
		StopLoadingTip();
		
		if ( OnGiveUpCallback != null )
			OnGiveUpCallback();
		finishAction();
	}
	
	public void onSuccess(GeneralResponse generalResponse)
	{
		StopLoadingTip();
		
		if ( OnSuccessCallback != null )
			OnSuccessCallback( generalResponse );
		finishAction();
	}
	
	public void onError(ErrorResponse errorResponse )
	{
		StopLoadingTip();
		
		if ( OnErrorCallback != null ){
			OnErrorCallback( errorResponse );
		}else{
			showMessageBox(errorResponse.message);
		}
		//Debug.LogError("onError: action="+_request.action+" error="+errorResponse.message);
		Debug.Log(string.Format("<color=yellow>ErrorResponse: action={0} error={1}</color>",_request.action,errorResponse.message));
		//GameDebuger.Log("onError: action="+_request.action+" error="+errorResponse.message);
		finishAction();
	}

	public void onTimeout()
	{
		StopLoadingTip();
		
		GameDebuger.Log("onTimeout: action="+_request.action+" error="+"请求超时");		
		
		if (OnTimeOutCallback != null){
			OnTimeOutCallback();
		}else{
			ErrorResponse errorResponse = new ErrorResponse(); 
			errorResponse.message = "请求超时";
			
			if ( OnErrorCallback != null ){
				OnErrorCallback( errorResponse );
			}
		}
		
		finishAction();
	}
	
	private void finishAction()
	{
		if ( OnFinishCallback != null )
			OnFinishCallback();
	}
	
	private void StopLoadingTip(){
		if (!string.IsNullOrEmpty(_tip)){
			RequestLoadingTip.Stop();
		}		
	}
	
	public void onSuccessWithBytes(ProtoByteArray bytes)
	{
		if ( OnSuccessCallbackWithBytes != null )
			OnSuccessCallbackWithBytes( bytes );
	}
	
	public bool HasCallbackWithBytes(){
		return OnSuccessCallbackWithBytes != null;
	}
	
	public static void requestServer( GeneralRequest request,byte requestType, string tip = "" , CallbackDelegate onSuccess = null,
									  CallbackOnError onError = null, CallbackDelegateVoid onTimeOut = null,
									  CallbackDelegateVoid onGiveUp = null, CallbackDelegateVoid onFinish = null,
									  CallbackDelegateWithBytes onSuccessCallbackWithBytes = null)
	{
		ServiceRequestAction action = new ServiceRequestAction(request, requestType);
		
		if ( onSuccess != null )
			action.OnSuccessCallback = onSuccess;
		
		if ( onError != null )
			action.OnErrorCallback = onError;
		
		if ( onTimeOut != null )
			action.OnTimeOutCallback = onTimeOut;
		
		if ( onGiveUp != null )
			action.OnGiveUpCallback = onGiveUp;
		
		if ( onFinish != null )
			action.OnFinishCallback = onFinish;
		
		if (onSuccessCallbackWithBytes != null)
			action.OnSuccessCallbackWithBytes = onSuccessCallbackWithBytes;
		
		action.setLoadingTip(tip);
		
		if (SocketManager.IsOnLink){
			action.send();
		}else{
			action.onTimeout();
		}
	}

	public static void requestServer(GeneralRequestInfo request, string tip = "" , CallbackDelegate onSuccess = null,
	                                 CallbackOnError onError = null, CallbackDelegateVoid onTimeOut = null,
	                                 CallbackDelegateVoid onGiveUp = null, CallbackDelegateVoid onFinish = null,
	                                 CallbackDelegateWithBytes onSuccessCallbackWithBytes = null)
	{
		requestServer(request.request, request.requestType, tip, onSuccess, onError, onTimeOut, onGiveUp, onFinish, onSuccessCallbackWithBytes);
	}

	private void showMessageBox(string msg)
	{
		TipManager.AddTip(msg);
	}

	//	Lua -----------------------------------
//	public static void requestServerByLua(string action, byte requestType, string tip = "", LuaFunction funcSuccess = null,
//	                                      LuaFunction funcError = null, LuaFunction funcTimeOut = null,
//	                                      LuaFunction funcGiveUp = null, LuaFunction funcFinish = null) {
//		UluaDebugManager.DebugLogWarning("requestServerByLua被Lua调用，用于请求服务器数据");
//
//		GeneralRequest request = new GeneralRequest(); 
//		request.action = action;
//		request.xparams = new List<object>();
//		GeneralRequestInfo requestInfo = new GeneralRequestInfo();
//		requestInfo.request = request;
//		requestInfo.requestType = requestType;
//		
//		requestServer(requestInfo, tip, delegate(GeneralResponse e) {
//			if (funcSuccess != null) funcSuccess.Call((e as com.nucleus.h1.logic.core.modules.trade.dto.TradeCenterDto).items);
//		}, delegate(ErrorResponse errorResponse) {
//			if (funcError != null) funcError.Call(errorResponse);
//		}, delegate() {
//			if (funcTimeOut != null) funcTimeOut.Call();
//		}, delegate() {
//			if (funcGiveUp != null) funcGiveUp.Call();
//		}, delegate() {
//			if (funcFinish != null) funcFinish.Call();
//		});
//	}
}

