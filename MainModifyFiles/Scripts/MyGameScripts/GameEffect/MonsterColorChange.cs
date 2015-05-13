using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SAttributeState : byte
{
    Normal = 1,
    Wrong = 2,//混乱
    NotMove = 4,//不能移动
    Still = 8,//晕倒
    NoSay = 16,//沉默
}

public class SAStruct
{
    public SAttributeState sas;
    public float time;

}
public class MonsterColorChange : MonoBehaviour {
    private List<Color> oldColor = new List<Color>();
    public Renderer rend;
    private bool isNeedChange;
    private float curTime;
    private float maxTime = 0.2f;

    public int curAttrState = (int)SAttributeState.Normal;
    


	// Use this for initialization
	void Start () {
	    //oldColor

        Material[] ms = rend.materials;
        foreach (Material m in ms)
        {
            if( m.HasProperty("color"))
                oldColor.Add(m.color);
        }

        curAttrState = (int)SAttributeState.Normal;
	}

    void Update()
    {
        if (isNeedChange)
        {
            curTime += Time.deltaTime;
            if (curTime >= maxTime)
            {
                int i = 0;
                foreach (Material m in rend.materials)
                {
                    if( m.HasProperty("color") )
                    {
                        m.color = oldColor[i];
                        i++;
                    }
                }
                isNeedChange = false;
            }
        }
        
    }

    void changeColor(Color col)
    {
        isNeedChange = true;
        curTime = 0.0f;
        foreach (Material m in rend.materials)
        {
            if( m.HasProperty("color") )
                m.color = col;
        }
    }

    void changeStill(float time)
    {   
        
        this.gameObject.animation.Stop();
        
    }

    void changeNPCOutLine(float alpha)
    {
        Material[] ms = rend.materials;
        Color mycolor = Color.white;
        mycolor.a = alpha;
        foreach (Material m in ms)
        {
            m.SetColor("_OutlineColor", mycolor);
        }
    }
    void changeOutLine( float alpha)
    {
        Material[] ms = rend.materials;
        Color mycolor = Color.red;
        mycolor.a = alpha;
        foreach (Material m in ms)
        {
            m.SetColor("_OutlineColor", mycolor);
        }
    }
}
