using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.equipment.data;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.data;

public class PetEqCombineViewController : MonoBehaviour,IViewController {

	private PetEquipmentCombineView _view;
	private List<PackItemDto> _packEqItemDtoList;
	private List<BaseItemCellController> _packItemSlotList;

	private List<BaseItemCellController> _combineItemSlotList;
	private List<int> _combineIndexList;

	private BaseItemCellController _resultItemSlot;
	private PackItemDto _resultItemDto;
	
	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PetEquipmentCombineView>();
		_view.Setup(this.transform);

		GameObject itemCellPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CommonUIPrefabPath.ITEMCELL_BASE) as GameObject;
		_combineItemSlotList = new List<BaseItemCellController>(4);
		_combineIndexList = new List<int>(4);
		for(int i=0;i<5;++i){
			GameObject itemCell;
			if(i<4){
				itemCell = NGUITools.AddChild(_view.combineItemGrid.gameObject,itemCellPrefab);
				BaseItemCellController com = itemCell.GetMissingComponent<BaseItemCellController>();
				com.InitItem(i,OnSelectCombineSlot);
				_combineItemSlotList.Add(com);
			}
			else{
				itemCell = NGUITools.AddChild(_view.resultItemAnchor,itemCellPrefab);
				_resultItemSlot = itemCell.GetMissingComponent<BaseItemCellController>();
				_resultItemSlot.InitItem(0,OnSelectResultSlot);
			}
		}


		_packEqItemDtoList = BackpackModel.Instance.GetPetEquipmentList();
		int itemCellCount = (_packEqItemDtoList.Count%2 == 0)?_packEqItemDtoList.Count:_packEqItemDtoList.Count+1;
		itemCellCount = Mathf.Max(8,itemCellCount);
		_packItemSlotList = new List<BaseItemCellController>(itemCellCount);
		for(int i=0;i<itemCellCount;++i){
			GameObject itemCell = NGUITools.AddChild(_view.eqItemPageGrid.gameObject,itemCellPrefab);
			itemCell.GetMissingComponent<UIDragScrollView>();
			BaseItemCellController com = itemCell.GetMissingComponent<BaseItemCellController>();
			com.InitItem(i,OnSelectPackItemSlot);
//			com.SetIconLblAnchor(UIWidget.Pivot.Right);
			_packItemSlotList.Add(com);
			if(i<_packEqItemDtoList.Count){
				com.SetIcon(_packEqItemDtoList[i].item.icon);
				com.SetIconLbl(_packEqItemDtoList[i].count.ToString());
			}
		}

		RegisterEvent();
	}
	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.closeBtn.onClick,OnClickCloseBtn);
		EventDelegate.Set(_view.combineBtn.onClick,OnClickCombineBtn);
		EventDelegate.Set(_view.combineHintBtn.onClick,OnClickCombineHintBtn);

		BackpackModel.Instance.OnUpdateItem += UpdatePropsItemInfo;
		BackpackModel.Instance.OnDeleteItem += RemovePropsItem;
	}
	public void Dispose ()
	{
		BackpackModel.Instance.OnUpdateItem -= UpdatePropsItemInfo;
		BackpackModel.Instance.OnDeleteItem -= RemovePropsItem;
	}
	#endregion

	private void UpdatePropsItemInfo (PackItemDto itemDto){
		for(int i=0;i<_packEqItemDtoList.Count;++i){
			if(_packEqItemDtoList[i].index == itemDto.index){
				_packEqItemDtoList[i]=itemDto;
				_packItemSlotList[i].SetIcon(itemDto.item.icon);
				_packItemSlotList[i].SetIconLbl(itemDto.count.ToString());
				return;
			}
		}

		if(itemDto.item.itemType == H1Item.ItemTypeEnum_PetEquipment){
			_packEqItemDtoList.Add(itemDto);
			UpdatePropsItemList();
		}
	}
	
	private void RemovePropsItem(int itemIndex){
		int removeIndex = -1;
		for(int i=0;i<_packEqItemDtoList.Count;++i){
			if(_packEqItemDtoList[i].index == itemIndex){
				removeIndex = i;
				break;
			}
		}
		
		if(removeIndex != -1){
			_packEqItemDtoList.RemoveAt(removeIndex);
			UpdatePropsItemList();
		}
	}

	private void UpdatePropsItemList(){
		for(int i=0;i<_packItemSlotList.Count;++i){
			_packItemSlotList[i].ResetItem();
			if(i<_packEqItemDtoList.Count){
				_packItemSlotList[i].SetIcon(_packEqItemDtoList[i].item.icon);
				_packItemSlotList[i].SetIconLbl(_packEqItemDtoList[i].count.ToString());
			}
		}
	}
	
	private int _lastSelectPackItemSlotIndex = -1;
	private void OnSelectPackItemSlot(int index){
		if(index < _packEqItemDtoList.Count){
			if(_lastSelectPackItemSlotIndex != index){
				ProxyItemTipsModule.Open(_packEqItemDtoList[index],_packItemSlotList[index].gameObject,false,null);
				_lastSelectPackItemSlotIndex = index;
			}else{
				_lastSelectPackItemSlotIndex = -1;
				AddToCombineList(index);
			}
		}else
			_lastSelectPackItemSlotIndex = -1;
	}

	private void AddToCombineList(int packSlotIndex){
		if(_combineIndexList.Count < 4){
			if(!_combineIndexList.Contains(packSlotIndex) && ValidateCombineItemQuality(_packEqItemDtoList[packSlotIndex])){
				_packItemSlotList[packSlotIndex].SetSelected(true);
				_combineIndexList.Add(packSlotIndex);
				UpdateCombineSlotList();

				if(_resultItemDto != null)
				{
					_resultItemDto = null;
					_resultItemSlot.ResetItem();
				}
			}
		}else
			TipManager.AddTip("材料槽已满");
	}

	//验证材料槽中的装备等级与部位是否一致
	private bool ValidateCombineItemQuality(PackItemDto newItemDto){
		if(_combineIndexList.Count > 0){
			PackItemDto firstItemDto = _packEqItemDtoList[_combineIndexList[0]];
			PetEquipment firstPetEq = firstItemDto.item as PetEquipment;
			PetEquipment newPetEq = newItemDto.item as PetEquipment;
			if(firstPetEq.id != newPetEq.id
			   || firstPetEq.petEquipPartType != newPetEq.petEquipPartType
			   || firstPetEq.wearableLevel != newPetEq.wearableLevel){
				TipManager.AddTip("参与合成的所有装备需要等级和品质都相同");
				return false;
			}
			return true;
		}
		else
			return true;
	}

	private int _lastSelectCombineItemSlotIndex =-1;
	private void OnSelectCombineSlot(int index){
		if(index < _combineIndexList.Count){
			if(_lastSelectCombineItemSlotIndex != index){
				PackItemDto itemDto = _packEqItemDtoList[_combineIndexList[index]];
				ProxyItemTipsModule.Open(itemDto,_combineItemSlotList[index].gameObject,false,null);
				_lastSelectCombineItemSlotIndex = index;
			}else{
				_lastSelectCombineItemSlotIndex = -1;
				RemoveFromCombineList(index);
			}
		}else
			_lastSelectCombineItemSlotIndex = -1;
	}

	private void RemoveFromCombineList(int combineSlotIndex){
		if(combineSlotIndex < _combineIndexList.Count){
			int packItemIndex = _combineIndexList[combineSlotIndex];
			_packItemSlotList[packItemIndex].SetSelected(false);

			_combineIndexList.RemoveAt(combineSlotIndex);
			UpdateCombineSlotList();
		}
	}

	private void UpdateCombineSlotList(){
		for(int i=0;i<_combineItemSlotList.Count;++i){
			if(i<_combineIndexList.Count){
				PackItemDto itemDto = _packEqItemDtoList[_combineIndexList[i]];
				_combineItemSlotList[i].SetIcon(itemDto.item.icon);
			}else
				_combineItemSlotList[i].ResetItem();
		}
	}

	private void OnSelectResultSlot(int index){
		if(_resultItemDto != null){
			ProxyItemTipsModule.Open(_resultItemDto,_resultItemSlot.gameObject,false,null);
		}
	}

	private void OnClickCombineBtn(){
		if(ValidateCombineItemCount()){
			string[] combineItemUIDArray = new string[_combineIndexList.Count];
			int[] packItemIndexs = new int[_combineIndexList.Count];
			for(int i=0;i<_combineIndexList.Count;++i){
				PackItemDto itemDto = _packEqItemDtoList[_combineIndexList[i]];
				combineItemUIDArray[i] = itemDto.uniqueId.ToString();
				packItemIndexs[i] = itemDto.index;
			}

			string petEquipmentUnqiueIdsStr = string.Join(",",combineItemUIDArray);
			ServiceRequestAction.requestServer(EquipmentService.petEquipmentCombine(petEquipmentUnqiueIdsStr),"CombinePetEquipment",(e)=>{
				_resultItemDto = e as PackItemDto;
				//客户端主动删除合成材料
				for(int i=0;i<packItemIndexs.Length;++i){
					BackpackModel.Instance.DeleteItem(packItemIndexs[i]);
				}

				BackpackModel.Instance.UpdateItem(_resultItemDto,false);
				TipManager.AddTip(string.Format("合成成功，你获得了一个全新的{0}",_resultItemDto.item.name));
				_resultItemSlot.SetIcon(_resultItemDto.item.icon);

				_combineIndexList.Clear();
				UpdateCombineSlotList();
			});
		}
	}

	private bool ValidateCombineItemCount(){
		if(_combineIndexList.Count >0){
			PackItemDto firstItemDto = _packEqItemDtoList[_combineIndexList[0]];
			PetEquipment firstPetEq = firstItemDto.item as PetEquipment;
			int partType = firstPetEq.petEquipPartType;
			if(partType == PetEquipment.PetEquipPartType_Amulet){
				if(_combineIndexList.Count == 2 || _combineIndexList.Count == 4){
					if(_combineIndexList.Count == 4){
						for(int i=0;i<_combineIndexList.Count;++i){
							PetEquipmentExtraDto petEqExtraDto =  _packEqItemDtoList[_combineIndexList[i]].extra as PetEquipmentExtraDto;
							if(petEqExtraDto.petSkillIds.Count < 2){
								TipManager.AddTip("生成双技能护符，需要4个护符都是双技能");
								return false;
							}
						}
						return true;
					}else
						return true;
				}else{
					TipManager.AddTip("护符合成需要数量为2个或者4个");
					return false;
				}
			}else{
				if(_combineIndexList.Count != 2){
					TipManager.AddTip("每次合成只需要2件同等级同部位的宠物装备");
					return false;
				}else
					return true;
			}
		}else{
			TipManager.AddTip("合成材料槽为空");
			return false;
		}
	}

	private void OnClickCombineHintBtn(){
		string hint = "1、两件同等级同种类的宠物装备可合成一件全新的宠物装备\n2、合成产物与材料的种类和等级相同\n3、放入4件双技能护符时，可必然合成一件双技能的护符";
		GameHintManager.Open(_view.combineHintBtn.gameObject,hint);
	}
	
	private void OnClickCloseBtn(){
		ProxyPetPropertyModule.CloseEqCombineView();
	}
}
