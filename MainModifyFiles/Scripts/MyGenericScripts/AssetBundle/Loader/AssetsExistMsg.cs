// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssetsExistMsg.cs
// Author   : wenlin
// Created  : 2013/12/28 
// Purpose  : 此类主要是保存游戏中缺失资源的信息，如果资源是缺陷， 会在此类里面保存
// **********************************************************************
using System;
using System.Collections.Generic;


public class AssetsExistMsg
{
	public AssetsExistMsg ()
	{
	}
	
	private ResourcesMissingMessage  _missingMessage = null;
	public ResourcesMissingMessage missingMessage
	{
		get
		{
			if( _missingMessage == null )
			{
				_missingMessage = new ResourcesMissingMessage(1);
			}
			return _missingMessage;
		}
	}
	
	public int resTotalNumber
	{
		get
		{
			return missingMessage.totalGameResNumber;
		}
		set
		{
			isInit = true;
			missingMessage.totalGameResNumber = value;
		}
	}
	
	private bool isInit = false;
	private const string remoteResVerDataFile = "Set/v.bytes";
	public bool InitMissingMessage( string downloadPath )
	{
		if( isInit )
		{
			return true ;
		}
		
		byte[] buff = DataHelper.GetFileBytes( downloadPath + remoteResVerDataFile );
		if( buff != null )
		{
			Dictionary<string, object> obj = DataHelper.GetJsonFile( buff, true );
			
			JsonResourcesMissingMessageParser parser = new JsonResourcesMissingMessageParser();		
			_missingMessage = parser.DeserializeJson_ResourcesMissingMessage( obj );
			
			if( _missingMessage != null )
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void SaveMissingMessage( string downloadPath )
	{
		if( _missingMessage == null )
		{
			return;
		}
		
		JsonResourcesMissingMessageParser parser = new JsonResourcesMissingMessageParser();
		Dictionary<string, object> obj = parser.SerializeJson_ResourcesMissingMessage( _missingMessage );
		DataHelper.SaveJsonFile( obj, downloadPath + remoteResVerDataFile, true ); 
		
	}
	
	/// <summary>
	/// 清空资源
	/// </summary>
	public void Clear()
	{
		missingMessage.totalGameResNumber = 0;
		missingMessage.missingAssets.Clear();
	}
	
	public void SetMissingResource( string key, AssetPrefab assetPrefab )
	{
		isInit = true;
		
		if( !missingMessage.missingAssets.ContainsKey( key ))
		{
			missingMessage.missingAssets.Add( key, assetPrefab );
		}
		else
		{
			GameDebuger.Log( string.Format( "ResourcesMissingMessage < The key is Exist : {0} >", key ));
		}
			
	}
	
	/// <summary>
	/// 判断游戏的资源是否缺失
	/// </summary>
	public bool IsAssetsExist( string key )
	{
		return missingMessage.missingAssets.ContainsKey( key );
	}
}




