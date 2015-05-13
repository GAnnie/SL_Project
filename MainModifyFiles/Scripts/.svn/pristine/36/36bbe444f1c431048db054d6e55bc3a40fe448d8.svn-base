// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  UmengAnalyticsHelper.cs
// Author   : willson
// Created  : 2013/8/12 13:04:36
// Purpose  : 
// **********************************************************************
using System;
using System.Collections.Generic;

class UmengAnalyticsHelper
{
	public static void onInit()
	{
#if (UNITY_EDITOR || UNITY_STANDALONE)
		return;
#elif UNITY_ANDROID
		//MtaAgent.initMTAConfig(false);
		//MtaAgent.startStatService(BaoyugameSdk.getMetaData("TA_APPKEY"));
#else
		return;
#endif
	}

    public static void onResume()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        //MobclickAgent.onResume();
		//MtaAgent.onResume();
#else
        return;
#endif
    }

    public static void onPause()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        //MobclickAgent.onPause();
		//MtaAgent.onPause();
#else
        return;
#endif
    }

	public static void trackPay(string orderId, int rmb)
	{
#if (UNITY_EDITOR || UNITY_STANDALONE)
		return;
#elif UNITY_ANDROID

		#if UNITY_BAOYUGAME
			//BaoyuGameSdk.trackPay(orderId, rmb);
		#endif	
		//MtaAgent.trackPay(orderId, rmb);

#else
		return;
#endif
	}

    public static void onEvent(String event_id)
    {
		GameDebuger.Log("UmengAnalyticsHelper Event="+event_id);
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        //MobclickAgent.onEvent(event_id);
		//MtaAgent.trackCustomEvent(event_id);
#else
        return;
#endif
    }

    public static void onEvent(String event_id, String label)
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        //MobclickAgent.onEvent(event_id,label);
		//MtaAgent.trackCustomEvent(event_id, label);
#else
        return;
#endif
    }

    public static void onEvent(string event_id, Dictionary<string, string> dic)
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        //MobclickAgent.onEvent(event_id,dic);
#else
        return;
#endif
    }

    public static void onEvent(String event_id, String sub_event_id, String label)
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
		//MtaAgent.trackCustomEvent(event_id, sub_event_id, label);

        //Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add(sub_event_id, label);
        //MobclickAgent.onEvent(event_id,dic);
#else
        return;
#endif
    }
}