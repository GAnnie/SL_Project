// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  AMF3Thread.cs
// Author   : senkay
// Created  : 4/17/2013 8:20:25 PM
// Purpose  : 
// **********************************************************************


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.proto;

public class ProtobufFileThread
{
	private byte[] _bytes = null;
    private object _obj = null;
    private string _filePath = null;

    public void SaveFile(byte[] bytes, string filePath)
    {
        _bytes = bytes;
        _filePath = filePath;

        Thread thread = new Thread(new ThreadStart(DoSaveByteFile));
        thread.Start();
    }	
	
    public void SaveFile(object obj, string filePath)
    {
        _obj = obj;
        _filePath = filePath;

        Thread thread = new Thread(new ThreadStart(DoSaveObjFile));
        thread.Start();
    }

    private void DoSaveObjFile()
    {
        _bytes = ProtobufUtilsNet.packIntoData(_obj);
		DoSaveByteFile();
    }
	
    private void DoSaveByteFile()
    {
        FileStream file;
        try
        {
            string dirPath = _filePath.Substring(0, _filePath.LastIndexOf('/')+1);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (File.Exists(_filePath))
            {
                file = new FileStream(_filePath, FileMode.Truncate);
            }
            else
            {
                file = new FileStream(_filePath, FileMode.CreateNew);
            }
			
	        BinaryWriter bw = new BinaryWriter(file);
	        bw.Write(_bytes);
	        bw.Close();
	        file.Close();
        }
        catch (Exception ex)
        {
			Debug.LogException(ex);
            return;
        }

		_obj = null;
		_bytes = null;
    }	
}
