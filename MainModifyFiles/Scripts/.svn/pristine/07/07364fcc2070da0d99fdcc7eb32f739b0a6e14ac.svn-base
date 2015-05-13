using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.data;

public class SkillTipsViewController : MonoBehaviourBase,IViewController 
{
	private SkillTipsView _view;
	
	public void InitView()
	{
		_view = gameObject.GetMissingComponent<SkillTipsView> ();
		_view.Setup(this.transform);
		RegisterEvent();
	}

	public void RegisterEvent()
	{
		UICamera.onClick += ClickEventHandler;
	}

	public void Dispose()
	{
		UICamera.onClick -= ClickEventHandler;
	}

	public void Show(string skillName,string skillDes,GameObject anchor)
	{
		_view.NameLabel.text = skillName;
		_view.SkillDescriptionLbl.text = skillDes;

		_view.ContentBg.height = 120 + _view.SkillDescriptionLbl.height;

		_view.posAnchor.container = anchor;
		_view.posAnchor.pixelOffset = new Vector2(-_view.ContentBg.width/2,0);
		_view.posAnchor.Update();

        UIPanel panel = UIPanel.Find(_view.ContentBg.cachedTransform);
        panel.ConstrainTargetToBounds(_view.ContentBg.cachedTransform, true);
	}

	public void Show(SkillInfo skillInfo,GameObject anchor)
	{
		//_view.IconSprite.spriteName = "";
		_view.NameLabel.text = skillInfo.skill.name;
		_view.SkillDescriptionLbl.text = skillInfo.skill.description;
        _view.IconSprite.spriteName = skillInfo.skill.icon.ToString();

		_view.ContentBg.height = 120 + _view.SkillDescriptionLbl.height;
		//Debug.Log(_view.SkillDescriptionLbl.height);

		_view.posAnchor.container = anchor;
		_view.posAnchor.pixelOffset = new Vector2(-_view.ContentBg.width/2,_view.ContentBg.height - 20);
		_view.posAnchor.Update();

        UIPanel panel = UIPanel.Find(_view.ContentBg.cachedTransform);
        panel.ConstrainTargetToBounds(_view.ContentBg.cachedTransform, true);
	}

	public void Show(Skill skill,GameObject anchor){
		_view.NameLabel.text = skill.name;
		_view.SkillDescriptionLbl.text = skill.description;
        _view.IconSprite.spriteName = skill.icon.ToString();

		_view.ContentBg.height = 120 + _view.SkillDescriptionLbl.height;
		_view.posAnchor.container = anchor;
		_view.posAnchor.pixelOffset = new Vector2(-_view.ContentBg.width/2,_view.ContentBg.height - 20);
		_view.posAnchor.Update();

        UIPanel panel = UIPanel.Find(_view.ContentBg.cachedTransform);
        panel.ConstrainTargetToBounds(_view.ContentBg.cachedTransform, true);
	}

	void ClickEventHandler(GameObject go){
		CloseView();
	}

	private void CloseView()
	{
		ProxySkillModule.CloseTips();
	}
}
