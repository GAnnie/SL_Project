// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  VigourWinUIController.cs
// Author   : willson
// Created  : 2015/4/11 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using com.nucleus.h1.logic.core.modules.faction.dto;
using com.nucleus.h1.logic.core.modules.assistskill.data;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.player.dto;

public class VigourWinUIController: MonoBehaviourBase,IViewController
{
	private VigourWinUI _view;
    private UIEventListener _workBtnListener;

	public void InitView()
	{
		_view = gameObject.GetMissingComponent<VigourWinUI> ();
		_view.Setup (this.transform);

		InitVigour();
		InitWork();
		InitFactionSKill();

		RegisterEvent();
	}

	public void RegisterEvent()
	{
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtn);
		UIButton workBtn = UIHelper.CreateBaseBtn(_view.WorkBtnPos,"打工");
        _workBtnListener = workBtn.gameObject.GetMissingComponent<UIEventListener>();
        _workBtnListener.onPress += OnWorkBtn;

		UIHelper.CreateBaseBtn(_view.AssistSkillBtn,"制作",OnAssistSkillBtn);
		UIHelper.CreateBaseBtn(_view.FactionSKillBtn,"制作",OnFactionSKillBtn);

		PlayerModel.Instance.OnSubWealthChanged += OnSubWealthChanged;
	}

	private void InitVigour()
	{
		_view.VigourVuale.text = PlayerModel.Instance.Vigour + "/" + PlayerModel.Instance.VigourMax;
		float percent = PlayerModel.Instance.Vigour * 1.0f / PlayerModel.Instance.VigourMax;
		if(percent == 0)
		{
			_view.BarSprite.enabled = false;
		}
		else
		{
			_view.BarSprite.enabled = true;
			_view.BarSprite.transform.localScale = new Vector3(percent,1f,1f);
		}
	}

	private void OnSubWealthChanged(SubWealthNotify notify)
	{
		InitVigour();
	}

	private int _productId = 0;

	private void InitFactionSKill()
	{
		FactionSkillDto dto = FactionSkillModel.Instance.GetFactionAssistSkill();
		FactionAssistSkillProduct skillProduct = DataCache.getDtoByCls<FactionAssistSkillProduct>(dto.factionSkillId);
		int lv = 0;
		if(!string.IsNullOrEmpty(skillProduct.subProductStr))
		{
			string[] productStrs = skillProduct.subProductStr.Split(',');
			if(productStrs.Length == 1)
			{
				string[] info = productStrs[productStrs.Length - 1].Split(':');
				_productId = int.Parse(info[0]);
				lv = dto.factionSkillLevel;
			}
			else
			{
				for(int index = productStrs.Length - 1;index >= 0;index--)
				{
					AssistSkillProduct product = new AssistSkillProduct();
					string[] info = productStrs[index].Split(':');
					
					/** 道具id */
					_productId = int.Parse(info[0]);
					/** 产品等级 */
					lv = int.Parse(info[1]);

					if(lv <= dto.factionSkillLevel)
						break;
				}
			}
		}

        /*
		int vigourConsume = LuaManager.Instance.DoVigourConsumeFormula("VigourConsume_" + dto.factionSkillId,
		                                                               skillProduct.vigourConsumeFormula,lv);
		_view.FactionSKillVigourLabel.text = "消耗活力：" + vigourConsume;
        */
        _view.FactionSKillVigourLabel.text = "";

		_view.FactionSkillLabel.text = dto.factionSkill.name;
	}

	private void InitWork()
	{
		_view.WorkVigourLabel.text = "消耗活力：40"; 
	}

    void Update()
    {
        if (!_pressWorkBtn)
            return;
        if (_mDragStartTime > RealTime.time)
            return;

        if (_pressWorkBtn)
        {
            _pressWorkBtn = false;
            handleWork(true);
        }
    }

    private float _mDragStartTime = 0f;
    private float _pressAndHoldDelay = 0.5f;
  
    private bool _pressWorkBtn;
    private void OnWorkBtn(GameObject go, bool state)
	{
		//TipManager.AddTip("我地呢班打工仔，通街走粜直头系坏肠胃，揾个些少到月底点够驶");
        if(state)
        {
            _pressWorkBtn = state;
            _mDragStartTime = RealTime.time + _pressAndHoldDelay;
        }
        else
        {
            if(_pressWorkBtn)
            {
                _pressWorkBtn = false;
                handleWork(false);
            }
        }
	}

    private void handleWork(bool isAll)
    {
        if (PlayerModel.Instance.Vigour < 40)
        {
            TipManager.AddTip("你的活力不足40点，无法打工");
            return;
        }

        if (!isAll)
        {
            ServiceRequestAction.requestServer(AssistSkillService.vigourConvertCopper(40), "factionMake", (e) =>
            {
                TipManager.AddTip(string.Format("消耗了[{0}]40[-]点活力，你获得了[{1}]4000[-]{2}",
                    ColorConstant.Color_Tip_LostCurrency_Str, ColorConstant.Color_Tip_GainCurrency_Str, ItemIconConst.Copper));
            });
        }
        else
        {
            int multiple = PlayerModel.Instance.Vigour / 40;
            ServiceRequestAction.requestServer(AssistSkillService.vigourConvertCopper(multiple * 40), "factionMake", (e) =>
            {
                TipManager.AddTip(string.Format("消耗了[{0}]{1}[-]点活力，你获得了[{2}]{3}[-]{4}",
                    ColorConstant.Color_Tip_LostCurrency_Str, multiple * 40, ColorConstant.Color_Tip_GainCurrency_Str, multiple * 4000, ItemIconConst.Copper));
            });
        }
    }

	private void OnAssistSkillBtn()
	{
		ProxyPlayerPropertyModule.CloseVigourWin();
		ProxyPlayerPropertyModule.Close();
		ProxySkillModule.Open(2);
	}

	private void OnFactionSKillBtn()
	{
        /*
		ServiceRequestAction.requestServer(AssistSkillService.factionMake(_productId),"factionMake",(e)=>{
			AssistSkillProductResultDto dto = e as AssistSkillProductResultDto;
			//GameLogicHelper.HandlerItemTipDto(dto.item,false);
			if(dto != null)
				TipManager.AddTip("获得"+dto.item.item.name);
		});
         * */
        ProxyPlayerPropertyModule.CloseVigourWin();
        ProxyPlayerPropertyModule.Close();
        ProxySkillModule.OpenAssistSkillFactionWin(FactionSkillModel.Instance.GetFactionAssistSkill());
	}

	private void OnCloseBtn()
	{
		ProxyPlayerPropertyModule.CloseVigourWin();
	}

	public void Dispose()
	{
		PlayerModel.Instance.OnSubWealthChanged -= OnSubWealthChanged;
        _workBtnListener.onPress -= OnWorkBtn;
	}
}