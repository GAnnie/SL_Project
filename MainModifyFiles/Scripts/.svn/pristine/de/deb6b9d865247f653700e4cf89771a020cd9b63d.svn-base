using UnityEngine;
using System.Collections;
using System;

public class AnimatorTestController : MonoBehaviour 
{
	public UILabel changeViewIdLbl;
	public UIInput mutateColorInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void On_ChangeView()
	{
		try
		{
			WorldManager.Instance.GetHeroView().ChangeView(int.Parse(changeViewIdLbl.text));
		}
		catch(Exception e)
		{
			TipManager.AddTip("变身id必须为数字");
		}
	}

	public void On_Anim_idle()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_idle);
	}

	public void On_Anim_battle()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_battle);
	}

	public void On_Anim_attack1()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_attack1);
	}

	public void On_Anim_attack2()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_attack2);
	}

	public void On_Anim_attack3()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_attack3);
	}

	public void On_Anim_death()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_death);
	}

	public void On_Anim_skill()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_skill);
	}

	public void On_Anim_run()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_run);
	}

	public void On_Anim_hit()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_hit);
	}

	public void On_Anim_def()
	{
		WorldManager.Instance.GetHeroView().TextPlayAnimation(ModelHelper.Anim_def);
	}
	public void On_Mutate()
	{
		WorldManager.Instance.GetHeroView ().MutateTest (mutateColorInput.value);
	}

	public void OnClose()
	{
		ProxyAnimatorTestModule.Close();
	}
}
