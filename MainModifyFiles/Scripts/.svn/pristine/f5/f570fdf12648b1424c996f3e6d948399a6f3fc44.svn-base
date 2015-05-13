using UnityEngine;
using System.Collections;
using com.nucleus.player.msg;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.model;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.services;

public class PlayerResetApItemLogicDef : IPropUseLogic {
	
	public bool usePropByPos (PackItemDto packItem){
		OpenDialogueView(packItem,null);
		return false;
	}

	public void OpenDialogueView(PackItemDto packItem,System.Action onSuccess){
		PlayerPropertyInfo playerInfo = PlayerModel.Instance.GetPlayerPropertyInfo();
		int resetMinLevel = DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIN_CHARACTOR_DISPOSABLE_POTENTIAL_MIN_LEVEL, 40);
		if(playerInfo.playerDto.level < resetMinLevel){
			TipManager.AddTip(string.Format("洗点需要人物等级达到{0}级",resetMinLevel));
			return;
		}

		int[] playerApVals = playerInfo.ToApInfoArray ();
		
		int minApVal = playerInfo.playerDto.level + DataHelper.GetStaticConfigValue (H1StaticConfigs.MIN_APTITUDE_RESET_POINT, 10);
		List<int> _canResetPointList = new List<int> (5);
		string[] optionTitleList = new string[5]{"体质","魔力","力量","耐力","敏捷"};
		List<string> optionStrList = new List<string> (5);
		List<int> optionApTypeList = new List<int>(5);
		for (int i=0; i<playerApVals.Length; ++i) {
			int canResetPoint = playerApVals [i] - minApVal;
			if (canResetPoint > 0) {
				_canResetPointList.Add (canResetPoint);
				optionApTypeList.Add(i+1);
				optionStrList.Add (string.Format ("{0}(可洗点数{1})", optionTitleList [i], canResetPoint));
			}
		}
		
		if (_canResetPointList.Count > 0) {
			Props propsInfo = DataCache.getDtoByCls<GeneralItem> (DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIN_CHARACTOR_RESET_APTITUDE_ITEM_ID, 10003)) as Props;
			int propCount = BackpackModel.Instance.GetItemCount (propsInfo.id);
			
			if (playerInfo.playerDto.level >= resetMinLevel
			    && !PlayerModel.Instance.HasCustomAptitude ()) {
				optionStrList.Add ("重置所有属性点(免费)");
			}
			
			ProxyWorldMapModule.OpenCommonDialogue ("角色洗点", string.Format ("当前你拥有人物洗点丹{0}个，并且人物达到{1}级可获得1次免费重置所有属性点的机会，请选择你要进行的操作：", propCount, resetMinLevel),
			                                        optionStrList, (selectIndex) => {
				if (selectIndex < _canResetPointList.Count) {
					int apType = optionApTypeList[selectIndex]; //选中洗点资质类型
					int resetPoint = Mathf.Min (_canResetPointList [selectIndex], DataHelper.GetStaticConfigValue (H1StaticConfigs.MAIN_CHARACTOR_RESET_PER_APTITUDE, 2));
					if (propCount > 0) {
						int packItemIndex = packItem == null ? BackpackModel.Instance.GetItemByItemId(propsInfo.id).index : packItem.index;
						if(resetPoint == 1){
							string hint = string.Format("该属性只有{0}点属性点可转化，是否继续",resetPoint.ToString().WrapColor(ColorConstant.Color_Tip_LostCurrency_Str));
							ProxyWindowModule.OpenConfirmWindow(hint,"",()=>{
								PlayerModel.Instance.ResetPlayerPerAp(packItemIndex,apType,resetPoint,false,onSuccess);
							});
						}else
							PlayerModel.Instance.ResetPlayerPerAp(packItemIndex,apType,resetPoint,false,onSuccess);
					} else {
						ProxyWindowModule.OpenConfirmWindow (string.Format ("你已经没有角色洗点丹了，可以使用{0}{1}购买，确认购买并且继续吗？", propsInfo.buyPrice,ItemIconConst.Ingot), "", () => {
							if(PlayerModel.Instance.isEnoughIngot(propsInfo.buyPrice,true))
								PlayerModel.Instance.ResetPlayerPerAp(-1,apType,resetPoint,true,onSuccess);
						});
					}
				} else {
					//重置资质属性点
					ServiceRequestAction.requestServer (PlayerService.resetAptitude (), "重置人物资质属性", (e) => {
						PlayerModel.Instance.RestPlayerApPoint ();
						if(onSuccess != null)
							onSuccess();
					});
				}
			}, 320);
		} else
			TipManager.AddTip ("当前没有可以洗点的属性");
	}
}
