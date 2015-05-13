using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
	
/**
 *数据管理 
 * @author SK
 * 
 */
	 
//namespace com.nucleus.h1.logic.common.data
//{		
using com.nucleus.commons.message;
using com.nucleus.commons.dispatcher;

public class DataManager
{
    public static string DataDir = "data/";
	public static string DataVer = "V2.6";
	
	public static bool UseCache = true;
	
	private DataManager ()
	{
	}
		
	private Dictionary< System.Type,System.Func<GeneralRequestInfo> > _dataServices;
    private Dictionary<string, DataList> _dataCaches;
	private Dictionary<string, byte[]> _dataCaches2;	
	private Dictionary<string, byte[]> _dataCachesNew;
		
	private List< Type > keyList;
	private int keyListIdx = 0;	
		
	public delegate void CallbackDelegate ();
		
	CallbackDelegate _callBack;
	
	public delegate void DataLoadingMessge (string msg);
	public delegate void DataLoadingMsgProcess (float msgProcess);
		
	public DataLoadingMessge dataLoadingMessge;
	public DataLoadingMsgProcess dataLoadingMsgProcess;

    private void ShowDataLoadingMessage(string message)
    {
        if (dataLoadingMessge != null)
        {
            dataLoadingMessge(message);
        }
    }

	private void loadNextData ()
	{
		float basePercent = (float)keyListIdx/keyList.Count;
		ShowLoadPrecent(basePercent);
		
		//Debug.Log("loadNextData "+_dataCaches.Count+":"+_dataCaches2.Count+":"+_dataCachesNew.Count);
		
		if (keyListIdx < keyList.Count) {
			Type dataServiceType = keyList [keyListIdx];
			Func< GeneralRequestInfo > function = _dataServices [dataServiceType];
			
			++keyListIdx;
			loadData (dataServiceType, function);
		} else {
			new ProtobufFileListThread().SaveFileList(_dataCaches, _dataCachesNew);
			
			SetupDataCache();
		}
	}
		
	private Type _currentLoadData;
		
	private int count = 0;
	
	private void loadData (Type cls, System.Func<GeneralRequestInfo> request)
	{
		_currentLoadData = cls;

		GeneralRequestInfo generalRequest = request ();
		
		if (count > 0){
			ServiceRequestAction.requestServer (generalRequest, "", onDataRequestSuccess, onError, null, null, null, onDataRequestSuccessWithBytes);		
		}else{
			ServiceRequestAction.requestServer (generalRequest, "", onDataRequestSuccess, onError, null, null, null, null);		
		}		
		
		count++;
	}
		
	void onDataRequestSuccessWithBytes(ProtoByteArray protoByteArray)
	{
		Debug.Log("onDataRequestSuccessWithBytes "+_currentLoadData.ToString());
		
//		object obj = protoByteArray.ReadObject();
//		
//		Debug.Log("obj="+obj);
//		if (obj is ErrorResponse){
//			onError(obj as ErrorResponse);
//		}else{
//			onDataRequestSuccess(obj as GeneralResponse);
//		}
		
		protoByteArray.Position = 0;
		byte[] bytes = new byte[protoByteArray.Length];
		protoByteArray.ReadBytes(bytes, 0, protoByteArray.Length);
		_dataCaches2[_currentLoadData.ToString()] = bytes;
		_dataCachesNew[_currentLoadData.ToString()] = bytes;
		_currentLoadData = null;
		loadNextData ();		
	}
	
	void onDataRequestSuccess (GeneralResponse e)
	{
		Debug.Log("GeneralResponse serial="+e.serial);
		DataList dataList = (DataList)e;

        _dataCaches[_currentLoadData.ToString()] = dataList;
		_currentLoadData = null;
		loadNextData ();
	}
		
	void onError (ErrorResponse e)
	{
		Debug.Log("ErrorResponse serial="+e.serial);

		string fullName = _currentLoadData.ToString ();
		
		if (_newDataVersionList != null){
			
            String[] names = fullName.Split(new char[] { '.' });
            string shortName = names[names.Length - 1];			
			
			//如果是有错误的数据， 则把版本号修改，下次登录时获取
	        foreach (DataListVersion version in _newDataVersionList.items)
	        {
	            if (version.type == shortName)
	            {
					version.ver = "1.0.1";
					break;
	            }
	        }
		}
		
		GameDebuger.Log(fullName + " " + e.message);
		_currentLoadData = null;
		loadNextData ();
	}

	//---------------------------------------------------------------------------------	
	
	private bool _preLoadingFinish = false;
	/// <summary>
	/// 预先加载缓存的数据
	/// </summary>
	public void PreLoadDataCache(){
		_preLoadingFinish = false;
		_newDataVersionList = null;
		_oldDataVersionList = null;
		
        if (Version.Release)
        {
            UseCache = true;
        }
		
		if (SocketManager.IsToolMode)
		{
			UseCache = false;
		}
		
		if (UseCache)
		{
			GameObject go = GameObject.Find("DataManagerThread");
			
			if (go == null)
			{
				go = new GameObject("DataManagerThread");
			}
			DataManagerThread thread = GameObjectExt.GetMissingComponent<DataManagerThread>(go);
			thread.loadDataFinish = OnLoadDataFinish;
            string dataInfoPath = GetDataPath(typeof(DataListVersion).ToString());
	        thread.ReadDataCache(dataInfoPath);	
		}
		else
		{
			_preLoadingFinish = true;
		}
	}

    public static string GetDataPath(string dataType)
    {
        return GetDataDir() + dataType + DataVer + ".dataInfo.bytes";
    }
	
