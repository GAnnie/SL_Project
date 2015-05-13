using UnityEngine;
using System.Collections;

public enum AniNameFlag
{
    DefaultAni = 0,
    jingli = 1,
    libi = 2,
    MaxAni,
}


public class VertexAniSet : VertexAni
{
    private Mesh[] MeshCopy;

    AniNameFlag m_curflag = AniNameFlag.DefaultAni;
	public override void Start()
    {
        base.Start();

        MeshCopy = new Mesh[m_Meshes.Length];

        for (int i = 0; i < m_Meshes.Length; i++)
        {
            MeshCopy[i] = m_Meshes[i];
        }
	}
	
	// Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void PlayAni(string aniName)
    {
        //enabled = m_AnimateAutomatically;
        
        if (aniName == "jingli")
        {
            if (m_curflag == AniNameFlag.jingli)
                return;

            m_Meshes[0] = MeshCopy[0];
            m_Meshes[1] = MeshCopy[1];
            m_Meshes[2] = MeshCopy[2];

            m_AnimateAutomatically = true;
            m_OneLoopLength = 0.5f;
            m_WrapMode = WrapMode.Clamp;
            m_AutomaticTime = 0;
            m_curflag = AniNameFlag.jingli;
           
        }
        else if(aniName == "libi")
        {
            if ((m_curflag != AniNameFlag.jingli) && (m_curflag == AniNameFlag.libi))            //敬礼后的动作
            {
                return;
            }


            m_Meshes[0] = MeshCopy[2];
            m_Meshes[1] = MeshCopy[1];
            m_Meshes[2] = MeshCopy[0];


            m_AnimateAutomatically = true;
            m_OneLoopLength = 0.6f;
            m_WrapMode = WrapMode.Clamp;
            m_AutomaticTime = 0;
            m_curflag = AniNameFlag.libi;
        }


        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = m_Meshes[0];
        m_Mesh = filter.mesh;
    }
}
