using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules.charactor.data;

public class PetExchangeDialogueLogic : MonoBehaviour
{
	public static void Open (int param)
	{
		if (param == 1) {
			//兑换源生之灵
			PackItemDto itemDto = BackpackModel.Instance.getPetExchangeProps();
			if (itemDto != null) {
				if(PetModel.Instance.isFullPet)
				{
					TipManager.AddTip ("你的宠物栏已满，请整理后再来");
					return;
				}

				ServiceRequestAction.requestServer(PetService.exchange(itemDto.index),"exchangePet",(e)=>{
					Props props = itemDto.item as Props;
					PropsParam_15 propParam = props.propsParam as PropsParam_15;
					Pet pet = DataCache.getDtoByCls<GeneralCharactor>(propParam.petId) as Pet;
					TipManager.AddTip(string.Format("恭喜！你获得了一个{0}",pet.name.WrapColor(ColorConstant.Color_Tip_Item_Str)));
				});
			} else {
				TipManager.AddTip ("你没有源生之灵，无法兑换");
				return;
			}
		} else if (param == 2) {
			//兑换珍兽
			ProxyTipsModule.Open("兑换珍兽",
			                     DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_PRECIOUS_PROP_ID,10022),
			                     DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_COUNT,80),
			                     (list)=>{
				if(PetModel.Instance.isFullPet)
				{
					TipManager.AddTip ("你的宠物栏已满，请整理后再来");
					return;
				}

				ServiceRequestAction.requestServer(PetService.exchangePrecious(),"exchangePreciousPet",(e)=>{
					PetCharactorDto petDto = e as PetCharactorDto;
					if(petDto != null){
						TipManager.AddTip(string.Format("恭喜！你获得了一个{0}",petDto.name.WrapColor(ColorConstant.Color_Tip_Item_Str)));
						PetModel.Instance.AddPet(petDto);
					}
				});
			});
		} else if (param == 3) {
			//兑换神兽
			ProxyTipsModule.Open("兑换神兽",
			                     DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_MYTH_PROP_ID,10023),
			                     DataHelper.GetStaticConfigValue(H1StaticConfigs.PET_EXCHANGE_COUNT,80),
			                     (list)=>{
				if(PetModel.Instance.isFullPet)
				{
					TipManager.AddTip ("你的宠物栏已满，请整理后再来");
					return;
				}
				
				ServiceRequestAction.requestServer(PetService.exchangeMyth(),"exchangeMythPet",(e)=>{
					PetCharactorDto petDto = e as PetCharactorDto;
					if(petDto != null){
						TipManager.AddTip(string.Format("恭喜！你获得了一个{0}",petDto.name.WrapColor(ColorConstant.Color_Tip_Item_Str)));
						PetModel.Instance.AddPet(petDto);
					}
				});
			});
		}
	}
}
