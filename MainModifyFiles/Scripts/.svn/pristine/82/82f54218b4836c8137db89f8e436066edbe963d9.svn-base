using UnityEngine;
using System.Collections;

public class TurnOn : MonoBehaviour
{
    private Transform[] m_Children;
	// Use this for initialization
	void Start ()
    {
        m_Children = gameObject.transform.GetComponentsInChildren<Transform>();
        if (null != m_Children)
        {
            for (int i = 0; i < m_Children.Length; ++i)
            {
                if (m_Children[i] == gameObject.transform) continue;

                //m_Children[i].GetComponent<EmitController>().m_TurnOn = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	
	}

    public void TurnOnEffect()
    {
        if (null != m_Children)
        {
            for (int i = 0; i < m_Children.Length; ++i)
            {
                if(m_Children[i] == transform) continue;

                //m_Children[i].GetComponent<EmitController>().m_TurnOn = true;
                m_Children[i].GetComponent<EmitController>().doReset();
            }
        }
    }
}
