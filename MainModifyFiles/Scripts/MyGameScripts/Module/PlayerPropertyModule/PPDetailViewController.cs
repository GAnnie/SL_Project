using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules;

public class PPDetailViewController : MonoBehaviour,IViewController {

	private int _maxValue;
	private PPDetailView _view;
	private AptitudeProperties _apInfo;
	public List<PropertyItemController> apItemList;

	public event System.Action OnSave;
	public event System.Action OnCancel;

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PPDetailView> ();
		_view.Setup(this.transform);


		_maxValue = DataHelper.GetStaticConfigValue (H1StaticConfigs.PRESET_ONUPGRADE_POTENTIAL_TOTAL_POINT, 10);

		apItemList = new List<PropertyItemController>(5);
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.PROPERTYITEM_WIDGET) as GameObject;
		for(int i=0;i<5;++i){
			GameObject item = NGUITools.AddChild(_view.infoItemGrid.gameObject,itemPrefab);
			var controller = item.GetMissingComponent<PropertyItemController>();
			controller.SetEditable(true);
			controller.OnAdd += OnApItemAdd;
			controller.OnMinus += OnApItemMinus;
			apItemList.Add(controller);
		}
		
		apItemList[0].InitItem("体质",_apInfo.constitution);
		apItemList[1].InitItem("魔力",_apInfo.intelligent);
		apItemList[2].InitItem("力量",_apInfo.strength);
		apItemList[3].InitItem("耐力",_apInfo.stamina);
		apItemList[4].InitItem("敏捷",_apInfo.dexterity);
		RegisterEvent();
	}

	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.closeBtn.onClick,CloseView);
		EventDelegate.Set(_view.bgMaskEventTrigger.onClick,CloseView);

		EventDelegate.Set(_view.saveBtn.onClick,Save);
		EventDelegate.Set(_view.cancelBtn.onClick,Cancel);
	}
	
	public void Dispose ()
	{
		
	}
	#endregion

	public void Open(AptitudeProperties ap)
	{
		_apInfo = ap;
		InitView();
	}

	private void Save(){
		int allPointCount = 0;
		for(int i=0;i<apItemList.Count;++i){
			allPointCount += apItemList[i].GetValue();
		}
		if(allPointCount == _maxValue)
		{
			if(OnSave != null)
				OnSave();
		}
		else
			TipManager.AddTip("五项属性分配点数总和必须为10，请重新分配");
	}

	private void Cancel(){
		if(OnCancel != null)
			OnCancel();
	}

	public void UpdateApInfoDto(){
		_apInfo.constitution = apItemList[0].GetValue();
		_apInfo.intelligent = apItemList[1].GetValue();
		_apInfo.strength = apItemList[2].GetValue();
		_apInfo.stamina = apItemList[3].GetValue();
		_apInfo.dexterity = apItemList[4].GetValue();
	}

	private void OnApItemAdd(PropertyItemController item){
		int val = item.GetValue();
		if(++val > _maxValue)
			val = _maxValue;

		item.SetValue(val,true);
	}

	private void OnApItemMinus(PropertyItemController item){
		int val = item.GetValue();
		if(--val < 0)
			val = 0;

		item.SetValue(val,true);
	}
	
	public void CloseView()
	{
		ProxyPlayerPropertyModule.ClosePPDetailView();
	}
}
