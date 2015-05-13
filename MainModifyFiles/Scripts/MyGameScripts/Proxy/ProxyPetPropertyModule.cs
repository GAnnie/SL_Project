using UnityEngine;
using System.Collections;

public static class ProxyPetPropertyModule{

	public const string PETBASEINFO_VIEW = "Prefabs/Module/PetPropertyModule/PetBaseInfoView";
	private const string PETPROPERTY_MAINVIEW = "Prefabs/Module/PetPropertyModule/PetPropertyMainView";
	public const string PETHANDBOOK_VIEW = "Prefabs/Module/PetPropertyModule/PetHandBookView";
	private const string PETCHANGENAME_VIEW = "Prefabs/Module/PetPropertyModule/PetChangeNameView";
	private const string PETEQCOMBINE_VIEW = "Prefabs/Module/PetPropertyModule/PetEquipmentCombineView";
	private const string PETINCREASEBASEAP_VIEW = "Prefabs/Module/PetPropertyModule/PetIncreaseBaseApView";

	public static void Open(int defaultRightTab=0){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(PETPROPERTY_MAINVIEW, UILayerType.DefaultModule,true);
		var controller = ui.GetMissingComponent<PetPropertyMainViewController>();
		controller.Open(defaultRightTab);
	}
	
	public static void Close(){
		UIModuleManager.Instance.CloseModule(PETPROPERTY_MAINVIEW);
	}
	
	public static void Hide(){
		UIModuleManager.Instance.HideModule(PETPROPERTY_MAINVIEW);	
	}
	
	public static void Show(){
		UIModuleManager.Instance.OpenFunModule(PETPROPERTY_MAINVIEW,UILayerType.DefaultModule,true);
	}

	public static void OpenChangeNameView(PetPropertyInfo petInfo){
		if(petInfo == null) return;

		GameObject ui = UIModuleManager.Instance.OpenFunModule(PETCHANGENAME_VIEW, UILayerType.FourModule,true);
		var controller = ui.GetMissingComponent<PetChangeNameViewController>();
		controller.Open(petInfo);
	}

	public static void CloseChangeNameView(){
		UIModuleManager.Instance.CloseModule(PETCHANGENAME_VIEW);
	}

	public static void OpenEqCombineView(){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(PETEQCOMBINE_VIEW, UILayerType.FourModule,true);
		var controller = ui.GetMissingComponent<PetEqCombineViewController>();
		controller.InitView();
	}

	public static void CloseEqCombineView(){
		UIModuleManager.Instance.CloseModule(PETEQCOMBINE_VIEW);
	}

	public static void OpenIncreaseBaseApView(PetPropertyInfo petInfo){
		GameObject ui = UIModuleManager.Instance.OpenFunModule(PETINCREASEBASEAP_VIEW, UILayerType.FourModule,true);
		var controller = ui.GetMissingComponent<PetIncreaseBaseApViewController>();
		controller.Open(petInfo);
	}

	public static void CloseIncreaseBaseApView(){
		UIModuleManager.Instance.CloseModule(PETINCREASEBASEAP_VIEW);
	}
}
