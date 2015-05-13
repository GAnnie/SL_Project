// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  EquipmentModel.cs
// Author   : willson
// Created  : 2015/2/3 
// Porpuse  : 
// **********************************************************************
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.equipment.data;
using System;
using com.nucleus.player.data;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.equipment.dto;
using com.nucleus.player.msg;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules;

public class EquipmentModel
{
	private static readonly EquipmentModel _instance = new EquipmentModel ();
	
	public static EquipmentModel Instance {
		get {
			return _instance;
		}
	}

	private EquipmentModel()
	{
		_smithCostDic = new Dictionary<int, EquipmentSmithCostDto>();
		_gemCostDic = new Dictionary<int, int>();
		_refreshAptitudeCostDic = new Dictionary<long, int>();
	}

	public static bool CanWearEquip(PackItemDto dto)
	{
		if(dto != null && dto.item != null && dto.item.itemType == H1Item.ItemTypeEnum_Equipment)
		{
			Equipment equip = dto.item as Equipment;
			EquipmentExtraDto extraDto = dto.extra as EquipmentExtraDto;
			// 1.判断性别 // 2.判断门派 // 3.判断是已经鉴定
			return (equip.mainCharactorId == 0 || equip.mainCharactorId == PlayerModel.Instance.GetPlayer().charactorId) 
				&& (equip.equipSexType == PlayerDto.Gender_Unknown || equip.equipSexType == PlayerModel.Instance.GetPlayerGender())
				&& (extraDto != null && extraDto.hasIdentified);
		}
		return false;
	}

	#region 装备打造相关

	private Dictionary< int ,List<Equipment> > _equipLvDic;
	private Dictionary<int,EquipmentSmithCostDto> _smithCostDic;

	public void SetupSmith()
	{
		if(_equipLvDic == null)
		{
			_equipLvDic = new Dictionary<int, List<Equipment>>();
			List<GeneralItem> list = DataCache.getArrayByClsWithoutSort<GeneralItem>();

			for(int index = 0;index < list.Count;index++)
			{
				Equipment equip = list[index] as Equipment;
				if(equip == null)
					continue;

				if(equip.smithable)
				{
					if(equip.mainCharactorId == 0 || equip.mainCharactorId == PlayerModel.Instance.GetPlayer().charactorId)
					{
						if(equip.equipSexType == PlayerDto.Gender_Unknown || equip.equipSexType == PlayerModel.Instance.GetPlayerGender())
						{
							if(_equipLvDic.ContainsKey(equip.equipLevel))
							{
								_equipLvDic[equip.equipLevel].Add(equip);
							}
							else
							{
								List<Equipment> equipLvList = new List<Equipment>();
								equipLvList.Add(equip);
								_equipLvDic[equip.equipLevel] = equipLvList;
							}
						}
					}
				}
			}
		}
	}

	public List<int> GetEquipLvRange()
	{
		List<int> lvRange = new List<int>();
		int maxLv = Math.Min(PlayerModel.Instance.GetPlayerLevel() + 10,PlayerModel.Instance.ServerGrade + 5);

		Dictionary<int ,List<Equipment>>.KeyCollection keys = _equipLvDic.Keys;
		foreach(int lv in keys)
		{
			if(lv <= maxLv)
			{
				lvRange.Add(lv);
			}
		}
		return lvRange;
	}

	public List<Equipment> GetEquipLvList(int lv)
	{
		if(_equipLvDic.ContainsKey(lv))
			return _equipLvDic[lv];

		Dictionary<int ,List<Equipment>>.KeyCollection keys = _equipLvDic.Keys;

		int maxLv = 0;
		int minLv = 99999;

		foreach(int k in keys)
		{
			if(k > maxLv)
			{
				maxLv = k;
			}

			if(k < minLv)
			{
				minLv = k;
			}
		}

		if(minLv > lv)
		{
			if(_equipLvDic.ContainsKey(minLv))
			   return _equipLvDic[minLv];
		}

		if(lv > maxLv)
		{
			if(_equipLvDic.ContainsKey(maxLv))
			   return _equipLvDic[maxLv];
		}

		return null;
	}

