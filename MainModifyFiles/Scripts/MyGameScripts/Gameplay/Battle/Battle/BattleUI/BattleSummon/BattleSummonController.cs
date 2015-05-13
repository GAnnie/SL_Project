using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BattleSummonController : MonoBehaviour,IViewController
{
	private static string PetCellPrefabPath = "Prefabs/Module/Battle/BattleUI/BattleSummonPetCell";

	private BattleSummonView _view;

	public event Action<long> OnSelect;

	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView(){
		_view = gameObject.GetMissingComponent<BattleSummonView> ();
		_view.Setup (this.transform);
	}
	
	/// <summaryprivate static string SkillCellPrefabPath = "Prefabs/Module/Battle/BattleUI/SkillCellPrefab";>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent(){
		EventDelegate.Set (_view.SummonButton_UIButton.onClick, OnSummonButtonClick);
		EventDelegate.Set (_view.CloseBtn_UIButton.onClick, OnCloseButtonClick);
	}
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose()
	{

	}
	
	public void Open(Action<long> onSelectDelegate)
	{
		if (_view == null)
		{
			InitView ();
			RegisterEvent ();
		}

		OnSelect = onSelectDelegate;

		List<PetPropertyInfo> list = PetModel.Instance.GetPetPropertyInfoList ();
		_view.BattleInfoLabel_UILabel.text = string.Format("已出战宠物数量:{0}/{1}", BattleController.Instance.GetOldBattlePetCount(), 5);

		foreach(PetPropertyInfo info in list)
		{
			AddPetCell(info);
		}
	}

	private BattleSummonPetCell _currentSelectCell;
	private long _currentSelectPetId;

	private void AddPetCell(PetPropertyInfo petInfo)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab(PetCellPrefabPath ) as GameObject;
		GameObject go = NGUITools.AddChild(_view.PetListGrid_UIGrid.gameObject, prefab);
		
		BattleSummonPetCell battleSummonPetCell = go.GetMissingComponent<BattleSummonPetCell> ();
		battleSummonPetCell.Setup (go.transform);

		battleSummonPetCell.NameLabel_UILabel.text = petInfo.pet.name;
		battleSummonPetCell.InfoLabel_UILabel.text = "等级:" + petInfo.petDto.level;

		if (BattleController.Instance.IsOldBattlePet(petInfo.petDto.id))
		{
			battleSummonPetCell.StatusSprite_UISprite.spriteName = "on-ground";
		}
		else if (BattleController.Instance.IsBattlePet(petInfo.petDto.id))
		{
			battleSummonPetCell.StatusSprite_UISprite.spriteName = "on-the-war";
		}
		else
		{
			battleSummonPetCell.StatusSprite_UISprite.spriteName = "";
		}

		EventDelegate.Set (battleSummonPetCell.BattleSummonPetCell_UIButton.onClick, delegate() {
			_currentSelectPetId = petInfo.petDto.id;
			if (_currentSelectCell != null)
			{
				_currentSelectCell.CellSelectSprite_UISprite.spriteName = "the-choice-lines";
			}
			_currentSelectCell = battleSummonPetCell;
			battleSummonPetCell.CellSelectSprite_UISprite.spriteName = "the-no-choice-lines";
			UpdatePetInfo(petInfo);
		});

		if (_currentSelectCell == null)
		{
			_currentSelectCell = battleSummonPetCell;
			battleSummonPetCell.CellSelectSprite_UISprite.spriteName = "the-no-choice-lines";
			_currentSelectPetId = petInfo.petDto.id;
			UpdatePetInfo (petInfo);
		}
	}

	private void UpdatePetInfo(PetPropertyInfo petInfo)
	{
		if (BattleController.Instance.IsOldBattlePet(petInfo.petDto.id))
		{
			_view.SummonButton_UIButton.enabled = false;
			_view.SummonButton_UIButton.GetComponent<UISprite>().isGrey = true;
		}
		else if (BattleController.Instance.IsBattlePet(petInfo.petDto.id))
		{
			_view.SummonButton_UIButton.enabled = false;
			_view.SummonButton_UIButton.GetComponent<UISprite>().isGrey = true;
		}
		else
		{
			_view.SummonButton_UIButton.enabled = true;
			_view.SummonButton_UIButton.GetComponent<UISprite>().isGrey = false;
		}

		_view.HpValLbl_UILabel.text = petInfo.hp.ToString ();
		_view.DefenseValLbl_UILabel.text = petInfo.defense.ToString ();
		_view.MpValLbl_UILabel.text = petInfo.mp.ToString ();
		_view.SpeedValLbl_UILabel.text = petInfo.speed.ToString ();
		_view.AttackValLbl_UILabel.text = petInfo.attack.ToString ();
		_view.MagicValLbl_UILabel.text = petInfo.magic.ToString ();
	}

	private void OnSummonButtonClick()
	{
		if (_currentSelectPetId > 0)
		{
			if (BattleController.Instance.GetOldBattlePetCount()+1 > 5)
			{
				TipManager.AddTip("不可以召唤更多宠物了");
				return;
			}

			if (OnSelect != null)
			{
				OnSelect(_currentSelectPetId);
			}
			ProxyBattleModule.CloseSummon ();
		}
		else
		{
			TipManager.AddTip("先选择召唤的宠物");
		}
	}

	private void OnCloseButtonClick()
	{
		ProxyBattleModule.CloseSummon ();
	}
}

