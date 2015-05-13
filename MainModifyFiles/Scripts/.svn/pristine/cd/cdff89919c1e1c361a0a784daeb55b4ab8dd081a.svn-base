// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  StorySkillCellController.cs
// Author   : willson
// Created  : 2015/4/2 
// Porpuse  : 
// **********************************************************************

public class StorySkillCellController : MonoBehaviourBase,IViewController
{
	private StorySkillCell _view;
	
	public void InitView()
	{
		_view = gameObject.GetMissingComponent<StorySkillCell>();
		_view.Setup(this.transform);
		
		_view.CountLbl.text = "";
		_view.IconSprite.enabled = false;
		
		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		EventDelegate.Set(_view.IconSpriteBtn.onClick,OnClickIcon);
	}
	
	private void OnClickIcon()
	{
		TipManager.AddTip("22222");
	}
	
	public void Dispose()
	{
	}
}