	public void GetSmithCost(int equipmentId,System.Action<EquipmentSmithCostDto> onSuccess)
	{
		if(_smithCostDic.ContainsKey(equipmentId))
		{
			if(onSuccess != null)
				onSuccess(_smithCostDic[equipmentId]);
		}
		else
		{
			ServiceRequestAction.requestServer(EquipmentService.smithCost(equipmentId),"smithCost", 
			(e) => {
				EquipmentSmithCostDto dto = e as EquipmentSmithCostDto;
				_smithCostDic[dto.equipmentId] = dto;

				if(onSuccess != null)
					onSuccess(dto);
			});
		}
	}

	public void SmithCostCacheClear()
	{
		_smithCostDic.Clear();
	}

	public void SmithEquip(int equipmentId,int previousIngotCost,bool ifFastSmith,System.Action<GeneralResponse> OnSmithCallBack)
	{
		ServiceRequestAction.requestServer(EquipmentService.smith(equipmentId,previousIngotCost,ifFastSmith),"smith", 
		(e) => {
			if(e is PackItemDto)
			{
				TipManager.AddTip("装备打造成功");
				PackItemDto dto = e as PackItemDto;
				BackpackModel.Instance.UpdateItem(dto);
			}
			else if(e is EquipmentSmithCostDto)
			{
				TipManager.AddTip("元宝打造价格发生变动");
				EquipmentSmithCostDto dto = e as EquipmentSmithCostDto;
				_smithCostDic[dto.equipmentId] = dto;
			}

			if(OnSmithCallBack != null)
				OnSmithCallBack(e);
		});
	}

	public void IdentifyEquip(long equipmentUniqueId,int previousIdentifyCost,System.Action<GeneralResponse> OnIdentifyCallBack)
	{
		ServiceRequestAction.requestServer(EquipmentService.identify(equipmentUniqueId,previousIdentifyCost),"identify", 
		(e) => {
			if(e is PackItemDto)
			{
				TipManager.AddTip("装备鉴定成功");
				PackItemDto dto = e as PackItemDto;
				BackpackModel.Instance.UpdateItem(dto);
			}
			else if(e is EquipmentSmithCostDto)
			{
				TipManager.AddTip("元宝鉴定价格发生变动");
				EquipmentSmithCostDto dto = e as EquipmentSmithCostDto;
				_smithCostDic[dto.equipmentId] = dto;
			}
			
			if(OnIdentifyCallBack != null)
				OnIdentifyCallBack(e);
		});
	}

	#endregion

	#region 宝石镶嵌相关

	private List<Props> _gems;

	public void SetupGem()
	{
		if(_gems == null)
		{
			_gems = new List<Props>();

			List<EquipmentPropertyInfo> infos = DataCache.getArrayByClsWithoutSort<EquipmentPropertyInfo>();
			List<int> gemIds = new List<int>();
			for(int index = 0;index < infos.Count;index++)
			{
				EquipmentPropertyInfo info = infos[index] as EquipmentPropertyInfo;
				for(int i = 0;i < info.embeddableItemIds.Count;i++)
				{
					if(gemIds.IndexOf(info.embeddableItemIds[i]) == -1)
					{
						gemIds.Add(info.embeddableItemIds[i]);
					}
				}
			}

			gemIds.Sort();

			for(int index = 0;index < gemIds.Count;index++)
			{
				Props props = DataCache.getDtoByCls<GeneralItem>(gemIds[index]) as Props;
				if(props != null)
					_gems.Add(props);
			}
		}
	}

	public List<Props> GetGems()
	{
		return _gems;
	}

	private Dictionary<int,int> _gemCostDic;

	public void GetGemCost(int gemId,System.Action<int> onSuccess)
	{
		if(_gemCostDic.ContainsKey(gemId))
		{
			if(onSuccess != null)
				onSuccess(_gemCostDic[gemId]);
		}
		else
		{
			ServiceRequestAction.requestServer(EquipmentService.embedItemCosts(),"embedItemCosts", 
			(e) => {
				EquipmentEmbedCostInfo dto = e as EquipmentEmbedCostInfo;
				UpDateGemCostDic(dto);
				if(onSuccess != null)
					onSuccess(_gemCostDic[gemId]);
			});
		}
	}

