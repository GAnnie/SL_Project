using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResourceQueueLoader
{
	private static readonly ResourceQueueLoader instance = new ResourceQueueLoader ();
	public static ResourceQueueLoader Instance {
		get {
			return instance;
		}
	}

	private Queue<QueueLoaderInfo> _loaderQueue;
	private QueueLoaderInfo _currentLoaderInfo = null;

	private ResourceQueueLoader ()
	{
		_loaderQueue = new Queue<QueueLoaderInfo>();
	}

	public void Load(string path, Action<UnityEngine.Object> onLoadFinish, ResourcePoolManager.PoolType poolType)
	{
		QueueLoaderInfo loaderInfo = new QueueLoaderInfo();
		loaderInfo.path = path;
		loaderInfo.onLoadFinish = onLoadFinish;
		loaderInfo.poolType = poolType;

		_loaderQueue.Enqueue(loaderInfo);
		LoadNext();
	}

	public void Remove(Action<UnityEngine.Object> onLoadFinish)
	{
	}

	private void LoadNext()
	{
		if (_currentLoaderInfo == null && _loaderQueue.Count > 0)
		{
			_currentLoaderInfo = _loaderQueue.Dequeue();
			ResourcePoolManager.Instance.Spawn(_currentLoaderInfo.path, OnLoadFinish, _currentLoaderInfo.poolType);
		}
	}

	private void OnLoadFinish(UnityEngine.Object obj)
	{
		_currentLoaderInfo.onLoadFinish(obj);
		_currentLoaderInfo = null;
		LoadNext();
	}
}

class QueueLoaderInfo
{
	public string path = "";
	public Action<UnityEngine.Object> onLoadFinish;
	public ResourcePoolManager.PoolType poolType;
}

