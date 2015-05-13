// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SceneAssetsCacheController.cs
// Author   : wenlin
// Created  : 2013/6/20 12:01:42
// Purpose  : 用于处理在场景中跳转的时候。 缓存已存在的资源目录信息。 并且在下一个场景归位原来的场景
//            根据SceneMessage ,分析场景Grass 和 Tree的下的物体是否可重用
//            有一份数据， 记录当前场景节点Grass 和 Tree 的数据
//            根据下一个SceneMessage去把 当前的物件传入 CacheAssets里面
//            下一个场景开始还原场景物品的时候。 会先判断是否在CacheAssets存在这个资源。 如果没有返回NULL
//            清空CacheAssets资源
//            重新记录当前场景Grass 和 Tree节点信息
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssetsCacheController
{
    #region Struct of CacheObj
    private struct CacheObj
    {
        public UnityEngine.Object asset;

        public bool cleanFlag;

        public CacheObj(UnityEngine.Object asset)
        {
            this.asset = asset;

            this.cleanFlag = false;
        }

        public void SetCleanFlag( bool flag )
        {
            cleanFlag = flag;
        }

        public void Dispose()
        {
            this.asset = null;
        }
    }
    #endregion

    //进行缓存的物体的引用
    Dictionary<string, CacheObj> _cacheGameObjectReferenceDic = null;

    public SceneAssetsCacheController()
    {
        //缓存引用
        _cacheGameObjectReferenceDic = new Dictionary<string, CacheObj>();
    }

    /// <summary>
    /// 添加当前场景资源的引用
    /// </summary>
    /// <param name="goName"></param>
    /// <param name="go"></param>
    public void AddCacheObjectReference(string assetName, UnityEngine.Object asset)
    {
        if (_cacheGameObjectReferenceDic == null) return;

        if (!_cacheGameObjectReferenceDic.ContainsKey(assetName))
        {
            _cacheGameObjectReferenceDic.Add(assetName, new CacheObj( asset ));
        }
    }

    /// <summary>
    /// 获取场景缓存的资源引用
    /// </summary>
    /// <param name="goName"></param>
    /// <returns></returns>
    public UnityEngine.Object GetCacheGameObject(string assetName)
    {
        if (_cacheGameObjectReferenceDic == null) return null;

        //取消(Clone)的标示
        if (_cacheGameObjectReferenceDic.ContainsKey(assetName))
        {
            return _cacheGameObjectReferenceDic[assetName].asset;
        }
        return null;
    }

    /// <summary>
    /// 进行场景资源缓存
    /// </summary>
    /// <param name="set"></param>
    public void CacheSceneObject(SceneObjectsInfoSet set)
    {
        if (set == null)
        {
            GameDebuger.Log("SceneAssetsCacheController : SceneObjectsInfoSet is null ");
            return;
        }

        _ResetCacheObjectReference();

        foreach (KeyValuePair<string, SceneNode> item in set.sceneNodeDic)
        {
            _AnalyCache(item.Value);
        }

        _CleanCacheObjectReference();
    }

    /// <summary>
    /// 具体缓存场景数据
    /// </summary>
    /// <param name="node"></param>
    private void _AnalyCache(SceneNode node)
    {
        foreach (KeyValuePair<string, SceneObjectData> item in node.sceneObjectDataDic)
        {
            SceneObjectData data = item.Value;

            if (_cacheGameObjectReferenceDic.ContainsKey(data.objectName))
            {
                _cacheGameObjectReferenceDic[data.objectName].SetCleanFlag(false);
            }
        }
    }

    /// <summary>
    /// 重置缓存标记
    /// </summary>
    private void _ResetCacheObjectReference()
    {
        if (_cacheGameObjectReferenceDic != null)
        {
            foreach (KeyValuePair<string, CacheObj> item in _cacheGameObjectReferenceDic)
            {
                item.Value.SetCleanFlag(true);            
            }
        }
    }


    /// <summary>
    /// 清空缓冲场景资源的引用
    /// </summary>
    private void _CleanCacheObjectReference()
    {
        if (_cacheGameObjectReferenceDic != null)
        {
            List<string> deleteList = new List<string>();

            foreach (KeyValuePair<string, CacheObj> item in _cacheGameObjectReferenceDic)
            {
                if (item.Value.cleanFlag)
                {
                    deleteList.Add(item.Key);
                }
            }

            foreach (string deleteStr in deleteList)
            {
                CacheObj cacheObj = _cacheGameObjectReferenceDic[deleteStr];
                _cacheGameObjectReferenceDic.Remove(deleteStr);
                cacheObj.Dispose();
            }
        }
        //Resources.UnloadUnusedAssets();
    }

    private struct MeshRendererSet
    {
        public string materialName;

        public Material material;

        public List<GameObject> sameMaterilGoList;

        public MeshRendererSet(string materialName, Material material)
        {
            this.materialName = materialName;

            this.material = material;

            sameMaterilGoList = new List<GameObject>();
        }
    }


    private Dictionary<string, MeshRendererSet> _sameMaterialObjDic = new Dictionary<string, MeshRendererSet>();

    /// 清除StaticBatching的缓存
    public void CleanStaticBatchingCache()
    {
        _sameMaterialObjDic.Clear();
    }

    /// 收集MeshRenderer的信息， 用于把相同材质的GO合拼在一起
    /// <param name="meshRendererList"></param>
    public void StaticBatchingSelect(List<MeshRenderer> meshRendererList)
    {
        if (meshRendererList != null)
        {
            for (int i = 0; i < meshRendererList.Count; i++)
            {
                MeshRenderer meshRenderer = meshRendererList[i];

                Material material  = meshRenderer.sharedMaterial;
                if( material == null ) continue;

                string materilName = material.name;
                if (!_sameMaterialObjDic.ContainsKey(materilName))
                {
                    _sameMaterialObjDic.Add(materilName, new MeshRendererSet(materilName, material)); 
                }
                MeshRendererSet set = _sameMaterialObjDic[materilName];

                if (set.material != material)
                {
                    meshRenderer.sharedMaterial = set.material;
                }
                set.sameMaterilGoList.Add(meshRenderer.gameObject);
            }
        }
    }


    /// 合拼相同的材质的GO
    /// <param name="root"></param>
    public void StaticBatchingCombine(Transform rootTransform)
    {
        if (rootTransform == null) return;

        foreach (KeyValuePair<string, MeshRendererSet> item in _sameMaterialObjDic)
        {
            MeshRendererSet set = item.Value;
            GameObject parentGO = new GameObject(set.materialName);
            parentGO.transform.parent = rootTransform;

            StaticBatchingUtility.Combine(set.sameMaterilGoList.ToArray(), parentGO);
        }

        CleanStaticBatchingCache();
    }
 }


