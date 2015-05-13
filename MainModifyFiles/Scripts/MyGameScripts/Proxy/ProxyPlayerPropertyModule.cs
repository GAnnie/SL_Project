using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.player.data;
using com.nucleus.h1.logic.core.modules.player.data;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.h1.logic.core.modules.title.dto;

public class ProxyPlayerPropertyModule{

	private const string BASEINFO_VIEW = "Prefabs/Module/PlayerPropertyModule/PlayerBaseInfoMainView";
	private const string PPDETAIL_VIEW = "Prefabs/Module/PlayerPropertyModule/PPDetailView";
	private const string CHANGENAME_VIEW = "Prefabs/Module/PlayerPropertyModule/PlayerChangeNameView";
	private const string APPELLATION_VIEW = "Prefabs/Module/PlayerPropertyModule/PlayerAppellationView";
	private const string DYE_VIEW = "Prefabs/Module/PlayerPropertyModule/PlayerDyeView";
	
	public static void Open(int defaultRightTab=0){
		PlayerModel.Instance.RequestCharacterDto(()=>{
			OpenView(defaultRightTab);
		});
	}
	
	private static void OpenView(int defaultRightTab){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(BASEINFO_VIEW,UILayerType.DefaultModule,true);
		var controller = ui.GetMissingComponent<PlayerBaseInfoMainViewController>();
		controller.Open(defaultRightTab);
	}

	public static void UpdateHpValue(int currentVal, int maxVal)
	{
//		GameObject ui = UIModuleManager.Instance.GetModule(BASEINFO_VIEW);
//		if (ui != null)
//		{
//			var controller = ui.GetMissingComponent<PlayerBaseInfoMainViewController>();
//			controller.UpdateHpValue(currentVal, maxVal);
//		}
	}
	
	public static void UpdateMpValue(int currentVal, int maxVal)
	{
//		GameObject ui = UIModuleManager.Instance.GetModule(BASEINFO_VIEW);
//		if (ui != null)
//		{
//			var controller = ui.GetMissingComponent<PlayerBaseInfoMainViewController>();
//			controller.UpdateMpValue(currentVal, maxVal);
//		}
	}

	public static void Close(){
		UIModuleManager.Instance.CloseModule(BASEINFO_VIEW);
	}

	public static PPDetailViewController OpenPPDetailView(Vector3 pos){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(PPDETAIL_VIEW,UILayerType.SubModule,false);
		ui.transform.localPosition = pos;
		var controller = ui.GetMissingComponent<PPDetailViewController>();

		return controller;
	}

	public static void ClosePPDetailView(){
		UIModuleManager.Instance.CloseModule(PPDETAIL_VIEW);
	}

	#region 改名面板
	public static void OpenPlayerChangeNameView(bool fromProp)
	{
		int playerLevel = PlayerModel.Instance.GetPlayerLevel();
		Props prop = DataCache.getDtoByCls<GeneralItem>( DataHelper.GetStaticConfigValue(H1StaticConfigs.RENAME_ITEM_ID) ) as Props;

		if (playerLevel < prop.minGrade)
		{
			TipManager.AddTip("10≤等级≤30时可使用改名许可证免费改名");
			return;
		}

		if (fromProp)
		{
			if (playerLevel > prop.maxGrade)
			{
				ServiceRequestAction.requestServer(PlayerService.propsRenameExpired());
				TipManager.AddTip("等级≤30级才能使用改名许可证免费改名");
				return;
			}
		}
		else
		{
			if (BackpackModel.Instance.ContainItemWithItemId ( DataHelper.GetStaticConfigValue(H1StaticConfigs.RENAME_ITEM_ID) ))
			{
				if (playerLevel >= prop.minGrade && playerLevel <= prop.maxGrade)
				{
					fromProp = true;
				}
				else if (playerLevel > prop.maxGrade)
				{
					fromProp = false;
				}
			}
		}

		GameObject ui = UIModuleManager.Instance.OpenFunModule(CHANGENAME_VIEW,UILayerType.SubModule, true);
		var controller = ui.GetMissingComponent<PlayerChangeNameController>();
		controller.InitView ();
		controller.SetData (fromProp);
	}

	public static void ClosePlayerChangeNameView()
	{
		UIModuleManager.Instance.CloseModule(CHANGENAME_VIEW);
	}
	#endregion

	#region 活力兑换
	private const string VIGOUR_VIEW = "Prefabs/Module/PlayerPropertyModule/VigourWinUI";

	public static void OpenVigourWin(int depath = UILayerType.SubModule)
	{
		GameObject view = UIModuleManager.Instance.OpenFunModule(VIGOUR_VIEW,depath,true);
		if(view != null)
		{
			var controller = view.GetMissingComponent<VigourWinUIController>();
			controller.InitView();
			//controller.SetData(dto);
		}
	}
	
	public static void CloseVigourWin()
	{
		UIModuleManager.Instance.CloseModule(VIGOUR_VIEW);
	}
	#endregion

	#region 称谓面板
	public static void OpenPlayerAppellationView(){
		if(PlayerModel.Instance.GetTitleList().Count > 0){
			GameObject ui = UIModuleManager.Instance.OpenFunModule(APPELLATION_VIEW,UILayerType.SubModule,true);
			var controller = ui.GetMissingComponent<PlayerAppellationViewController>();
			controller.InitView();
		}else
			TipManager.AddTip("你当前并没有获得称谓");
	}

	public static void ClosePlayerAppellationView(){
		UIModuleManager.Instance.CloseModule(APPELLATION_VIEW);
	}
	#endregion

	#region 人物染色面板
	public static void SelectDyeOption(int param){
		if(param == 1){
			if(PlayerModel.Instance.IsDyeMode()){
				OpenDyeView();
			}else
				ProxyWindowModule.OpenConfirmWindow("你现在身着便装，无法染色。需要换穿华服才能继续染色（可随时穿回便装状态），是否切换并且继续？","",()=>{
					PlayerModel.Instance.ChangeDyeMode(OpenDyeView);
				});
		}else if(param == 2){
			if(PlayerModel.Instance.IsDyeMode())
				PlayerModel.Instance.ChangeDyeMode();
			else
				TipManager.AddTip("当前已穿着便装无需更换");
		}else if(param == 3){
			if(!PlayerModel.Instance.IsDyeMode())
				PlayerModel.Instance.ChangeDyeMode();
			else
				TipManager.AddTip("当前已穿着华服无需更换");
		}
	}

	private static void OpenDyeView(){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(DYE_VIEW,UILayerType.SubModule,true);
		var controller = ui.GetMissingComponent<PlayerDyeViewController>();
		controller.InitView();
	}
	
	public static void ClosePlayerDyeView(){
		UIModuleManager.Instance.CloseModule(DYE_VIEW);
	}
	#endregion
}
