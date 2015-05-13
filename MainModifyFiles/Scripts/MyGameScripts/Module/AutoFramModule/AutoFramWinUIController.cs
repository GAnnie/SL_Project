using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.scene.data;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules;

public class AutoFramWinUIController : MonoBehaviourBase,IViewController
{
	private const string AutoFramMapCellName = "Prefabs/Module/AutoFramModule/AutoFramMapCell";
	private const string MonsterIconCellName = "Prefabs/Module/AutoFramModule/MonsterIconCell";

	private GameObject _baseButtonPrefab;

	private AutoFramWinUI _view;
	private List<AutoFramMapCellController> _cells;

	private List<MonsterIconCellController> _monsterList;
	private bool _isAllShowMonster;
	private bool _beginShowOtherMonster;

	public void InitView()
	{
		_isAllShowMonster = false;
		_beginShowOtherMonster = false;

		_view = gameObject.GetMissingComponent<AutoFramWinUI> ();
		_view.Setup(this.transform);

		InitAutoFramMap();
		UpdateDoubleExpInfo();
		RegisterEvent();
	}

	private void InitAutoFramMap()
	{
		List<SceneMap> mapList= DataCache.getArrayByClsWithoutSort<SceneMap>();
		_cells = new List<AutoFramMapCellController>();

		for(int index = 0;index < mapList.Count;index++)
		{
			SceneMap map = mapList[index];
			if(map.monsterIds != null && map.monsterIds.Count > 0)
			{
				_cells.Add(AddAutoFramCell(map));
			}
		}

		// 设置推荐挂机地图
		int mapIndex = -1;
		for(int index = 0;index < _cells.Count;index++)
		{
			SceneMap map = _cells[index].GetData();
			if(map.levelLimit <= PlayerModel.Instance.GetPlayerLevel() 
			   && PlayerModel.Instance.GetPlayerLevel() <= map.levelLimit + 9)
			{
				mapIndex = index;
			}
			else
			{
				_cells[index].SetRecommend(false);
			}
		}

		if(mapIndex == -1)
		{
			SceneMap map = _cells[0].GetData();
			if(PlayerModel.Instance.GetPlayerLevel() < map.levelLimit)
			{
				mapIndex = 0;
			}
			else
			{
				mapIndex = _cells.Count - 1;
			}
		}
		_cells[mapIndex].SetRecommend(true);

		// 定位到最优
		if(mapIndex < 2)
		{
			// 头
			_view.MapPanelScrollView.SetDragAmount(0,0,false);
			int index = 0;
			while(index < 4 && index < _cells.Count)
			{
				_cells[index].ShowMonster();
				index++;
			}
		}
		else if(mapIndex >= _cells.Count - 3)
		{
			// 尾
			_view.MapPanelScrollView.SetDragAmount(1f,0,false);
			int index = 1;
			while(index <= 4 && _cells.Count - index >= 0)
			{
				_cells[_cells.Count - index].ShowMonster();
				index++;
			}
		}
		else if(_cells.Count - 4 > 0)
		{
			// 中间
			float v = (mapIndex - 1)* 1.0f/(_cells.Count - 4);
			_view.MapPanelScrollView.SetDragAmount(v,0,false);
			int beginIndex = mapIndex - 1;
			int index = 0;
			while(index < 4 && index + beginIndex < _cells.Count)
			{
				_cells[index + beginIndex].ShowMonster();
				index++;
			}
		}
		_isAllShowMonster = false;
		_beginShowOtherMonster = true;
	}

	void Update ()
	{
		if(_isAllShowMonster == false && _beginShowOtherMonster == true)
		{
			_isAllShowMonster = true;
			for(int index = 0;index < _cells.Count;index++)
			{
				if(_cells[index].IsShowMonster() == false)
				{
					_cells[index].ShowMonster();
					_isAllShowMonster = false;
					break;
				}
			}
		}
	}

	private AutoFramMapCellController AddAutoFramCell(SceneMap map)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( AutoFramMapCellName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(_view.MapGrid.gameObject,prefab);
		//UIHelper.AdjustDepth(module,1);

		AutoFramMapCellController cell = module.GetMissingComponent<AutoFramMapCellController>();
		cell.InitView();
		cell.SetData(map,ShowSceneMapMonster);
		_view.MapGrid.Reposition();
		return cell;
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);

