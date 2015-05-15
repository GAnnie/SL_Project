using com.nucleus.h1.logic.core.modules.guild.dto;
using com.nucleus.h1.logic.services;
using System.Collections.Generic;
using UnityEngine;
public class GuildsInfoWinUIController : MonoBehaviourBase, IViewController
{
    private const string GuildInfoCellName = "Prefabs/Module/GuildModule/GuildInfoCell";

    private GuildsInfoWinUI _view;

    private List<GuildInfoCellController> _guildInfoCellList;
    private GuildInfoCellController _currCell;

    public void InitView()
    {
        _view = gameObject.GetMissingComponent<GuildsInfoWinUI>();
        _view.Setup(this.transform);

        _view.GuildsInfoGroup.SetActive(true);
        _view.GuildCreateGroup.SetActive(false);
        RegisterEvent();

        _guildInfoCellList = new List<GuildInfoCellController>();
    }

    public void RegisterEvent()
    {
        EventDelegate.Set(_view.CloseBtn.onClick, OnCloseBtn);

        EventDelegate.Set(_view.AllApplyJoinButton.onClick, OnAllApplyJoinBtn);
        EventDelegate.Set(_view.ApplyJoinButton.onClick, ApplyJoinBtn);
        EventDelegate.Set(_view.CreateButton.onClick, OnShowGuildCreateGroup);
        
    }

    public void SetData(GuildListDto dto)
    {
        AddGuildInfo(dto);
    }

    private void AddGuildInfo(GuildListDto dto)
    {
        int index = 0;
        for(;index < dto.list.Count;index++)
        {
            if(index < _guildInfoCellList.Count)
            {
                _guildInfoCellList[index].SetData(dto.list[index], OnSelectGuild);
            }
            else
            {
                GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab(GuildInfoCellName) as GameObject;
                GameObject module = GameObjectExt.AddChild(_view.GuildListGrid.gameObject, prefab);
                GuildInfoCellController cell = module.GetMissingComponent<GuildInfoCellController>();
                cell.InitView();
                cell.SetData(dto.list[index], OnSelectGuild);
                _guildInfoCellList.Add(cell);
            }
        }

        while (index < _guildInfoCellList.Count)
        {
            _guildInfoCellList[index].SetActive(false);
            index++;
        }

        _view.GuildListGrid.Reposition();
    }

    private void OnSelectGuild(GuildInfoCellController cell)
    {
        _currCell = cell;
        _view.PurposeLabel.text = _currCell.GetData().notice;

        for(int index = 0;index < _guildInfoCellList.Count;index++)
        {
            _guildInfoCellList[index].IsSelect = cell == _guildInfoCellList[index];
        }
    }

    private void OnAllApplyJoinBtn()
    {
        ServiceRequestAction.requestServer(GuildService.applyAll(), "applyAll", (e) =>
        {
            TipManager.AddTip("一键申请 成功");
        });
    }

    private void ApplyJoinBtn()
    {
        if(_currCell != null)
        {
            ServiceRequestAction.requestServer(GuildService.applyJoin(_currCell.GetData().id), "applyJoin", (e) =>
            {
                TipManager.AddTip(string.Format("申请{0}成功", _currCell.GetData().name));
            });
        }
        else
        {
            TipManager.AddTip("请选中帮会");
        }
    }

    private UIButton _OkBtn;
    private UIButton _CancelBtn;
    private void OnShowGuildCreateGroup()
    {
        _view.GuildsInfoGroup.SetActive(false);
        _view.GuildCreateGroup.SetActive(true);
        if(_OkBtn == null)
        {
            _OkBtn = UIHelper.CreateBaseBtn(_view.OkBtnPos, "创建帮派", OnCreateGuildBtn);
        }

        if (_CancelBtn == null)
        {
            _CancelBtn = UIHelper.CreateBaseBtn(_view.CancelBtnPos, "取消", OnCancelBtn);
        }
    }

    private void OnCreateGuildBtn()
    {
        if(!string.IsNullOrEmpty(_view.GuildNameLabel.text) && !string.IsNullOrEmpty(_view.GuildPurposeLabel.text))
        {
            ServiceRequestAction.requestServer(GuildService.create(_view.GuildNameLabel.text,_view.GuildPurposeLabel.text),"getAllGuild",(e)=>
            {
                TipManager.AddTip("成功创建帮派");
                GuildModel.Instance.UpDateMyGuildInfo(e as GuildDto);
            });
             OnCloseBtn();
        }
        else
        {
            TipManager.AddTip("帮派名字,宗旨不能为空");
        }
    }

    private void OnCancelBtn()
    {
        _view.GuildsInfoGroup.SetActive(true);
        _view.GuildCreateGroup.SetActive(false);
    }

    private void OnCloseBtn()
    {
        ProxyGuildModule.CloseGuildInfoList();
    }

    public void Dispose()
    {

    }
}