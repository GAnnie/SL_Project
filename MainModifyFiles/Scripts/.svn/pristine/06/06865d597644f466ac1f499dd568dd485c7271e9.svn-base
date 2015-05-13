using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.player.dto;

public static class ProxyMainUIModule{

	private const string MAINUI_VIEW = "Prefabs/Module/MainUIModule/MainUIView";
	private const string PLAYERINFO_VIEW = "Prefabs/Module/MainUIModule/PlayerInfoView";
	private const string BUFFTIPS_VIEW = "Prefabs/Module/MainUIModule/MainUIBuffTipsView";
	private const string SATIATIONPROPSUSE_VIEW = "Prefabs/Module/MainUIModule/SatiationPropsUseView";

	public static void Open(){
		if(MainUIViewController.Instance == null){
			GameObject ui = UIModuleManager.Instance.OpenFunModule(MAINUI_VIEW,0,false);
			var controller = ui.GetMissingComponent<MainUIViewController>();
			controller.InitView();
		}else{
			MainUIViewController.Instance.Dispose();
			MainUIViewController.Instance.InitView();
		}
	}

	public static void Hide(){
		UIModuleManager.Instance.HideModule(MAINUI_VIEW);	
	}
	
	public static void Show(){
		UIModuleManager.Instance.OpenFunModule(MAINUI_VIEW,0,false);
	}

	public static void OpenPlayerInfoView(GameObject anchor,SimplePlayerDto playerDto){
		GameObject view = UIModuleManager.Instance.OpenFunModule(PLAYERINFO_VIEW,UILayerType.ThreeModule,false);
		GameObjectExt.AddPoolChild(anchor,view);
		PlayerInfoViewController com = view.GetMissingComponent<PlayerInfoViewController>();
		com.Open(playerDto);
	}

	public static void OpenPlayerInfoView(SimplePlayerDto playerDto){

		GameObject view = UIModuleManager.Instance.OpenFunModule(PLAYERINFO_VIEW,UILayerType.ThreeModule,false);
		PlayerInfoViewController com = view.GetMissingComponent<PlayerInfoViewController>();
		com.Open(playerDto);

	}

	public static void OpenPlayerInfoView(SimplePlayerDto playerDto,Vector3 position){
		
		GameObject view = UIModuleManager.Instance.OpenFunModule(PLAYERINFO_VIEW,UILayerType.ThreeModule,false);
		PlayerInfoViewController com = view.GetMissingComponent<PlayerInfoViewController>();
		com.Open(playerDto,position);
		
	}


	public static void ClosePlayerInfoView(){
		UIModuleManager.Instance.CloseModule(PLAYERINFO_VIEW);
	}

	public static void OpenBuffTipsView(){
		GameObject view = UIModuleManager.Instance.OpenFunModule(BUFFTIPS_VIEW,UILayerType.SubModule,false);
		var com = view.GetMissingComponent<MainUIBuffTipsViewController>();
		com.InitView();
	}

	public static void CloseBuffTipsView(){
		UIModuleManager.Instance.CloseModule(BUFFTIPS_VIEW);
	}

	public static void OpenSatiationPropsUseView(){
		if(!PlayerModel.Instance.isFullSatiation()){
			GameObject view = UIModuleManager.Instance.OpenFunModule(SATIATIONPROPSUSE_VIEW,UILayerType.SubModule,false);
			var com = view.GetMissingComponent<SatiationPropsUseViewController>();
			com.InitView();
		}else
			TipManager.AddTip("饱食次数已满");
	}

	public static void CloseSatiationPropsUseView(){
		UIModuleManager.Instance.CloseModule(SATIATIONPROPSUSE_VIEW);
	}
}
