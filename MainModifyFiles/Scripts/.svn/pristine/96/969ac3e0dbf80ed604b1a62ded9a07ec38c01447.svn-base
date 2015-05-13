// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TipManager.cs
// Author   : SK
// Created  : 2013/3/5
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TipManager
{
	static public FloatTipText mDialogueText=null;

	//	队列数据体
	struct TipStruct {
		public string tipInfo;
		public Color tipColor;
		public float delayTime;

		public TipStruct(string info, Color color, float delay) {
			tipInfo = info;
			tipColor = color;
			delayTime = delay;
		}
	}
	//	队列用于存储获得道具提示
	private static Queue< TipStruct > tipQueue = new Queue<TipStruct>();
	private static bool _isWaiting = true;
	private static float _intervalDelayTime = 0.1f;
	private static string _coolDownName = "tipManager_";

	//	装载
	static public void Setup()
	{
		GameObject dialogueHudText = NGUITools.AddChild (LayerManager.Instance.floatTipAnchor,
		                                                 (GameObject)ResourcePoolManager.Instance.SpawnUIPrefab ("Prefabs/HUDText/FloatTipText"));
		mDialogueText = dialogueHudText.GetComponentInChildren<FloatTipText> ();
		dialogueHudText.transform.localPosition = new Vector3( 0f , 70f, 0f);
	}
	
	static public void AddTip(string tip, float stayDuration = 0.5f)
	{
		AddColorTip(tip, Color.white ,stayDuration);
	}
	
	static public void AddColorTip(string tip, Color color,float stayDuration)
	{
		if (!string.IsNullOrEmpty(tip)){
			if (mDialogueText != null){
				//mDialogueText.Add(tip, color,stayDuration);
				TipStruct tipStruct = new TipStruct(tip, color, stayDuration);

				tipQueue.Enqueue(tipStruct);

				if (_isWaiting) {
					_isWaiting = false;
					CoolDownManager.Instance.SetupCoolDown(_coolDownName, _intervalDelayTime, null, OnFinishedTime);
				}
			}
		}
	}

	private static void OnFinishedTime() {
		//	提示
		TipStruct tmpTipStruct = tipQueue.Dequeue();
		mDialogueText.Add(tmpTipStruct.tipInfo, tmpTipStruct.tipColor, _intervalDelayTime);

		//	重新开始
		if (tipQueue.Count > 0) {
			CoolDownManager.Instance.SetupCoolDown(_coolDownName, _intervalDelayTime,null,OnFinishedTime);
		} else {
			_isWaiting = true;
		}
	}
	//	**************	End RefreTime	***********************

	public static void AddGainCurrencyTip(long changeValue, string currencyType)
	{
		TipManager.AddTip(string.Format("获得{0}{1}",Mathf.Abs(changeValue).ToString("#,#").WrapColor(ColorConstant.Color_Tip_GainCurrency),currencyType));
	}

	public static void AddLostCurrencyTip(long changeValue, string currencyType)
	{
		TipManager.AddTip(string.Format("消耗{0}{1}",Mathf.Abs(changeValue).ToString("#,#").WrapColor(ColorConstant.Color_Tip_LostCurrency),currencyType));
	}

	public static void AddGainCurrencyTip(long changeValue, int itemId)
	{
		TipManager.AddTip(string.Format("获得{0}{1}",Mathf.Abs(changeValue).ToString("#,#").WrapColor(ColorConstant.Color_Tip_GainCurrency),ItemIconConst.GetIconConstByItemId(itemId)));
	}
	
	public static void AddLostCurrencyTip(long changeValue, int itemId)
	{
		TipManager.AddTip(string.Format("消耗{0}{1}",Mathf.Abs(changeValue).ToString("#,#").WrapColor(ColorConstant.Color_Tip_LostCurrency),ItemIconConst.GetIconConstByItemId(itemId)));
	}
}

