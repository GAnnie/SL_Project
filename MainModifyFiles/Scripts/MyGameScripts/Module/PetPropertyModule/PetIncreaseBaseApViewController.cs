using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.charactor.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules;

public class PetIncreaseBaseApViewController : MonoBehaviour,IViewController {
	
	private PetIncreaseBaseApView _view;
	private PetPropertyInfo _petInfo;
	private List<PropertySliderController> _petApInfoItemList;
	private int _propCount;

	public void Open(PetPropertyInfo petInfo){
		_petInfo = petInfo;
		InitView();
	}
	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PetIncreaseBaseApView>();
		_view.Setup(this.transform);

		_view.petIconSprite.spriteName = "pet_default";
		_view.petNameLbl.text = _petInfo.petDto.name;
		_view.petLvLbl.text = string.Format("等级：{0}",_petInfo.petDto.level);

		int propId = DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_INCREASE_BASE_APTITUDE_ITEM_ID,10034);
		_view.propIconSprite.spriteName = propId.ToString();
		_propCount = BackpackModel.Instance.GetItemCount(propId);

		_petApInfoItemList = new List<PropertySliderController>(5);
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.PROPERTYSLIDER_WIDGET) as GameObject;
		List<string> baseApTitleList = new List<string>(6){"攻击资质","防御资质","体力资质","法力资质","速度资质"};
		List<int> hintIdList = new List<int>(6){13,14,15,16,17,18};
		for (int i=0; i<5; ++i) {
			GameObject item = NGUITools.AddChild (_view.baseApItemGrid.gameObject, itemPrefab);
			var controller = item.GetMissingComponent<PropertySliderController> ();
			controller.InitItem(i,baseApTitleList[i],hintIdList[i],OnSelectIncreaseApOption);
            _petApInfoItemList.Add (controller);
        }

		UpdateViewInfo();
		RegisterEvent();
	}
	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.closeBtn.onClick,OnClickClose);
	}
	public void Dispose ()
	{

	}
	#endregion

	private void UpdateViewInfo(){
		_view.propCountLbl.text = _propCount.ToString();

		BaseAptitudeProperties basePetApInfo = _petInfo.petDto.baseAptitudeProperty;
		BaseAptitudeProperties originPetApInfo = _petInfo.pet.baseAptitudeProperties;
		List<int> curApValList = new List<int>(5){basePetApInfo.attack,basePetApInfo.defense,basePetApInfo.physical,basePetApInfo.magic,basePetApInfo.speed};
		List<int> maxApValList = new List<int>(5){basePetApInfo.maxAttack,basePetApInfo.maxDefense,basePetApInfo.maxPhysical,basePetApInfo.maxMagic,basePetApInfo.maxSpeed};
		List<int> baseApValList = new List<int>(5){originPetApInfo.attack,originPetApInfo.defense,originPetApInfo.physical,originPetApInfo.magic,originPetApInfo.speed};
		for(int i=0;i<_petApInfoItemList.Count;++i){
			var slider = _petApInfoItemList[i];
			if(curApValList[i] == maxApValList[i]){
				slider.SetValLbl(string.Format("{0}/{1}(已满)",curApValList[i],maxApValList[i]));
				slider.SetSliderVal(1f);
				slider.SetValueBtnActive(false);
			}else{
				float percent = PetModel.Instance.GetPetBaseApPercent(_petInfo,curApValList[i],maxApValList[i],baseApValList[i]);
				slider.SetSliderVal(percent);
				slider.SetValueBtnActive(true);
				
				int intervalMin = 0,intervalMax =0;
				PetBaseAptitudeIncreaseInfo increaseInfo = GetBaseApIncreaseInfo(percent);
				if(increaseInfo != null){
					int canAddPoint = maxApValList[i] - curApValList[i];
					intervalMin = Mathf.Min(canAddPoint,increaseInfo.minGain);
					intervalMax = Mathf.Min(canAddPoint,increaseInfo.maxGain);
				}
				
				if(intervalMin == intervalMax){
					slider.SetValLbl(string.Format("{0}/{1}(增加{2}点)",curApValList[i],maxApValList[i],intervalMin));
				}else{
					slider.SetValLbl(string.Format("{0}/{1}(增加{2}~{3}点)",curApValList[i],maxApValList[i],intervalMin,intervalMax));
				}
			}
		}
	}

	private PetBaseAptitudeIncreaseInfo GetBaseApIncreaseInfo(float percent){
		List<PetBaseAptitudeIncreaseInfo> baseApIncreaseInfoList= DataCache.getArrayByCls<PetBaseAptitudeIncreaseInfo>();
		//百分比为0时直接返回
		if(percent == 0f)
			return baseApIncreaseInfoList[0];
		
		int curVal = Mathf.FloorToInt(percent * 100.0f);
		int minVal = 0;
		for(int i=0;i<baseApIncreaseInfoList.Count;++i){
			if(curVal > minVal && curVal <= baseApIncreaseInfoList[i].id){
                return baseApIncreaseInfoList[i];
            }else
                minVal = baseApIncreaseInfoList[i].id;
        }
        
        return null;
    }
    
    private void OnSelectIncreaseApOption(int index){
		if(_propCount > 0){
			if(_petInfo.GetCurBaseApVal(index) == _petInfo.GetMaxBaseApVal(index)){
				string titleName = _petApInfoItemList[index].GetTitleName();
				TipManager.AddTip(string.Format("{0}的{1}已达上限，不能继续提升",_petInfo.petDto.name,titleName));
	        }else
				PetModel.Instance.IncreaseBaseAptitude(_petInfo,index,()=>{
					if(--_propCount < 0)
						_propCount = 0;
					UpdateViewInfo();
				});
		}
		else
			TipManager.AddTip("资质丹不足");
    }

	void OnClickClose(){
		ProxyPetPropertyModule.CloseIncreaseBaseApView();
	}
}