	private void UpDateGemCostDic(EquipmentEmbedCostInfo dto)
	{
		_gemCostDic.Clear();
		for(int index = 0;index < dto.itemIds.Count;index++)
		{
			if(index < dto.itemCosts.Count)
			{
				_gemCostDic[dto.itemIds[index]] = dto.itemCosts[index];
			}
		}
	}

	public void EmbedGem(PackItemDto itemDto,int embedItemId,int unitPrice,System.Action<GeneralResponse> OnEmbedCallBack)
	{
		ServiceRequestAction.requestServer(EquipmentService.embed(itemDto.uniqueId,embedItemId,unitPrice),"embed", 
		(e) => {
			if(e is EquipmentEmbedInfo)
			{
				TipManager.AddTip("镶嵌宝石成功");
				EquipmentEmbedInfo info = e as EquipmentEmbedInfo;
				(itemDto.extra as EquipmentExtraDto).equipmentEmbedInfo = info;
				BackpackModel.Instance.UpdateItem(itemDto);
			}
			else if(e is EquipmentEmbedCostInfo)
			{
				TipManager.AddTip("元宝镶嵌价格发生变动");
				EquipmentEmbedCostInfo dto = e as EquipmentEmbedCostInfo;
				UpDateGemCostDic(dto);
			}

			if(OnEmbedCallBack != null)
				OnEmbedCallBack(e);
		});	
	}

	public void RomeveGem(PackItemDto itemDto,int embedItemId,System.Action OnRomeveCallBack)
	{
		ServiceRequestAction.requestServer(EquipmentService.unembed(itemDto.uniqueId,embedItemId),"unembed", 
		(e) => {
			TipManager.AddTip("宝石拆除成功");
			EquipmentExtraDto extraDto = itemDto.extra as EquipmentExtraDto;
			if(extraDto.equipmentEmbedInfo != null 
			   && extraDto.equipmentEmbedInfo.embedItemIds != null
			   && extraDto.equipmentEmbedInfo.embedItemIds.Count > 0)
			{
				int index = extraDto.equipmentEmbedInfo.embedItemIds.IndexOf(embedItemId);
				if(index != -1)
				{
					extraDto.equipmentEmbedInfo.embedItemIds.RemoveAt(index);
					if(index < extraDto.equipmentEmbedInfo.embedLevels.Count)
						extraDto.equipmentEmbedInfo.embedLevels.RemoveAt(index);

					BackpackModel.Instance.UpdateItem(itemDto);

					if(OnRomeveCallBack != null)
						OnRomeveCallBack();
				}
			}
		});
	}

	#endregion

	#region 属性刷新相关
	
	private Dictionary<long,int> _refreshAptitudeCostDic;

	public void GetRefreshAptitudeCost(long equipmentUniqueId,System.Action<int> onSuccess)
	{
		if(_refreshAptitudeCostDic.ContainsKey(equipmentUniqueId))
		{
			if(onSuccess != null)
				onSuccess(_refreshAptitudeCostDic[equipmentUniqueId]);
		}
		else
		{
			ServiceRequestAction.requestServer(EquipmentService.refreshAptitudeCost(equipmentUniqueId),"refreshAptitudeCost", 
			(e) => {
				EquipmentRefreshAptitudeCost dto = e as EquipmentRefreshAptitudeCost;
				_refreshAptitudeCostDic[dto.equipmentUniqueId] = dto.ingotCost;

				if(onSuccess != null)
					onSuccess(_refreshAptitudeCostDic[equipmentUniqueId]);
			});
		}
	}

