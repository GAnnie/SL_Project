using System;
using UnityEngine;
public class AssetU3D : U3DDataBase
{
    private static int MAX_LIMIT = 5;
    private AssetBase _asset = null;

    public AssetU3D(string bundleName, AssetBase asset)
    {
        maxLimit = MAX_LIMIT;

        _asset   = asset;
    }

    public AssetBase asset { get { return _asset; } }

    public override void Unload()
    {
        //GameDebuger.Log("Unload AssetU3D ");
        //Resources.UnloadAsset(_asset);
		
//		GameObject.DestroyImmediate( _asset );
        _asset.UnLoad();

    }

    public override int AssetSize()
    {
//#if UNITY_DEBUG
//        return Profiler.GetRuntimeMemorySize(_asset.asset);
//#else
//        return 
//#endif
        return 0;
    }
}
