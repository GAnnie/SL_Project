using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.commons.message;

public class ActionCallbackManager
{
    private static Dictionary<int, ActionCallback> callbacks;
	private static MonoTimer timer;

	public static void init()
	{
		if (SocketManager.IsToolMode == false){
			if (timer == null){
				timer = TimerManager.GetTimer("RequestTimeoutChecker");
				timer.Setup2Time(1, clearExpiredCallback);
				timer.Play();							
			}
		}
        callbacks = new Dictionary<int, ActionCallback>();
	}
	
	/**
	 * 删除过期的ActionCallback
	 */
	private static void clearExpiredCallback()
	{
		List<int> deleteKey = null;
		//检查过期，过期的调用ActionCallback的onTimeout
		foreach(KeyValuePair<int, ActionCallback> keyValue in callbacks)
		{
			ExpiredActionCallback callback = keyValue.Value as ExpiredActionCallback;
			if(callback.isExpiredTime() || SocketManager.IsOnLink == false){
				callback.onTimeout();
				if (deleteKey == null){
					deleteKey = new List<int>();
				}
				deleteKey.Add(keyValue.Key);
				break;
			}	
		}		
		
		//删除过期的ActionCallback
		if (deleteKey != null)
		{
			foreach(int serial in deleteKey)
			{
				remove(serial);
			}			
		}
	}
	
	public static ActionCallback remove(int serial){
        if (callbacks == null || callbacks.ContainsKey(serial) == false)
        {
            return null;
        }

		ActionCallback callback = (ActionCallback)callbacks[serial];
		callbacks.Remove(serial);
		return callback;
	}
	
	public static void put( int serial, ActionCallback callback )
	{
		if(callback == null)
			return;
		long nowTime = DateTime.UtcNow.Ticks/10000;
		callbacks[serial] = new ExpiredActionCallback(callback, nowTime );
	}
	
	public static void removeActionCallback(int serial)
	{
		callbacks.Remove(serial);
	}
	
	public static ActionCallback getActionCallback(int serial)
	{
        if (callbacks.ContainsKey(serial) == false)
        {
            return null;
        }

		ExpiredActionCallback callback = (ExpiredActionCallback)callbacks[serial];
		return callback.callback;
	}
	
	public static int getNextActionCallbackSerial(){
		if (callbacks.Count > 0)
		{
			int[] keys = new int[callbacks.Count];
			callbacks.Keys.CopyTo(keys, 0);
			return keys[0];
		}
		else
		{
			return -1;	
		}
	}
	
	public static void destroy()
	{
        if (timer != null)
        {
			TimerManager.RemoveTimer(timer);
            timer.Stop();
        }
		timer = null;
		callbacks = null;
	}
}


class ExpiredActionCallback : ActionCallback
{
	public static readonly int ExpiredTime = 5*60*1000;
	public ActionCallback callback;
	
	private long expiredTimeInMs;
	
	public ExpiredActionCallback( ActionCallback callback, long expiredTimeInMs)
	{
		this.callback=callback;
		this.expiredTimeInMs=expiredTimeInMs;
	}
	
	public bool isExpiredTime()
	{
		//long nowTime = DateTime.UtcNow.Ticks/10000;
		//return nowTime - expiredTimeInMs > ExpiredTime;
		
		//取消请求超时处理
		return false;
	}
	
	public void onSuccess( GeneralResponse generalResponse )
	{
		callback.onSuccess(generalResponse);
	}
	
	public void onError( ErrorResponse errorResponse )
	{
		callback.onError(errorResponse);
	}
	
	public void onTimeout()
	{
		callback.onTimeout();
	}
}