	public void RefreshAptitude(PackItemDto itemDto,int aptitudeSequence,int previousIngotCost,System.Action<GeneralResponse> OnRefreshAptitudeCallBack)
	{
		ServiceRequestAction.requestServer(EquipmentService.refreshAptitude(itemDto.uniqueId,aptitudeSequence,previousIngotCost),"refreshAptitudeCost", 
		(e) => {
			if(e is EquipmentAptitude)
			{
				TipManager.AddTip("转换属性成功");
				EquipmentAptitude aptitude = e as EquipmentAptitude;
				EquipmentExtraDto extraDto = itemDto.extra as EquipmentExtraDto;
				if(extraDto.aptitudeProperties != null 
				   && extraDto.aptitudeProperties.Count > 0)
				{
					if(aptitudeSequence < extraDto.aptitudeProperties.Count)
					{
						extraDto.aptitudeProperties[aptitudeSequence] = aptitude;
					}
					BackpackModel.Instance.UpdateItem(itemDto);
				}
			}
			else if(e is EquipmentRefreshAptitudeCost)
			{
				TipManager.AddTip("转换属性价格发生变动");
				EquipmentRefreshAptitudeCost dto = e as EquipmentRefreshAptitudeCost;
				_refreshAptitudeCostDic[dto.equipmentUniqueId] = dto.ingotCost;
			}

			if(OnRefreshAptitudeCallBack != null)
				OnRefreshAptitudeCallBack(e);
		});
	}

	#endregion

	#region 宝石转移相关

	public void EmbedTransfer(PackItemDto src,PackItemDto target,System.Action OnEmbedTransferCallBack)
	{
		ServiceRequestAction.requestServer(EquipmentService.embedTransfer(src.uniqueId,target.uniqueId),"embedTransfer", 
		(e) => {
			TipManager.AddTip("宝石转移成功");
			EquipmentEmbedTransferDto dto = e as EquipmentEmbedTransferDto;

			(src.extra as EquipmentExtraDto).equipmentEmbedInfo = dto.oldEmbedInfo;
			BackpackModel.Instance.UpdateItem(src);

			(target.extra as EquipmentExtraDto).equipmentEmbedInfo = dto.newEmbedInfo;
			BackpackModel.Instance.UpdateItem(target);

			if(OnEmbedTransferCallBack != null)
				OnEmbedTransferCallBack();
		});
	}

	#endregion

	#region 修理装备
	public void Repair(PackItemDto itemDto)
	{
		ServiceRequestAction.requestServer(EquipmentService.repair(itemDto.uniqueId),"repair", 
		(e) => {
			ResetMaxDuration(itemDto);
			BackpackModel.Instance.UpdateItem(itemDto);
			//检索是否存在损毁的装备
			BackpackModel.Instance.CheckOutBreakdownEquipment();
		});
	}

	public void RepairAll(){

		ServiceRequestAction.requestServer(EquipmentService.repairAll(),"repairAll",(e)=>{
			var eqDic = BackpackModel.Instance.GetBodyEquip();
			foreach(PackItemDto itemDto in eqDic.Values){
				if(ResetMaxDuration(itemDto)){
					BackpackModel.Instance.UpdateItem(itemDto);
				}
			}
			PlayerBuffModel.Instance.ToggleEqBreakdownBuffTip(false);
		});
	}

	private bool ResetMaxDuration(PackItemDto dto)
	{
		Equipment equip = dto.item as Equipment;
		EquipmentExtraDto extraDto = dto.extra as EquipmentExtraDto;
		if(equip != null && extraDto != null)
		{
			int maxDuration = equip.equipLevel*DataHelper.GetStaticConfigValue(H1StaticConfigs.EQUIPMENT_DURATION_FACTOR1,5) + DataHelper.GetStaticConfigValue(H1StaticConfigs.EQUIPMENT_DURATION_FACTOR2,100);
			if(extraDto.duration != maxDuration)
			{
				extraDto.duration = maxDuration;
				return true;
			}
		}

		return false;
	}

	public long GetEquipmentRepairFee(){
		int result = 0;
		var eqDic = BackpackModel.Instance.GetBodyEquip();
		foreach(PackItemDto itemDto in eqDic.Values){
			Equipment equip = itemDto.item as Equipment;
			EquipmentExtraDto extraDto = itemDto.extra as EquipmentExtraDto;
			int maxDuration = equip.equipLevel*DataHelper.GetStaticConfigValue(H1StaticConfigs.EQUIPMENT_DURATION_FACTOR1,5) + DataHelper.GetStaticConfigValue(H1StaticConfigs.EQUIPMENT_DURATION_FACTOR2,100);
			result += (int)(equip.salePrice *(1.0f - (float)extraDto.duration/(float)maxDuration));
		}

		return result;
	}
	#endregion
	public void Destroy()
	{
	}
}