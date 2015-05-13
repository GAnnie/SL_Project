// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  AssetsPoolManager.cs
// Author   : wenlin
// Created  : 2013/6/19 16:51:09
// Purpose  : 
// **********************************************************************

using UnityEngine;
using System;
using System.Collections.Generic;

public class AssetsPoolManager
{
    //U3d's data dictionary
    private Dictionary<string, U3DDataBase> _u3DDataBasePool = null;

    public AssetsPoolManager()
    {
        _u3DDataBasePool = new Dictionary<string, U3DDataBase>();
    }

    /// <summary>
    /// Is assign u3dData exist
    /// </summary>
    /// <param name="assetName"></param>
    public bool IsU3DExist( string assetName )
    {
        if (_u3DDataBasePool == null) return false;

        return _u3DDataBasePool.ContainsKey(assetName);
    }

    /// <summary>
    /// Get U3DData
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public U3DDataBase GetU3DData(string assetName)
    {
        if (_u3DDataBasePool == null) return null;

        U3DDataBase data = _u3DDataBasePool[assetName];
        data.Reset();
        return data;
    }

    /// <summary>
    /// Add U3DData 
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="u3dDataBase"></param>
    /// <returns></returns>
    public bool AddU3DData(string assetName, U3DDataBase u3dDataBase)
    {
        if (u3dDataBase == null || _u3DDataBasePool == null) return false;

        if (_u3DDataBasePool.ContainsKey(assetName))
        {
            return false;
        }
        else
        {
            _u3DDataBasePool.Add(assetName, u3dDataBase);
        }
        return true;
    }

    /// <summary>
    /// Check Assets
    /// </summary>
    private float _oldTime = 0.0f;
    private const float _MaxTime = 1.0f;
    public void Check()
    {
        float delay = Time.time - _oldTime;
        //GameDebuger.Log("Delay Time : " + delay.ToString());

        if (delay < _MaxTime) return;
        _oldTime = Time.time;

        List<string> _deleteList = null;
        foreach (KeyValuePair<string, U3DDataBase> item in _u3DDataBasePool)
        {
            if (item.Value.IsOvertake())
            {
                item.Value.Unload();

                if (_deleteList == null) _deleteList = new List<string>();
                _deleteList.Add(item.Key);
            }
            else
            {
                item.Value.AddAccNumber();
            }
        }

        if (_deleteList != null)
        {
            foreach (string deleteName in _deleteList)
            {
                _u3DDataBasePool.Remove(deleteName);
            }

            _deleteList.Clear();
            _deleteList = null;
            //Resource Unload
            //GameDebuger.Log("AssetsPoolManager : UnloadUnuseAssets");
            //Resources.UnloadUnusedAssets();
        }

    }

#if UNITY_DEBUG
    public int CollectAssetbundleMemory()
    {
        int totalMemory = 0;
        foreach (KeyValuePair<string, U3DDataBase> item in _u3DDataBasePool)
        {
            totalMemory += item.Value.AssetSize();
        }

        return totalMemory;
    }
#endif


    public void Delete(string u3dName)
    {
        if (_u3DDataBasePool.ContainsKey(u3dName))
        {
            U3DDataBase dataBase = _u3DDataBasePool[u3dName];
            dataBase.Unload();
            _u3DDataBasePool.Remove(u3dName);
        }
            
    }

    /// <summary>
    /// Dispose AssetPoolManager
    /// </summary>
    public void Dispose()
    {
        if (_u3DDataBasePool == null) return;
        foreach (KeyValuePair<string, U3DDataBase> item in _u3DDataBasePool)
        {
            item.Value.Unload();
        }
        _u3DDataBasePool.Clear();
        _u3DDataBasePool = null;
    }
}
