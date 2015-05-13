// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MiniWorldMapViewController.cs
// Author   : willson
// Created  : 2015/4/17
// Porpuse  : 
// **********************************************************************

using com.nucleus.h1.logic.core.modules.scene.data;
using System.Collections.Generic;
using UnityEngine;
public class MiniWorldMapViewController : MonoBehaviourBase, IViewController
{
    private MiniWorldMapView _view;
    private Dictionary<int, GameObject> _placeMap;
    public void InitView()
    {
        _placeMap = new Dictionary<int, GameObject>();

        _view = gameObject.GetMissingComponent<MiniWorldMapView>();
        _view.Setup(this.transform);
        _view.GoHomeBtn.gameObject.SetActive(false);
        RegisterEvent();
    }

    public void RegisterEvent()
    {
        EventDelegate.Set(_view.CloseBtn.onClick, OnCloseBtn);
        EventDelegate.Set(_view.BackMyFactionBtn.onClick, OnBackMyFactionBtn);
        EventDelegate.Set(_view.CurrMapBtn.onClick, OnCurrMapBtn);
    }

    public void Open()
    {
        _view.CurrMapBtnLbl.text = WorldManager.Instance.GetModel().GetSceneDto().name;

        List<SceneMap> maps = DataCache.getArrayByClsWithoutSort<SceneMap>();
        for(int index = 0; index < maps.Count; index++)
        {
            SceneMap map = maps[index];
            Transform maptf = _view.PlaceGroup.Find("Place_" + map.id);
            if(maptf != null && maptf.GetComponent<UIButton>() != null)
            {
                _placeMap.Add(map.id, maptf.GetComponent<UIButton>().gameObject);
                EventDelegate.Set(maptf.GetComponent<UIButton>().onClick, delegate()
                {
                    //Debug.Log("################ " + UIButton.current.name);
                    WorldManager.Instance.Enter(map.id, false);
                    OnCloseBtn();
                });
            }
        }

        // 设置我当前的位置
        int sceneId = WorldManager.Instance.GetModel().GetSceneId();
        if (_placeMap.ContainsKey(sceneId))
        {
            GameObject btn = _placeMap[sceneId];
            _view.HeroSprite.SetActive(true);
            Vector3 heroPosition = new Vector3(btn.transform.localPosition.x, btn.transform.localPosition.y - 25f, btn.transform.localPosition.z);
            _view.HeroSprite.transform.localPosition = heroPosition;
        }
        else
        {
            _view.HeroSprite.SetActive(false);
        }
    }

    private float _timer = 0f;

    void Update()
    {
        if (_view.HeroSprite.activeSelf)
        {
            _timer += Time.deltaTime;
            if (_timer > 0.3f)
            {
                _timer = 0f;
                _view.HeroSprite.transform.localEulerAngles = new Vector3(0, _view.HeroSprite.transform.localEulerAngles.y + 180, 0);
            }
        }
    }

    private void OnBackMyFactionBtn()
    {
        WorldManager.Instance.Enter(PlayerModel.Instance.GetPlayer().faction.factionSceneId, false);
        OnCloseBtn();
    }

    private void OnCurrMapBtn()
    {
        OnCloseBtn();
        ProxyWorldMapModule.OpenMiniMap();
    }

    private void OnCloseBtn()
    {
        ProxyWorldMapModule.CloseMiniWorldMap();
    }

    public void Dispose()
    {

    }
}