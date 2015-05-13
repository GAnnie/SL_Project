// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  AMF3Thread.cs
// Author   : senkay
// Created  : 4/17/2013 8:20:25 PM
// Purpose  : 
// **********************************************************************

//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using com.nucleus.h1.logic.core.modules;

public class ProtobufFileListThread
{
	
	private Queue<DataListInfo> dataListInfos;

    public void SaveFileList(Dictionary<string, DataList> dataCaches, Dictionary<string, byte[]> dataCaches2)
    {
		dataListInfos = new Queue<DataListInfo>();
		
        foreach (KeyValuePair<string, DataList> cls in dataCaches)
        {
			string dataInfoPath = DataManager.GetDataPath(cls.Key);
			
			DataListInfo info = new DataListInfo();
			info.filePath = dataInfoPath;
			byte[] bytes = com.nucleus.h1.logic.core.modules.proto.ProtobufUtilsNet.packIntoData(cls.Value);
			info.bytes = bytes;
			
			dataListInfos.Enqueue(info);
        }
		
        foreach (KeyValuePair<string, byte[]> cls in dataCaches2)
        {
			string dataInfoPath = DataManager.GetDataPath(cls.Key);
			
			DataListInfo info = new DataListInfo();
			info.filePath = dataInfoPath;
			info.bytes = cls.Value;
			
			dataListInfos.Enqueue(info);
        }		
		
        Thread thread = new Thread(new ThreadStart(DoSaveObjFile));
		thread.Start();
    }

    private void DoSaveObjFile()
    {
		Debug.Log("ProtobufFileListThread Start save file count=" + dataListInfos.Count);
		
		while(dataListInfos.Count > 0){
			DataListInfo info = dataListInfos.Dequeue();
			DoSaveByteFile(info);
		}
		
		GameDebuger.Log("ProtobufFileListThread End save file");
    }
	
    private void DoSaveByteFile(DataListInfo info)
    {
		string filePath = info.filePath;
		byte[] bytes = info.bytes;
		
        FileStream file;
        try
        {
            string dirPath = filePath.Substring(0, filePath.LastIndexOf('/')+1);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            if (File.Exists(filePath))
            {
                file = new FileStream(filePath, FileMode.Truncate);
            }
            else
            {
                file = new FileStream(filePath, FileMode.CreateNew);
            }
			
	        BinaryWriter bw = new BinaryWriter(file);
	        bw.Write(bytes);
	        bw.Close();
	        file.Close();
        }
        catch (Exception ex)
        {
			Debug.LogException(ex);
            return;
        }
    }	
}

class DataListInfo
{
	public string filePath;
	public byte[] bytes;
}
