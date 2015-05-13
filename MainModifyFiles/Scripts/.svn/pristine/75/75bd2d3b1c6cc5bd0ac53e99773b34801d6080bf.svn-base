using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.dto;
using com.nucleus.h1.logic.core.modules.player.dto;

public class CharactorProperyController : MonoBehaviourBase {
	
	CharacterPropertyView _view;
	private bool _isPlayer;

	private List<UISprite> _buffIconList;
	
	public void InitItem (bool isPlayer)
	{
		_isPlayer = isPlayer;

		_view = gameObject.GetMissingComponent<CharacterPropertyView> ();
		_view.Setup(this.transform);
		_view.sp_foreground_UISprite.spriteName = isPlayer ? "yellow-line" : "the-exp-line";

//		_view.buffGrid.gameObject.SetActive (false);
		if(isPlayer){
			UpdateBuffList();
			PlayerBuffModel.Instance.OnPlayerBuffUpdate += UpdateBuffList;
		}
		else
			_view.mainUIBuffIcon.SetActive(false);
	}

	private void UpdateBuffList(){
		if(_buffIconList == null){
			//初始化buff列表
			_buffIconList = new List<UISprite>(8);
			for(int i=0;i<8;++i){
				GameObject item = i==0?_view.mainUIBuffIcon:NGUITools.AddChild(_view.buffGrid.gameObject,_view.mainUIBuffIcon);
				UISprite icon = item.GetComponent<UISprite>();
				UIEventTrigger btn = item.GetComponent<UIEventTrigger>();
				EventDelegate.Set(btn.onClick,OnClickBuffBtn);
				_buffIconList.Add(icon);
				item.SetActive(false);
			}
		}

		List<int> buffList= PlayerBuffModel.Instance.GetActiveBuffList();
		if(buffList != null){
			for(int i=0;i<_buffIconList.Count;++i){
				if(i<buffList.Count){
					_buffIconList[i].spriteName = buffList[i].ToString();
					_buffIconList[i].cachedGameObject.SetActive(true);
				}
				else
					_buffIconList[i].cachedGameObject.SetActive(false);
			}
		}

		_view.buffGrid.repositionNow = true;
	}

	private void OnClickBuffBtn(){
		ProxyMainUIModule.OpenBuffTipsView();
	}

	public void SetupIconFunc(EventDelegate.Callback callBack)
	{
		EventDelegate.Set(_view.iconBtn.onClick,callBack);
	}

	public void SetupStatusBarFunc(EventDelegate.Callback callBack)
	{
		EventDelegate.Set(_view.PlayerPropertyInfo_UIButton.onClick,callBack);
	}

	public void RemoveStatusBarFunc(EventDelegate.Callback callBack)
	{
		EventDelegate.Remove (_view.PlayerPropertyInfo_UIButton.onClick, callBack);
	}

	public void SetCharacterDto(object dto)
	{
		if (dto == null)
		{
			_view.icon.spriteName = "pet_null";
		}
		else if (dto is SimpleCharactorDto)
		{
			_view.icon.spriteName = "player_default";
		}
		else if (dto is PetCharactorDto)
		{
			_view.icon.spriteName = "pet_default";
		}
	}

	public void SetLvLbl(int lv){
		_view.lvLbl.text = string.Format("[b]{0}[-]",lv);
	}

	public void SetHpSlider(int hp,int maxHp){
		//_view.hpLbl.text = string.Format("{0}",hp);
		if (maxHp == 0)
		{
			_view.hpSlider.value = 0;
		}else{
			_view.hpSlider.value = (float)hp / (float)maxHp;
		}
	}

	public void SetMpSlider(int mp,int maxMp){
		//_view.mpLbl.text = string.Format("{0}",mp);
		if (maxMp == 0)
		{
			_view.mpSlider.value = 0;
		}else{
			_view.mpSlider.value = (float)mp / (float)maxMp;
		}
	}

	public void SetSpSlider(long sp,long maxSp){
		//_view.spLbl.text = string.Format("{0}",sp);
		if (maxSp == 0)
		{
			_view.spSlider.value = 0;
		}else{
			_view.spSlider.value = (float)sp / (float)maxSp;
		}
	}

	public void SetExpSlider(long exp,long maxExp){
		//_view.spLbl.text = "";
		if (maxExp == 0)
		{
			_view.spSlider.value = 0;
		}else{
			_view.spSlider.value = (float)exp / (float)maxExp;
		}
	}

	public void ChangeMode(bool isBattle)
	{
//		_view.hpLbl.gameObject.SetActive (isBattle);
//		_view.mpLbl.gameObject.SetActive (isBattle);
//		_view.spLbl.gameObject.SetActive (isBattle);
	}

	public void SetEnable(bool enable)
	{
//		_view.hpLbl.text = "";
//		_view.mpLbl.text = "";
//		_view.spLbl.text = "";
		_view.lvLbl.text = "";

		_view.hpSlider.value = 0;
		_view.mpSlider.value = 0;
		_view.spSlider.value = 0;

		//_view.LevelBg.SetActive (enable);

//		_view.lvLbl.gameObject.SetActive (enable);
		//_view.buffGrid.gameObject.SetActive (enable);
	}
}
