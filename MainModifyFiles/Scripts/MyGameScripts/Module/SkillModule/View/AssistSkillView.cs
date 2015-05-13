﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for AssistSkillView.
/// </summary>
public class AssistSkillView : BaseView
{
	public UIButton AssistSkillStudyBtn;
	public UIGrid AssistSkillGrid;
	public GameObject WorkBtnPos;
	public UIButton WorkNotesBtn;
	public UILabel VigourValLbl;
	public UIButton VigourNotesBtn;
	public UIButton StorySkillStudyBtn;
	public UIGrid StorySkillGrid;

	public void Setup (Transform root)
	{
		AssistSkillStudyBtn = root.Find("TopGroup/AssistSkillStudyBtn").GetComponent<UIButton>();
		AssistSkillGrid = root.Find("TopGroup/AssistSkillGrid").GetComponent<UIGrid>();
		WorkBtnPos = root.Find("TopGroup/WorkBtnPos").gameObject;
		WorkNotesBtn = root.Find("TopGroup/WorkNotesBtn").GetComponent<UIButton>();
		VigourValLbl = root.Find("TopGroup/vigour/bgSprite/valueBg/VigourValLbl").GetComponent<UILabel>();
		VigourNotesBtn = root.Find("TopGroup/vigour/bgSprite/valueBg/VigourNotesBtn").GetComponent<UIButton>();
		StorySkillStudyBtn = root.Find("BottomGroup/StorySkillStudyBtn").GetComponent<UIButton>();
		StorySkillGrid = root.Find("BottomGroup/StorySkillGrid").GetComponent<UIGrid>();
	}
}
