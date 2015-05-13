//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for BattleDemoView.
/// </summary>
public class BattleDemoView : BaseView
{
	public UIButton BattleButton;
	public UIInput EnemyNumInput;
	public UIButton PositionButton;
	public UIButton CloseButton;
	public UIInput SceneIdInput;
	public UIInput SceneCameraInput;
	public UIInput AttackInput;
	public UIInput DefenseInput;
	public UIInput HpInput;
	public UIInput SpeedInput;
	public UIInput StyleInput;
	public UIInput activeSkillInput_UIInput;
	public UIInput passiveSkillInput_UIInput;
	public UIButton ResumeButton_UIButton;
	public UILabel NameLabel_UILabel;
	public UIInput MagicInput_UIInput;

	public void Setup (Transform root)
	{
		BattleButton = root.Find("BattleButton").GetComponent<UIButton>();
		EnemyNumInput = root.Find("EnemyNumLabel/NumInput").GetComponent<UIInput>();
		PositionButton = root.Find("PositionButton").GetComponent<UIButton>();
		CloseButton = root.Find("CloseButton").GetComponent<UIButton>();
		SceneIdInput = root.Find("SceneIdLabel/SceneIdInput").GetComponent<UIInput>();
		SceneCameraInput = root.Find("SceneCameraLabel/SceneCameraInput").GetComponent<UIInput>();
		AttackInput = root.Find("EnemyDummySettingPanel/attackLabel/AttackInput").GetComponent<UIInput>();
		DefenseInput = root.Find("EnemyDummySettingPanel/defenseLabel/DefenseInput").GetComponent<UIInput>();
		HpInput = root.Find("EnemyDummySettingPanel/hpLabel/HpInput").GetComponent<UIInput>();
		SpeedInput = root.Find("EnemyDummySettingPanel/speedLabel/SpeedInput").GetComponent<UIInput>();
		StyleInput = root.Find("EnemyDummySettingPanel/styleLabel/StyleInput").GetComponent<UIInput>();
		activeSkillInput_UIInput = root.Find("EnemyDummySettingPanel/activeSkillLabel/activeSkillInput").GetComponent<UIInput>();
		passiveSkillInput_UIInput = root.Find("EnemyDummySettingPanel/passiveSkillLabel/passiveSkillInput").GetComponent<UIInput>();
		ResumeButton_UIButton = root.Find("ResumeButton").GetComponent<UIButton>();
		NameLabel_UILabel = root.Find("EnemyDummySettingPanel/NameLabel").GetComponent<UILabel>();
		MagicInput_UIInput = root.Find("EnemyDummySettingPanel/magicLabel/MagicInput").GetComponent<UIInput>();
	}
}
