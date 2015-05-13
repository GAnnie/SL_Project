// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  CollectMissingAssetsManager.cs
// Author   : wenlin
// Created  : 2014/1/2 
// Purpose  : 收集丢失资源的过滤器,  判断当前资源是否正确的资源
// **********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;

public class CollectMissingAssetsFilter
{
	private AssetsConfig 	_assetConfig   = null;
	private AssetsExistMsg _assetExistMsg = null;
	public CollectMissingAssetsFilter ( AssetsConfig assetConfig, AssetsExistMsg assetMsg )
	{
		this._assetConfig   = assetConfig;
		
		this._assetExistMsg = assetMsg; 
	}
	
	/// <summary>
	/// 判断资源是否正确， 如果不正确则直接过滤， 返回的是RefIds
	/// </summary>
	public string Filter( string assetName )
	{
		if( this._assetConfig == null || this._assetExistMsg == null )
		{
			return null;
		}
		
		string refId = _CheckAsset( assetName );
		if( string.IsNullOrEmpty( refId ))
		{
			return null;
		}
		
		return refId;
	}
	
	
	/// <summary>
	/// 主动收集当前丢失的资源
	/// </summary>
//	public bool Collect( string assetName )
//	{
//		string refId = _CheckAsset( assetName );
//		
//		if( string.IsNullOrEmpty( refId ))
//		{
//			return false;
//		}	
//
//		//加入下载列表
//		_downloadQueue.Enqueue( refId );
//		
//		return true;
//	}
//	
	
	/// <summary>
	/// 检查RefId的有效性
	/// </summary>	
	public bool CheckRefIdValid( string refId )
	{
		if( !_CheckAssetRefId( refId ))
		{
			return false;
		}
		
		string assetName = AssetPathReferenceManager.Instance.GetAssetPath( refId );
		if( string.IsNullOrEmpty( assetName ))
		{
			return false;
		}
		
		if( _assetConfig.IsContainAsset( assetName, false ))
		{
			return false;
		}
		
		return true;
	}
	
	/// <summary>
	/// 返回队列中的资源
	/// </summary>
//	public List< string > GetMissingAssets( int getNumber = 1 )
//	{
//		List< string > returnList = new List<string>();
//		
//		if( _downloadQueue.Count == 0 )
//		{
//			return returnList;
//		}
//		
//		while( returnList.Count == getNumber )
//		{
//			if( _downloadQueue.Count == 0 )
//			{
//				break;
//			}
//			
//			string refId = _downloadQueue.Dequeue();
//			if( AssetbundleManager.Instance.IsAssetMissing( refId ) )
//			{
//				returnList.Add( refId );
//			}
//		}
//		
//		return returnList;
//	}
	
	/// <summary>
	/// 检查文件目录的正确性
	/// </summary>
	/// <returns>
	/// 返回文件的引用ID
	/// </returns>
	
	private string _CheckAsset( string assetName )
	{
		if( string.IsNullOrEmpty( assetName ))
		{
			return null;
		}
		
		//如果当前资源已经存在
		if(_assetConfig.IsContainAsset( assetName , false ))
		{
			return null;
		}
		
		//如果不存在当前路径的引用ID， 则返回
		string refId = AssetPathReferenceManager.Instance.GetAssetPathRefId( assetName );
		if( !_CheckAssetRefId( refId ))
		{
			return null;
		}
		
		return refId;
	}
	
	/// <summary>
	/// 检查RefID正确性
	/// </summary>
	private bool _CheckAssetRefId( string refId )
	{
		if( string.IsNullOrEmpty( refId ))
		{
			return false;
		}
		
		//如果当前资源不是丢失资源， 则返回
		if( !_assetExistMsg.IsAssetsExist( refId ))
		{
			return false;
		}
		
		return true;
	}
}