	private static string GetDataDir(){
		return Application.persistentDataPath + "/" + DataDir;
	}

	private object _dataObj;
    public void OnLoadDataFinish(object dataObj)
    {
		_preLoadingFinish = true;
		_dataObj = dataObj;
		
		if (_newDataVersionList != null){
			CheckDataVersion(_dataObj);
		}
    }
	
	public void Initialize (CallbackDelegate callBack)
	{
		_callBack = callBack;
		
		_dataServices = DataCacheMap.services ();
		
		_dataCaches = new Dictionary<string, DataList>();
		_dataCaches2 = new Dictionary<string, byte[]>();
		_dataCachesNew = new Dictionary<string, byte[]>();
		
		keyList = new List<Type> ();
		keyListIdx = 0;

		if (_dataServices == null)
            GameDebuger.Log("_dataServices == null");

        ShowDataLoadingMessage("数据初始化，请稍后。。。");

        if (Version.Release)
        {
            UseCache = true;
        }
		
		if (SocketManager.IsToolMode)
		{
			UseCache = false;
		}
		
		GameDebuger.Log("DataManager start");

		ServiceRequestAction.requestServer(StaticDataService.dataTableVersions(), "GetDataVersion", onDataVersionListSuccess, onError);
	}

    private DataList _newDataVersionList;
    private DataList _oldDataVersionList;
    private void onDataVersionListSuccess(GeneralResponse e)
    {
        _newDataVersionList = (e as DataList);

        _dataCaches.Add(typeof(DataListVersion).ToString(), _newDataVersionList);
		
		if (UseCache){
			if (_preLoadingFinish){
				CheckDataVersion(_dataObj);
			}			
		}else{
			foreach (KeyValuePair<Type, System.Func<GeneralRequestInfo>> keyVal in _dataServices)
            {
				if (keyVal.Key.ToString() != typeof(DataListVersion).ToString()){
					keyList.Add(keyVal.Key);
				}
            }		
			
			loadNextData();
		}
    }
	
	private void CheckDataVersion(object dataObj){
        GameTimeDisplay.BeginTime(GameTimeDisplay.GameTimeType.STATIC_DATA_TIME_TYPE);

        if (dataObj != null)
        {
            _oldDataVersionList = dataObj as DataList;

			foreach (KeyValuePair<Type, System.Func<GeneralRequestInfo>> keyVal in _dataServices)
            {
                string fullName = keyVal.Key.ToString();

                if (fullName == typeof(DataListVersion).ToString())
                {
                    continue;
                }

                String[] names = fullName.Split(new char[] { '.' });
                string shortName = names[names.Length - 1];
                DataListVersion oldDataVersion = GetOldDataListVersion(shortName);
                DataListVersion newDataVersion = GetNewDataListVersion(shortName);

                if (oldDataVersion != null && newDataVersion != null && oldDataVersion.ver == newDataVersion.ver && newDataVersion.ver != "1.0.1")
                {
                    byte[] bytes = ProtobufFileUtils.LoadBytesFromFile(GetDataPath(fullName));
                    if (bytes != null)
                    {
                        _dataCaches2.Add(fullName, bytes);
                    }
                    else
                    {
                        keyList.Add(keyVal.Key);
                    }
                }
                else
                {
                    keyList.Add(keyVal.Key);
                }
            }
			
			GameDebuger.Log("CheckDataVersion needLoadDataCount=" + keyList.Count);
			
            if (keyList.Count > 0)
            {
                loadNextData();
            }
            else
            {
                SetupDataCache();
            }
        }
        else
        {
			foreach (KeyValuePair<Type, System.Func<GeneralRequestInfo>> keyVal in _dataServices)
            {
                keyList.Add(keyVal.Key);
            }
			GameDebuger.Log("CheckDataVersion needLoadDataCount=" + keyList.Count);
            loadNextData();
        }
	}

    private DataListVersion GetOldDataListVersion(string type)
    {
        if (_oldDataVersionList == null)
        {
            return null;
        }
        else
        {
            foreach (DataListVersion version in _oldDataVersionList.items)
            {
                if (version.type == type)
                {
                    return version;
                }
            }
            return null;
        }
    }

    private DataListVersion GetNewDataListVersion(string type)
    {
        if (_newDataVersionList == null)
        {
            return null;
        }
        else
        {
            foreach (DataListVersion version in _newDataVersionList.items)
            {
                if (version.type == type)
                {
                    return version;
                }
            }
            return null;
        }
    }

	private void ShowLoadPrecent(float basePercent)
	{
		int percent = Mathf.FloorToInt(basePercent*100);
		ShowDataLoadingMessage(percent.ToString() + "％");
		//dataLoadingMessge (keyListIdx + "/" + keyList.Count);
		if (dataLoadingMsgProcess != null) {
			dataLoadingMsgProcess(basePercent);
		}
	}

	private void SetupDataCache(){
		GameDebuger.Log("DataManager finish");

		ShowLoadPrecent(1f);
		
		GameTimeDisplay.EndTime(GameTimeDisplay.GameTimeType.STATIC_DATA_TIME_TYPE);

		DataCache.setup ( _dataCaches );
		DataCache.setup2 ( _dataCaches2 );
		_dataServices = null;
		_dataCaches = null;
		_dataCaches2 = null;
		_dataCachesNew = null;

        if (_callBack != null)
        {
            _callBack();
            _callBack = null;
        }
	}

	public static void CleanData()
	{
		Directory.Delete(GetDataDir(), true);
	}
	
	private static DataManager instance = null;

	public static DataManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new DataManager ();
			}
			return instance;
		}
	}
}
//}