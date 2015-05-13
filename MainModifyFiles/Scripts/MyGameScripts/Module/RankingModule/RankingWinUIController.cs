using com.nucleus.h1.logic.whole.modules.trade.data;
using System.Collections.Generic;
using UnityEngine;
public class RankingWinUIController : MonoBehaviourBase, IViewController
{
    private const string RankingTypeCellName = "Prefabs/Module/RankingModule/RankingTypeCell";

    private RankingWinUI _view;

    public void InitView()
    {
        _view = gameObject.GetMissingComponent<RankingWinUI>();
        _view.Setup(this.transform);

        RegisterEvent();
    }

    public void RegisterEvent()
    {
        EventDelegate.Set(_view.CloseBtn.onClick, OnCloseBtn);
    }

    List<TradeMenu> _tradeMenuList = null;
    public void SetData()
    {
        if (_tradeMenuList == null)
        {
            _tradeMenuList = TradeDataModel.Instance.GetTradeMenu();
        }

        InitTypeList();
    }

    private void InitTypeList()
    {
        List<TradeMenu> list = new List<TradeMenu>();
        GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab(RankingTypeCellName) as GameObject;

        for (int i = 0, len = _tradeMenuList.Count; i < len; i++)
        {
            TradeMenu menu = _tradeMenuList[i];
            if (menu.parentId == 0 && menu.type == TradeMenu.TradeMenuEnum_Trade && TradeDataModel.Instance.IsOpenMenu(menu))
            {
                list.Add(menu);
            }
        }
        /*
        list.Sort(delegate(TradeMenu x, TradeMenu y)
        {
            return -x.id.CompareTo(y.id);
        });
        */
        for (int i = 0, len = list.Count; i < len; i++)
        {
            TradeMenu menu = list[i];
            GameObject go = NGUITools.AddChild(_view.RankingGrid.gameObject, prefab);
            go.name = string.Format("{0}", menu.id);

            RankingTypeCellController typeCell = go.AddMissingComponent<RankingTypeCellController>();
            typeCell.InitView();
            typeCell.SetData(menu);

            //	默认选中第一个
            //if (typeCellPrefab == null)
            //{
            //    typeCellPrefab = typeCell;
            //}
        }

        _view.RankingGrid.Reposition();
    }

    private void AddRankingTypeCell()
    {
        GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab(RankingTypeCellName) as GameObject;
        GameObject module = GameObjectExt.AddChild(_view.RankingGrid.gameObject, prefab);
        RankingTypeCellController cell = module.GetMissingComponent<RankingTypeCellController>();
        cell.InitView();
        _view.RankingGrid.Reposition();
    }

    private void OnCloseBtn()
    {
        ProxyRankingModule.Close();
    }

    public void Dispose()
    {

    }
}