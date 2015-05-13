// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  RequestLoadingTip.cs
// Author   : SK
// Created  : 2013/8/29
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

/// <summary>
/// Request loading tip.
/// </summary>
public class RequestLoadingTip : MonoBehaviour
{
	public static RequestLoadingTip instance;
	private static RequestLoadingTipPrefab _view;
	
	public static void Setup()
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/window/RequestLoadingTipPrefab") as GameObject;
		if (prefab != null)
		{
			GameObject parent = LayerManager.Instance.frontAnchor.transform.Find("LockScreenPanel").gameObject;
			if (parent == null)
			{
				Debug.LogError("RequestLoadingTip Setup not find parent");
				return;
			}
			GameObject loadingTip = NGUITools.AddChild(parent, prefab);
			_view = loadingTip.AddMissingComponent<RequestLoadingTipPrefab>();
			_view.Setup(loadingTip.transform);

			loadingTip.name = "RequestLoadingTip";
			instance = loadingTip.AddMissingComponent<RequestLoadingTip>();
		}
	}

	// Use this for initialization
	void Start ()
	{
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_view.CircleSprite_UISprite.gameObject.activeInHierarchy)
		{
			_view.CircleSprite_UISprite.fillAmount += Time.deltaTime * (1.0f/20f);
		}
	}
	
	public static void Show(string tip, bool showCircle = false){
        if (instance != null)
        {
            instance._Show(tip, showCircle); 
        }
	}
	
	public static void Stop()
    {
        if (instance != null)
        {
            instance._Stop(); 
        }
	}
	
	public int _loadingCount = 0;
	
	public void _Show(string tip, bool showCircle = false){
		
		
		_loadingCount++;
		
//		GameDebuger.Log("RequestLoadingTipShow "+tip +" "+_loadingCount);
		
		//CancelInvoke("DelayShowCircle");
		CancelInvoke("DelayStop");

		_view.LoadingGroup_Transform.gameObject.SetActive(showCircle);
		
		if (showCircle)
		{
			_view.CircleSprite_UISprite.fillAmount = 0;
			_view.TipLabel_UILabel.text = tip;
		}
		this.gameObject.SetActive(true);
	}
	
	private void DelayShowCircle(){
		//_circleSprite.SetActive(true);
	}
	
	public void _Stop(){
		_loadingCount--;
		
		GameDebuger.Log("RequestLoadingTipStop" + " "+_loadingCount);
		
		if (_loadingCount > 0){
			return;
		}
		
		_loadingCount = 0;
		
		CancelInvoke("DelayStop");
		Invoke( "DelayStop" , 0.05f);
	}
	
	private void DelayStop(){
		this.gameObject.SetActive(false);
	}
}

