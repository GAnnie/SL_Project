﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for MonsterSelectPrefab.
/// </summary>
public class MonsterSelectView : BaseView
{
	static public MonsterSelectView CreateNew(MonsterController mc)
	{
		Transform tf = mc.GetMountHit ();
		
		if ( tf == null )
		{
			Debug.LogError("MonsterSelectView CreateNew MountHit == null");
			return null;
			//tf = mc.gameObject.transform;
		}
		
		GameObject monsterSelectPrefab = Resources.Load("Prefabs/Module/Battle/BattleUI/MonsterSelectPrefab") as GameObject;
		GameObject monsterSelectGO = NGUITools.AddChild(LayerManager.Instance.battleHudTextAnchor, monsterSelectPrefab);
		MonsterSelectView monsterSelectView = GameObjectExt.GetMissingComponent<MonsterSelectView>(monsterSelectGO);
		monsterSelectView.Setup (monsterSelectGO.transform);

		UIFollowTarget follower = monsterSelectGO.AddComponent<UIFollowTarget>();
		
		follower.gameCamera = LayerManager.Instance.GetBattleFollowCamera();
		follower.uiCamera = LayerManager.Instance.UICamera;
		
		tf.localRotation = Quaternion.identity;
		tf.localScale = Vector3.one;
		
		tf.position = new Vector3(tf.position.x, tf.position.y, tf.position.z);	
		
		follower.target = tf;
		follower.offset = new Vector3(0,0,0);

		monsterSelectView.SetData (mc);

		return monsterSelectView;
	}


	public UILabel NameLabel_UILabel;
	
	public void Setup (Transform root)
	{
		NameLabel_UILabel = root.Find("NameLabel").GetComponent<UILabel>();
	}

	private MonsterController _mc;
	private void SetData(MonsterController mc)
	{
		_mc = mc;
		if (mc.GetFaction () != null) 
		{
			NameLabel_UILabel.text = "[b]" + mc.GetGrade() + " " + mc.GetFaction ().shortDesc;
		} 
		else 
		{
			if (mc.IsMonster())
			{
				NameLabel_UILabel.text = "";
			}
			else
			{
				NameLabel_UILabel.text = "[b]" + mc.GetGrade().ToString();
			}
		}
	}

	public void Show(bool show)
	{
		this.gameObject.SetActive (show);

		if (show)
		{
			_mc.PlayTargetSelectEffect ();
		}
		else{
			_mc.StopTargetSelectEffect();
		}
	}

	public void Destroy()
	{
		_mc.StopTargetSelectEffect();
		GameObject.Destroy(this.gameObject);
		_mc = null;
	}
}
