// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  PetInfoWinUIController.cs
// Author   : willson
// Created  : 2015/3/27 
// Porpuse  : 
// **********************************************************************

public class PetInfoWinUIController : MonoBehaviourBase,IViewController
{
	private PetInfoWinUI _view;

	private PetBaseInfoViewController _petBaseInfoViewController;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<PetInfoWinUI> ();
		_view.Setup(this.transform);

		RegisterEvent();

		_petBaseInfoViewController = UIModuleManager.Instance.
			AddChildPanel(ProxyPetPropertyModule.PETBASEINFO_VIEW,_view.gameObject.transform).GetMissingComponent<PetBaseInfoViewController>();
		_petBaseInfoViewController.InitView();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);
	}

	public void SetData(PetPropertyInfo petInfo)
	{
		_petBaseInfoViewController.ShowPetDetailInfo(petInfo);
	}

	private void OnCloseBtn()
	{
		ProxyPetTipsModule.Close();
	}

	public void Dispose()
	{
	}
}