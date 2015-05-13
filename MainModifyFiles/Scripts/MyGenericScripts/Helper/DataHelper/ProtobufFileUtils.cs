// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AMF3Utils.cs
// Author   : wenlin
// Created  : 2013/2/18 
// Purpose  : 
// **********************************************************************
using System;
using UnityEngine;

using System.IO;
using com.nucleus.h1.logic.core.modules.proto;

public class ProtobufFileUtils
{
	public static bool SaveFile( object obj, string filePath )
	{
		if ( obj == null ) return false;

        new ProtobufFileThread().SaveFile(obj, filePath);

        return true;
 	}
	
	public static bool SaveFile( byte[] bytes, string filePath )
	{
		if ( bytes == null ) return false;

        new ProtobufFileThread().SaveFile(bytes, filePath);

        return true;
 	}	
	
	public static object LoadObjFile( string filePath )
	{
		if( !ResourceLoader.IsAssetExist( filePath ) )
		{
			GameDebuger.Log( "#Error : File - " + filePath + " is not exit " );
			return null;
		}
		
		TextAsset ta 		= ResourceLoader.Load(filePath) as TextAsset;
        byte[] unCompressBuff = ta.bytes;

        return ProtobufUtilsNet.parseTypedMessageFrom(unCompressBuff);
	}
	
	public static object LoadObjFromFile( string filePath )
	{
		FileStream file;
		try
		{
			if ( File.Exists( filePath ) )
			{
			 	file = new FileStream( filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			}else{
				return null;
			}
		}
		catch( FileLoadException ex )
		{
			GameDebuger.Log("#Error : Open file which path of : " + filePath);		
			return null;
		}		
		
		BinaryReader binaryReader = new BinaryReader(file);
		
		byte [] unCompressBuff = binaryReader.ReadBytes((int)file.Length);

		file.Close();

        return ProtobufUtilsNet.parseTypedMessageFrom(unCompressBuff);
	}	
	
	public static byte[] LoadBytesFromFile( string filePath )
	{
		FileStream file;
		try
		{
			if ( File.Exists( filePath ) )
			{
			 	file = new FileStream( filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			}else{
				return null;
			}
		}
		catch( FileLoadException ex )
		{
			GameDebuger.Log("#Error : Open file which path of : " + filePath);		
			return null;
		}		
		
		BinaryReader binaryReader = new BinaryReader(file);
		
		byte [] unCompressBuff = binaryReader.ReadBytes((int)file.Length);

		file.Close();
		
		return unCompressBuff;
	}		
}