		UIHelper.CreateBaseBtn(_view.AutoFramBtnPos,"原地巡逻",OnAutoFramBtn);
		UIHelper.CreateBaseBtn(_view.FrozenPos,"冻结",OnFrozenBtn);
		UIHelper.CreateBaseBtn(_view.ReceivePos,"领取",OnReceiveBtn);

		EventDelegate.Set(_view.AutoFramTipsButton.onClick,OnShowAutoFramTips);
		EventDelegate.Set(_view.ReceiveTipsButton.onClick,OnShowReceiveTips);

		EventDelegate.Set(_view.TipsBoxCollider.onClick,OnCloseTips);
		EventDelegate.Set(_view.MonsterBoxCollider.onClick,OnCloseMonster);
		PlayerModel.Instance.OnOpenDoublePointChanged += UpdateDoubleExpInfo;
	}

	private void OnAutoFramBtn()
	{
		if(WorldManager.Instance.GetModel().GetSceneDto().sceneMap.monsterIds.Count > 0)
		{
			PlayerModel.Instance.StartAutoFram();
			ProxyAutoFramModule.Close();
		}
		else
		{
			TipManager.AddTip("当前场景不是挂机场景，不能进行原地巡逻");
		}
	}

	private void OnFrozenBtn()
	{
		PlayerModel.Instance.FreezeDoubleExp();
	}

	private void OnReceiveBtn()
	{
		PlayerModel.Instance.ReceiveDoubleExp();
	}

	private void UpdateDoubleExpInfo(){
		DoubleExpStateBarDto dto = PlayerModel.Instance.GetDoubleExpDto();
		_view.PointLabel.text = string.Format("双倍点数：{0}",dto.point);
		_view.GetPointLabel.text = string.Format("已领取：{0}",dto.openPoint);
	}

	private void OnShowAutoFramTips()
	{
		_view.TipsPanel.SetActive(true);

		_view.TipsTitlelbl.text = "挂机说明";
		_view.TipsInfolbl.text = "1、在这些场景中自动巡逻，可以遭遇暗雷怪物，战胜后可获得经验奖励。\n2、长按主界面【挂机】按钮可进入自动巡逻。\n3、点击地面走路可以取消自动巡逻。\n4、领取双倍点数后会有几率遇到宠物宝宝。";
	}

	private void OnShowReceiveTips()
	{
		_view.TipsPanel.SetActive(true);

		_view.TipsTitlelbl.text = "双倍点数说明";
		_view.TipsInfolbl.text = "1、领取双倍点数后，野外打怪、抓鬼任务、封妖可获得双倍奖励，抓鬼任务获得物品概率加倍。\n2、每场战斗消耗1点，野外打怪50%概率不消耗。\n3、已领取的双倍点数可以冻结，冻结的点数会返还到总点数里，下次可以继续领取使用。";
	}
	
	private void OnCloseTips()
	{
		_view.TipsPanel.SetActive(false);
	}

	public void ShowSceneMapMonster(SceneMap map)
	{
		_view.MonsterPanel.SetActive(true);
		_view.SceneMaplbl.text = map.name;
		if(_monsterList == null)
			_monsterList = new List<MonsterIconCellController>();
		for(int index = 0;index < map.monsterIds.Count;index++)
		{
			if(index < _monsterList.Count)
			{
				_monsterList[index].SetData(map.monsterIds[index]);
			}
			else
			{
				GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( MonsterIconCellName ) as GameObject;
				GameObject module = GameObjectExt.AddChild(_view.MonsterGrid.gameObject,prefab);
				MonsterIconCellController cell = module.GetMissingComponent<MonsterIconCellController>();
				cell.InitView();
				cell.SetData(map.monsterIds[index]);
				_monsterList.Add(cell);
			}
		}
		_view.MonsterGrid.Reposition();
	}
	
	private void OnCloseMonster()
	{
		_view.MonsterPanel.SetActive(false);
	}

	public void OnCloseBtn()
	{
		ProxyAutoFramModule.Close();
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnOpenDoublePointChanged -= UpdateDoubleExpInfo;
	}
}
