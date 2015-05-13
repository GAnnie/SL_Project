using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetMaterialOffset : MonoBehaviour
{
    public List<Material> materialList = new List<Material>();
    public bool isX;
    public bool isY;
    public float speed;

    void Update()
    {
        float xoffset = speed * Time.time;
        float yoffset = speed * Time.time;
        if (!isX) xoffset = 0;
        if (!isY) yoffset = 0;
		foreach (Material ma in materialList)
        {
           ma.SetTextureOffset("_MainTex", new Vector2(-xoffset, yoffset));
        }
    }
}