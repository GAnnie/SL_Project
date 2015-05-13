// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AssistSkillRefineWinUIController.cs
// Author   : willson
// Created  : 2015/4/8 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.assistskill.data;

using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using UnityEngine;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.dto;

public class AssistSkillRefineWinUIController : MonoBehaviourBase,IViewController
{
	private const string ItemCellName = "Prefabs/Module/BackpackModule/ItemCell";

	private AssistSkillRefineWinUI _view;
	private List<TabBtnController> _tabBtnList;

	private AssistSkillDto _dto;

	private AssistSkillRefineConfig _config;

	private int _vigourConsume;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AssistSkillRefineWinUI>();
		_view.Setup(this.transform);

		RegisterEvent();

		InitTabBtn();
	}
	
	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);
		
		UIHelper.CreateBaseBtn(_view.OptBtnPos,"炼制",OnOptBtn);
		EventDelegate.Set(_view.OptBtn.onClick,OnOptBtn);
        EventDelegate.Set(_view.TipsBtn.onClick, OnClickTips);

		PlayerModel.Instance.OnSubWealthChanged += OnSubWealthChanged;
	}

	private void InitTabBtn()
	{
		_tabBtnList = new List<TabBtnController> (2);
		GameObject tabBtnPrefab = ResourcePoolManager.Instance.SpawnUIPrefab (CommonUIPrefabPath.TABBUTTON_V1) as GameObject;
		for (int i=0; i<2; ++i) 
		{
			GameObject item = NGUITools.AddChild(_view.TabGroup.gameObject, tabBtnPrefab);
			TabBtnController com = item.GetMissingComponent<TabBtnController> ();
			_tabBtnList.Add(com);
			_tabBtnList[i].InitItem (i,OnSelectTabBtn);
		}
		
		_tabBtnList[0].SetBtnName ("炼药");
		_tabBtnList[1].SetBtnName ("药品");
	}

	public void OnSelectTabBtn (int index)
	{
		if (index == 0)
			ShowRefineView();
		else if (index == 1)
			ShowPreviewView();
	}

	private void UpdateTabBtnState (int selectIndex)
	{
		for (int i=0; i<_tabBtnList.Count; ++i)
		{
			if (i != selectIndex)
				_tabBtnList[i].SetSelected(false);
			else
				_tabBtnList[i].SetSelected(true);
		}
	}

    private void OnClickTips()
    {
        GameHintManager.Open(_view.TipsBtn.gameObject, "1、炼药材料为草药，各地药店均有出售\n2、炼药成功率受材料和技能等级影响\n3、所得丹药的品质受技能等级影响\n4、没有材料时，可消耗铜币进行炼药");
    }

	public void SetData(AssistSkillDto dto)
	{
		_dto = dto;
		OnSelectTabBtn(0);
	}

	#region 药品制作
	private List<ItemCellController> _materialItems;
	private ItemCellController _productItemItems;

	private List<ItemCellController> _packItems;

	private void InitRefineView()
	{
		_view.RefineGroup.SetActive(true);
		_view.PreviewGroup.SetActive(false);

		if(_config == null)
			_config = DataCache.getDtoByCls<AssistSkillRefineConfig>(1);

		if(_productItemItems == null)
		{
			_productItemItems = UIHelper.CreateItemCell(_view.ProductsItemPos);
			_productItemItems.CanDisplayCount(false);
		}

		if(_materialItems == null)
		{
			_materialItems = new List<ItemCellController>(4);
			for(int index = 0;index < 4;index++)
			{
				ItemCellController cell = UIHelper.CreateItemCell(_view.MaterialsGrid.gameObject);
				cell.CanDisplayCount(false);
				_materialItems.Add(cell);
			}
			_view.MaterialsGrid.Reposition();
		}

		if(_packItems == null)
		{
			_packItems = new List<ItemCellController>(8);
			for(int index = 0;index < 8;index++)
			{
				ItemCellController cell = UIHelper.CreateItemCell(_view.ItemGrid.gameObject);
				cell.CanDisplayCount(true);
				_packItems.Add(cell);
			}
			_view.ItemGrid.Reposition();
		}

		bool isEmtpy = IsMaterialEmtpy();
		_view.OptBtnPos.SetActive(!isEmtpy);
		_view.OptBtn.gameObject.SetActive(isEmtpy);
	}

	private bool IsMaterialEmtpy()
	{
		for(int index = 0;index < _materialItems.Count;index++)
		{
			if(_materialItems[index].GetData() != null)
				return false;
		}
		return true;
	}

	private int GetMaterialCount()
	{
		int count = 0;
		for(int index = 0;index < _materialItems.Count;index++)
		{
			if(_materialItems[index].GetData() != null)
				count++;
		}
		return count;
	}

	private void ClearMaterial()
	{
		for(int index = 0;index < _materialItems.Count;index++)
		{
			if(_materialItems[index].GetData() != null)
				_materialItems[index].SetData(null,null);
		}
	}

	private void ShowRefineView()
	{
		UpdateTabBtnState(0);
		InitRefineView();

		_vigourConsume = LuaManager.Instance.DoVigourConsumeFormula(_dto,_dto.level);
		_view.VigourValLbl.text = PlayerModel.Instance.Vigour + "/" + _vigourConsume;

		// 显示背包材料
		List<PackItemDto> items = BackpackModel.Instance.GetItemsByItemIds(_config.materialPropIds);
		int index = 0;
		for(;index < items.Count;index++)
		{
			if(index < _packItems.Count)
			{
				_packItems[index].SetData(items[index],OnPackItem);
			}
		}

		while(index < _packItems.Count)
		{
			_packItems[index].SetData(null);
			index++;
		}
	}

	private void OnSubWealthChanged(SubWealthNotify notify)
	{
		_view.VigourValLbl.text = PlayerModel.Instance.Vigour + "/" + _vigourConsume;
	}

	private void OnPackItem(ItemCellController cell)
	{
		if(GetMaterialCount() >= 4)
		{
			TipManager.AddTip("材料已满");
			return;
		}

		for(int index = 0;index < _materialItems.Count;index++)
		{
			if(_materialItems[index].GetData() == null)
			{
				if(cell.ItemCount != 1)
				{
					_materialItems[index].SetData(ItemHelper.GetOneInPackItemDto(cell.GetData()),OnMaterialItem);
					cell.ItemCount = cell.ItemCount - 1;
				}
				else
				{
					_materialItems[index].SetData(cell.GetData(),OnMaterialItem);
					cell.SetData(null,null);
				}

				break;
			}
		}

		bool isEmtpy = IsMaterialEmtpy();
		_view.OptBtnPos.SetActive(!isEmtpy);
		_view.OptBtn.gameObject.SetActive(isEmtpy);
	}

	private void OnMaterialItem(ItemCellController cell)
	{
		PackItemDto itemDto = cell.GetData();
		ItemCellController emtpyCell = null;
		bool isFind = false;
		for(int index = 0;index < _packItems.Count;index++)
		{
			if(_packItems[index].GetData() == null && emtpyCell == null)
			{
				emtpyCell = _packItems[index];
			}

			if(_packItems[index].GetData() != null && itemDto.index == _packItems[index].GetData().index)
			{
				_packItems[index].ItemCount = _packItems[index].ItemCount + 1;
				isFind = true;
				break;
			}
		}

		if(!isFind && emtpyCell != null)
		{
			emtpyCell.SetData(cell.GetData(),OnPackItem);
		}

		cell.SetData(null,null);

		bool isEmtpy = IsMaterialEmtpy();
		_view.OptBtnPos.SetActive(!isEmtpy);
		_view.OptBtn.gameObject.SetActive(isEmtpy);
	}

	private void OnOptBtn()
	{
		string material = "";
		if(IsMaterialEmtpy() == false)
		{
			int count= 0;
			Dictionary<int, int> materialMap = new Dictionary<int, int>();
			// @parm material 索引:数量,索引:数量,索引:数量...
			for(int index = 0;index < _materialItems.Count;index++)
			{
				if(_materialItems[index].GetData() != null)
				{
					count ++;
					if(materialMap.ContainsKey(_materialItems[index].GetData().index))
						materialMap[_materialItems[index].GetData().index] += 1;
					else
						materialMap[_materialItems[index].GetData().index] = 1;
				}
			}

			if(count == 1)
			{
				TipManager.AddTip("至少需要放入2个草药才能进行炼药");
				return;
			}

			foreach (int index in materialMap.Keys) 
			{
				material += string.Format("{0}:{1},",index,materialMap[index]);
			}

			if(!string.IsNullOrEmpty(material))
				material = material.Substring(0,material.Length - 1);
		}
		else
		{
			if(!PlayerModel.Instance.isEnoughCopper(_config.copperConsume,true))
			{
				return;
			}
		}

		if(PlayerModel.Instance.isEnoughVigour(_vigourConsume,true))
		{
			ServiceRequestAction.requestServer(AssistSkillService.refine(material),"refine",(e)=>{
				AssistSkillProductResultDto dto = e as AssistSkillProductResultDto;
				if(dto != null)
				{
					TipManager.AddTip("获得"+dto.item.item.name);
					ClearMaterial();
					_productItemItems.SetData(ItemHelper.ItemIdToPackItemDto(dto.item.itemId),OnClickProductItem);
				}
			});
		}
	}
	#endregion

	#region 药品预览
	private List<ItemCellController> _previewItems;

	private void ShowPreviewView()
	{
		UpdateTabBtnState(1);
		_view.RefineGroup.SetActive(false);
		_view.PreviewGroup.SetActive(true);

		if(_previewItems == null)
		{
			_previewItems = new List<ItemCellController>();
			List<int> previewItemIds = new List<int>();
			previewItemIds.AddRange(_config.materialPropIds);
			previewItemIds.Add(_config.mainPropId);
			previewItemIds.AddRange(_config.vicePropIds);

			int maxCout = 20;
			int index = 0;

			for(;index < previewItemIds.Count;index++)
			{
				if(index < maxCout)
				{
					ItemCellController cell = UIHelper.CreateItemCell(_view.PreviewGrid.gameObject);
					cell.CanDisplayCount(false);
					cell.SetData(ItemHelper.ItemIdToPackItemDto(previewItemIds[index]),OnClickProductItem);
					_previewItems.Add(cell);
				}
			}

			while(index < maxCout)
			{
				ItemCellController cell = UIHelper.CreateItemCell(_view.PreviewGrid.gameObject);
				cell.CanDisplayCount(false);
				_previewItems.Add(cell);
				index++;
			}
			_view.PreviewGrid.Reposition();
		}
	}
	#endregion
	
	private void OnClickProductItem(ItemCellController cell)
	{
		ProxyItemTipsModule.Open(cell.GetData(),cell.gameObject,false);
	}

	private void OnCloseBtn()
	{
		ProxySkillModule.CloseAssistSkillRefineWin();
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnSubWealthChanged -= OnSubWealthChanged;
	}
}