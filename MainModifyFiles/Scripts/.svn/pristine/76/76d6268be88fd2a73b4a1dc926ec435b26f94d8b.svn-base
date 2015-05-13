using UnityEngine;
using System.Collections;
using com.nucleus.player.msg;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules;
using System.Collections.Generic;

public class FormationItemUseLogic : IPropUseLogic {

	public bool usePropByPos (PackItemDto packItem)
	{
		Props props = packItem.item as Props;
		PropsParam_8 propsParam = props.propsParam as PropsParam_8;

		if(PlayerModel.Instance.GetAcquiredFormationIdList().Contains(propsParam.formationId))
		{
			TipManager.AddTip("已学会该阵法");
			return false;
		}

		int defaultCapacity = DataHelper.GetStaticConfigValue(H1StaticConfigs.FORMATION_DEFAULT_CAPACITY,4);
		int curFormationCount = PlayerModel.Instance.GetAcquiredFormationIdList().Count;
		if(curFormationCount < defaultCapacity ){
			ProxyWindowModule.OpenConfirmWindow(string.Format("确定使用八阵图，学习上面记录的{0}吗？\n（使用后八阵图将会消失）",propsParam.formation.name),"",()=>{
				PlayerModel.Instance.LearnNewFormation(packItem,propsParam.formation,true);
			});
		}else{
			string content = string.Format("你当前最多可掌握{0}种阵法，现在学习{1}会随机覆盖已掌握的阵法，你确定要学习这个阵法吗？",
			                               Mathf.Max(curFormationCount,defaultCapacity),
			                               propsParam.formation.name.WrapColor(ColorConstant.Color_Tip_LostCurrency_Str));
			int ingotCount = DataHelper.GetStaticConfigValue(H1StaticConfigs.FORMATION_OVER_DEFAULT_CAPACITY,200);
			List<string> optionStrList = new List<string>(3);
			optionStrList.Add("继续学习");
			optionStrList.Add(string.Format("使用{0}{1}开阵格并学习",ingotCount,ItemIconConst.Ingot));
			optionStrList.Add("还是算了");
			ProxyWorldMapModule.OpenCommonDialogue("学习八阵图",content,optionStrList,(selectIndex)=>{
				if(selectIndex == 0)
					PlayerModel.Instance.LearnNewFormation(packItem,propsParam.formation,true);
				else if(selectIndex == 1){
					if(PlayerModel.Instance.isEnoughIngot(ingotCount,true))
						PlayerModel.Instance.LearnNewFormation(packItem,propsParam.formation,false);
				}
			},300);
		}

		return false;
	}
}
