// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AutoFramMapCellController.cs
// Author   : willson
// Created  : 2015/1/8 
// Porpuse  : 
// **********************************************************************
using com.nucleus.h1.logic.core.modules.scene.data;
using com.nucleus.h1.logic.core.modules.battle.data;
using UnityEngine;

public class AutoFramMapCellController : MonoBehaviourBase,IViewController
{
	private AutoFramMapCell _view;

	private ModelDisplayController _modelController;

	private SceneMap _map;

	private System.Action<SceneMap> _showSceneMapMonsterFun;

	private Monster _showMonster;
	private bool _isShowMonster;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<AutoFramMapCell> ();
		_view.Setup(this.transform);

		RegisterEvent();
	}
	
	public void RegisterEvent()
	{
		EventDelegate.Set(_view.SpriteBgBtn.onClick,OnBtnClick);
		EventDelegate.Set(_view.TipsButton.onClick,OnTipsClick);
	}

	public void SetData(SceneMap map,System.Action<SceneMap> showSceneMapMonsterFun)
	{
		_isShowMonster = false;
		_map = map;
		_showSceneMapMonsterFun = showSceneMapMonsterFun;

		_view.MapNameLabel.text = map.name;
		_view.MapLvLabel.text = map.description;

		_modelController = ModelDisplayController.GenerateUICom (_view.ModelAnchor.transform);
		_modelController.Init (200,200);
		_modelController.SetBoxColliderEnabled(false);

		_showMonster = DataCache.getDtoByCls<Monster>(map.monsterIds[0]);

	}

	public SceneMap GetData()
	{
		return _map;
	}

	public bool IsShowMonster()
	{
		return _isShowMonster;
	}

	public void ShowMonster()
	{
		if(!_isShowMonster)
		{
			_modelController.SetupModel(_showMonster.pet);
			_isShowMonster = true;
		}
	}

	public void SetRecommend(bool b)
	{
		if(b)
		{
			_view.SpriteBg.spriteName = "storehouse-under-ground";
			_view.SpriteBgBtn.normalSprite = "storehouse-under-ground";
		}
		else
		{
			_view.SpriteBg.spriteName = "panel_border_001";
			_view.SpriteBgBtn.normalSprite = "panel_border_001";
		}
	}

	private void OnBtnClick()
	{
		if(WorldManager.Instance.GetModel().GetSceneId() == _map.id)
		{
			PlayerModel.Instance.StartAutoFram();
		}
		else
		{
			//PlayerModel.Instance.IsAutoFram = true;
			WorldManager.Instance.Enter(_map.id, false,true);
		}
		ProxyAutoFramModule.Close();
	}

	private void OnTipsClick()
	{
		if(_showSceneMapMonsterFun != null)
			_showSceneMapMonsterFun(_map);
	}

	public void Dispose()
	{
	}
}