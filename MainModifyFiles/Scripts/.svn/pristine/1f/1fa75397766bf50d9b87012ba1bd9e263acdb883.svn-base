using UnityEngine;
using System.Collections;

public class WaterShaderAnalyzer : MonoBehaviour 
{
	void Start () 
    {
        MeshRenderer meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        if ( meshRenderer != null)
        {
            //如果当前手机不支持法相效果， 则替换其他材质
            if (!meshRenderer.sharedMaterial.shader.isSupported)
            {
                ResourceLoader.LoadAsync("ArtResources/Models/Scene/Object/SkyBox/Materials/water_000", delegate(AssetBase assetBase)
                {
                    if (assetBase != null && assetBase.asset != null)
                    {
                        Material waterMaterial = assetBase.asset as Material;
                        meshRenderer.material = waterMaterial;
                    }
                    else
                    {
                        GameDebuger.Log("Get Water_000 Material Error !!");
                    }
                });
            }
        }
        else
        {
            GameDebuger.Log("Can not found MeshRenderer !!! ");
        } 
	}
}
