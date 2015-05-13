using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class VertexAni : MonoBehaviour 
{
/// REALLY IMPORTANT NOTE.
/// When using the mesh morpher you should absolutely make sure that you turn
/// off generate normals automatically in the importer, or set the normal angle to 180 degrees.
/// When importing a mesh Unity automatically splits vertices based on normal creases.
/// However the mesh morpher requires that you use the same amount of vertices for each mesh and that
/// those vertices are laid out in the exact same way. Thus it won't work if Unity autosplits vertices based on normals.
    public Mesh[] m_Meshes;
    public bool m_AnimateAutomatically = true;
    public float m_OneLoopLength = 1.0f;   /// The time it takes for one loop to complete
    public WrapMode m_WrapMode = WrapMode.Loop;
    protected float m_AutomaticTime = 0;

    protected int m_SrcMesh = -1;
    protected int m_DstMesh = -1;
    protected float m_Weight = -1;
    protected Mesh m_Mesh;


	// Use this for initialization
    public virtual void Start() 
    {
        enabled = m_AnimateAutomatically;
        MeshFilter filter = GetComponent<MeshFilter>();
        // Make sure all meshes are assigned!
        for (int i=0; i<m_Meshes.Length; i++) {
          if (m_Meshes[i] == null) 
          {
             //Debug.Log("MeshMorpher mesh  " + i + " has not been setup correctly");
             m_AnimateAutomatically = false;
             return;
          }
        }

        // At least two meshes
        if (m_Meshes.Length < 2)
        {
          //Debug.Log ("The mesh morpher needs at least 2 source meshes");
          m_AnimateAutomatically = false;
          return;
        }

        filter.sharedMesh = m_Meshes[0];
        m_Mesh = filter.mesh;


        //print("mesh1 : \t" + m_Meshes[0].vertexCount);
        //print("\nmesh2 : \t" + m_Meshes[1].vertexCount);
        int vertexCount = m_Mesh.vertexCount;
        for (int i=0; i<m_Meshes.Length; i++) {
          if (m_Meshes[i].vertexCount != vertexCount)
          {
             //Debug.Log("Mesh " + i + " doesn't have the same number of vertices as the first mesh");
             m_AnimateAutomatically = false;
             return;
          }
        }
	}
	
	// Update is called once per frame
	public virtual void Update () 
    {
        return;
	    if (m_AnimateAutomatically) 
        {
            float deltaTime = Time.deltaTime * (m_Meshes.Length - 1) / m_OneLoopLength;
            m_AutomaticTime += deltaTime;
            float time;
            if (m_WrapMode == WrapMode.Loop)
                time = Mathf.Repeat(m_AutomaticTime, m_Meshes.Length - 1);
            else if (m_WrapMode == WrapMode.PingPong)
                time = Mathf.PingPong(m_AutomaticTime, m_Meshes.Length - 1);
            else
                time = Mathf.Clamp(m_AutomaticTime, 0, m_Meshes.Length - 1);

            SetMorph (time);
        }
	}


    /// Set the current morph in
    void SetComplexMorph (int srcIndex, int dstIndex, float t)
    {
       if (m_SrcMesh == srcIndex && m_DstMesh == dstIndex && Mathf.Approximately(m_Weight, t))
          return;
       Vector3[] v0 = m_Meshes[srcIndex].vertices;
       Vector3[] v1 = m_Meshes[dstIndex].vertices;
       Vector3[] vdst = new Vector3[m_Mesh.vertexCount];
       for (int i = 0; i < vdst.Length; i++)
          vdst[i] = Vector3.Lerp(v0[i], v1[i], t);

       m_Mesh.vertices = vdst;

       m_Mesh.RecalculateBounds();
    }

    /// t is between 0 and m_Meshes.Length - 1.
    /// 0 means the first mesh, m_Meshes.Length - 1 means the last mesh.
    /// 0.5 means half of the first mesh and half of the second mesh.
    void SetMorph (float t) 
    {
       int floor = (int)t;
       floor = Mathf.Clamp(floor, 0, m_Meshes.Length - 2);
       float fraction = t - floor;
       fraction = Mathf.Clamp(t - floor, 0.0f, 1.0f);
       SetComplexMorph (floor, floor + 1, fraction);
    }


}
