// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  DataManagerThread.cs
// Author   : senkay
// Created  : 4/17/2013 8:52:05 PM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DataManagerThread : MonoBehaviour
{
    private object _cacheObject;

    private string _dataInfoPath;
    private bool _dataReady = false;
	
	public delegate void LoadDataFinish(object dataObj);
	public LoadDataFinish loadDataFinish;
	
	void Awake()
	{
		DontDestroyOnLoad( this.gameObject );
	}
	
    public void ReadDataCache(string dataInfoPath)
    {
        _dataInfoPath = dataInfoPath;
        _dataReady = false;

        Thread thread = new Thread(new ThreadStart(ReadObject));
        thread.Start();
    }

    private void ReadObject()
    {
        _cacheObject = ProtobufFileUtils.LoadObjFromFile(_dataInfoPath);
        _dataReady = true;
    }

    void Update()
    {
        if (_dataReady)
        {
            _dataReady = false;

            object cacheObject = _cacheObject;
            _cacheObject = null;

            LoadDataFinish tmpLoadDataFinish = loadDataFinish;
            loadDataFinish = null;

            if (tmpLoadDataFinish != null)
            {
                tmpLoadDataFinish(cacheObject);
                tmpLoadDataFinish = null;
			}
            cacheObject = null;
        }
    }
}
