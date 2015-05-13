using System;
using UnityEngine;

public class SceneU3D : U3DDataBase
{
    private static int MAX_LIMIT = 5;
    private AssetBundle _assetbundle = null;    
    private SceneObjectsInfoSet _set = null;

    public SceneU3D(string bundleName, AssetBundle assetbundle, SceneObjectsInfoSet set)
    {
        maxLimit     = MAX_LIMIT;
        _assetbundle = assetbundle;
        _set         = set;
    }

    public AssetBundle assetbundle      { get { return _assetbundle; } }
    public SceneObjectsInfoSet infoSet  { get { return _set; } }

    public override void Unload()
    {
        if (_assetbundle != null)
        {
            _assetbundle.Unload(false);
            _assetbundle = null;

            GameDebuger.Log("Unload SceneU3D ");
        }
    }

    public override int AssetSize()
    {
//#if UNITY_DEBUG
//        return Profiler.GetRuntimeMemorySize(assetbundle);
//#endif

        return 0;
    }

}
