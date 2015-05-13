using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ResourceDataManager
{
	/// <summary>
	/// 解析ResourceData
	/// </summary>
	public static void AnalyResourceData ( byte[] data ,
											out ResourcesVersionData	resVerSionData , 
											out AssetPathReferenceList	assetRefList
											)
	{
		 byte[]	resVersionDataBytes	= null;
		 byte[]	assetRefListBytes	= null;
		
		//分析ResourceData
		AnalyResourceData( 	data, 
							out resVersionDataBytes,
							out assetRefListBytes
						);
		
		
		//解析ResourceVersionData
		if( resVersionDataBytes != null )
		{
			JsonResourcesVersionDataParser parser = new JsonResourcesVersionDataParser();
			Dictionary< string ,object > obj = DataHelper.GetJsonFile( resVersionDataBytes, true );
			resVerSionData = parser.DeserializeJson_ResourcesVersionData( obj );
		}
		else
		{
			resVerSionData = null;
		}
		
		
		//解析AssetPathReferenceList
		if ( assetRefListBytes != null )
		{
			JsonAssetPathReferenceListParser parser = new JsonAssetPathReferenceListParser();
			Dictionary< string ,object > obj = DataHelper.GetJsonFile( assetRefListBytes, true );
			assetRefList = parser.DeserializeJson_AssetPathReferenceList( obj );
		}
		else
		{
			assetRefList = null;
		}

//		
//		//解析mapAreaData
//		if( mapAreaDataBytes != null )
//		{
//			JsonMapAreasDataParser parser = new JsonMapAreasDataParser();
//			Dictionary< string ,object > obj = Utility.GetJsonFile( mapAreaDataBytes, true );
//			mapAreaData = parser.DeserializeJson_MapAreasData( obj );
//		}
//		else
//		{
//			mapAreaData = null;
//		}
		
		
	}
	
	
	/// <summary>
	/// 解析ResourceData
	/// </summary>
	public static void AnalyResourceData( byte[] data,
											out byte[]	resVersionData , 
											out byte[]	assetRefList
										)
	{
		if( data != null )
		{
			int position = 0;
			//读取头文件
			int head = BitConverter.ToInt32( GetByte( data, ref position, 4 ), 0);
			if( head == fileHead )
			{
				uint resVersionDataLen = BitConverter.ToUInt32( GetByte( data, ref position, 4 ), 0);
				uint assetRefListLen   = BitConverter.ToUInt32( GetByte( data, ref position, 4 ), 0);
//				uint mapAreaDataLen    = BitConverter.ToUInt32( GetByte( data, ref position, 4 ), 0);
				
				byte[] tempResVersionData = GetByte( data, ref position, (int)resVersionDataLen );
				byte[] tempAssetRefList   = GetByte( data, ref position, (int)assetRefListLen   );
//				byte[] tempMapAreaData    = GetByte( data, ref position, (int)mapAreaDataLen    );
				
				int end = BitConverter.ToInt32(  GetByte( data, ref position, 4 ), 0 );
				if( end != fileEnd )
				{
					Debug.LogError( "AnalyResourceData < Analy ResourceData Error, wrong file end >" );
				}
				else
				{
					resVersionData = tempResVersionData;
					assetRefList   = tempAssetRefList;
//					mapAreaData	   = tempMapAreaData;
					return;
				}
			}
			else
			{
				Debug.LogError( "AnalyResourceData < Analy ResourceData Error, wrong file head >" );
			}			
		}
		
		resVersionData = null;
		assetRefList = null;
//		mapAreaData = null;
	}
	
	private static byte[] GetByte( byte[] data, ref int pos,  int len )
	{
		if( data == null )
		{
			GameDebuger.Log( "Resource Data is null !!" );
			return null;
		}
		
		if( ( pos + len ) > data.Length )
		{
			Debug.LogError( "ResourceDataManager < Get Data Error , out of array len >" );
		}
		
		byte[] retureByte = new byte[ len ];
		Buffer.BlockCopy( data, pos, retureByte, 0, len );
		
		pos += len;
		
		return retureByte;
	}
	
	
	
	/// <summary>
	/// 封装ResourceData数据
	/// </summary>
	private static readonly int fileHead = 0x56ae45;
	private static readonly int fileEnd	= 0x32;
	public static byte[] AssembleResourceData( 											
											byte[]	resVersionData , 
											byte[]	assetRefList
												)
	{
		//合拼资源
		List<byte> bytesList = new List<byte>();				
		
		//文件头
		bytesList.AddRange( BitConverter.GetBytes( fileHead )  );
		
		//ResourceVersionData长度
		bytesList.AddRange( BitConverter.GetBytes( (uint)resVersionData.Length ) );
		
		//MapAreasData 的长度
		bytesList.AddRange( BitConverter.GetBytes( (uint)assetRefList.Length ) );
		
//		//MapAreasData 的长度
//		bytesList.AddRange( BitConverter.GetBytes( (uint)mapAreaData.Length ));
		
		//ResourceVersionData数据体
		bytesList.AddRange( resVersionData );
			
		//AssetReferenceList数据体
		bytesList.AddRange( assetRefList );
		
//		//MapAreasData数据体
//		bytesList.AddRange( mapAreaData );
		
		//文件尾
		bytesList.AddRange( BitConverter.GetBytes( fileEnd ) );
		
		return bytesList.ToArray();
	}
												
}
