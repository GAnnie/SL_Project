using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.charactor.data;

public class PetAptitudeTabContentController : MonoBehaviour {

	private List<PropertySliderController> _petApInfoItemList;
	private PetPropertyInfo _petInfo;

	public void InitView(){
		_petApInfoItemList = new List<PropertySliderController>(6);
		GameObject itemGrid = this.transform.Find("itemGrid").gameObject;
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYSLIDER_WIDGET) as GameObject;
		List<string> baseApTitleList = new List<string>(6){"攻击资质","防御资质","体力资质","法力资质","速度资质","寿命"};
		List<int> hintIdList = new List<int>(6){13,14,15,16,17,18};
		for (int i=0; i<6; ++i) {
			GameObject item = NGUITools.AddChild (itemGrid, itemPrefab);
			var controller = item.GetMissingComponent<PropertySliderController> ();
			controller.InitItem(baseApTitleList[i],hintIdList[i]);
			_petApInfoItemList.Add (controller);
		}
	}

	public void UpdateViewInfo(PetPropertyInfo petInfo){
		_petInfo = petInfo;
		BaseAptitudeProperties baseApInfo = petInfo.petDto.baseAptitudeProperty;
		BaseAptitudeProperties originApInfo = petInfo.pet.baseAptitudeProperties;
		SetupApInfo(_petApInfoItemList[0],baseApInfo.attack,baseApInfo.maxAttack,originApInfo.attack);
		SetupApInfo(_petApInfoItemList[1],baseApInfo.defense,baseApInfo.maxDefense,originApInfo.defense);
		SetupApInfo(_petApInfoItemList[2],baseApInfo.physical,baseApInfo.maxPhysical,originApInfo.physical);
		SetupApInfo(_petApInfoItemList[3],baseApInfo.magic,baseApInfo.maxMagic,originApInfo.magic);
		SetupApInfo(_petApInfoItemList[4],baseApInfo.speed,baseApInfo.maxSpeed,originApInfo.speed);

		if(petInfo.petDto.lifePoint == DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_IMMORTAL_LIEF_POINT,-999))
			_petApInfoItemList[5].SetValLbl("永生");
		else
			_petApInfoItemList[5].SetValLbl(petInfo.petDto.lifePoint.ToString());
	}

	public void CleanUpViewInfo(){
		SetupApInfo(_petApInfoItemList[0],0,0,0);
		SetupApInfo(_petApInfoItemList[1],0,0,0);
		SetupApInfo(_petApInfoItemList[2],0,0,0);
		SetupApInfo(_petApInfoItemList[3],0,0,0);
		SetupApInfo(_petApInfoItemList[4],0,0,0);

		_petApInfoItemList[5].SetValLbl("------");
	}

	private void SetupApInfo(PropertySliderController com,int val,int maxVal,int baseVal){
		if(maxVal != 0)
			com.SetSliderVal(PetModel.Instance.GetPetBaseApPercent(_petInfo,val,maxVal,baseVal));
		else
			com.SetSliderVal(0f);
		com.SetValLbl(string.Format("{0}/{1}",val,maxVal));
	}
}
