// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BaiduVoiceRecognition.cs
// Author   : willson
// Created  : 2014/11/4 
// Porpuse  : 
// **********************************************************************
using System;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using System.IO;

public class BaiduVoiceRecognition : MonoBehaviour 
{
	private static string serverURL = "http://vop.baidu.com/server_api";
	private static string token;// = "24.85e67dfebb06c06c1cf41626cb21cc71.2592000.1417863541.282335-4534789";

	// put your own params here
	private static string apiKey = "dbAZnpWMVWuzVSmVt1CZms3x";
	private static string secretKey = "BYTOGHEUIvPBkVZ0t0NLMrpPlcWj7iek";

	// 用户id，推荐使用mac地址/手机IMEI等类似参数
	//private static string cuid = PlayerModel.Instance.GetPlayerId();

	private static BaiduVoiceRecognition _instance = null;
	public static BaiduVoiceRecognition Instance
	{
		get
		{
			CreateInstance();
			return _instance;
		}
	}

	static void CreateInstance()
	{
		if (_instance == null)
		{
			_instance = GameObject.FindObjectOfType(typeof(BaiduVoiceRecognition)) as BaiduVoiceRecognition;
			
			if (_instance == null)
			{
				GameObject go = new GameObject("_BaiduVoiceRecognition");
				DontDestroyOnLoad(go);
				_instance = go.AddComponent<BaiduVoiceRecognition>();
			}
		}
	}

	public void getToken()
	{
		string getTokenURL = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials" + 
			"&client_id=" + apiKey + "&client_secret=" + secretKey;

		Debug.Log(getTokenURL);

		StartCoroutine(_Sent(getTokenURL,SetToken));
	}

	private void SetToken(string text)
	{
		BaiduToken baiduToken = null;
		if( !string.IsNullOrEmpty( text ) )
		{
			Dictionary< string ,object > tempDic = MiniJSON.Json.Deserialize( text ) as Dictionary< string , object > ;
			if( tempDic.Count > 0)
			{
				JsonBaiduTokenParser info = new JsonBaiduTokenParser();
				baiduToken = info.DeserializeJson_BaiduToken( tempDic );
			}
		}

		if(baiduToken != null)
		{
			BaiduVoiceRecognition.token = baiduToken.access_token;
			Debug.Log(BaiduVoiceRecognition.token);
		}
		else
		{
			BaiduVoiceRecognition.token = "";
		}
	}

	private System.Action<string> _GetVoiceTextAction;

	/*
	public void GetVoiceText(AudioClip clip,System.Action<string> action)
	{
		GetVoiceText(SavWav.ToWav(clip),action);
	}
	*/

	/**
	 *	BaiduVoiceRecognition.GetVoiceText(SavWav.ToWav(AudioSource.clip),VoiceRecognitionBack);
	 * */
	public void GetVoiceText(byte[] samples,int dataLength,System.Action<string> action)
	{
		string url = serverURL + "?cuid=" + PlayerModel.Instance.GetPlayerId() + "&token=" + token;

		GameDebuger.Log("GetVoiceText "+url);

		_GetVoiceTextAction = action;
		StartCoroutine(_Post(url,samples,dataLength,GetVoiceTextHandle));
	}

	private void GetVoiceTextHandle(string text)
	{
		GameDebuger.Log("GetVoiceTextHandle " + text);

		string beginStr = "\"result\":[\"";
		int begin = text.IndexOf(beginStr);
		if(begin != -1)
		{
			begin = begin + beginStr.Length;
			int end = text.IndexOf("\"]") - begin;
			
			if(begin != -1 && end != -1)
			{
				text = text.Substring(begin,end);
#if UNITY_EDITOR || UNITY_IPHONE
				if(!string.IsNullOrEmpty(text))
				{
					text = BaiduEncoding.NormalU2C(text);
					GameDebuger.Log("BaiduEncoding " + text);
				}
#endif
			}

			if(_GetVoiceTextAction != null)
				_GetVoiceTextAction(text);
		}
	}

	IEnumerator _Post(string url,byte[] data,int dataLength,System.Action<string> action)
	{
		Hashtable postHeader = new Hashtable();

        postHeader.Add("Content-Type", "audio/amr; rate=8000");
//		postHeader.Add("Content-Type", "audio/wav; rate=8000");
		postHeader.Add("Content-Length", dataLength);
		postHeader.Add("Connection", "close");

//		#if UNITY_IPHONE
//			WWW sendWWW = new WWW( url, data, postHeader);
//			GameDebuger.Log("_Post " + url);
//		#else
			byte[] amrData = new byte[dataLength];
			Array.Copy(data, amrData, dataLength);
			WWW sendWWW = new WWW( url, amrData, postHeader);
		GameDebuger.Log("_Post " + url + "dataLength=" + dataLength + "dataLen=" + data.Length);
//		#endif

		yield return sendWWW;

		GameDebuger.Log("Return " + url);

		if( sendWWW.isDone && sendWWW.error == null )
		{
			GameDebuger.Log("isDone " + sendWWW.text);
			if ( action != null ) 
				action(sendWWW.text);
		}
		else
		{
			GameDebuger.Log("Error: " + sendWWW.error);
			if ( action != null ) 
				action("Error: " + sendWWW.error);
		}
	}

	IEnumerator _Sent(string url,System.Action<string> action)
	{
		WWW sendWWW = new WWW(url);
		yield return sendWWW;
		
		if( sendWWW.isDone && sendWWW.error == null )
		{
			if ( action != null ) 
				action(sendWWW.text);
		}
		else
		{
			GameDebuger.Log("Error: " + sendWWW.error);
		}
	}


}