using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class BattleArrow : MonoBehaviour
{
//	[System.Serializable]
	public Vector3[] positions;

	//------------------------------------------------------------------------------------------------
	//public MyPositions Positions;
	
	public float startWidth = 1;
	
	private Vector3[] allVertices = new Vector3[0];
	private Vector2[] allUVs = new Vector2[0];
	private int[] allTriangles = new int[0];
	
	public Mesh mesh;
	
	private MeshRenderer meshRenderer;
	
	public bool running = false;
	
	void Awake()
	{
		MeshFilter mF = transform.GetComponent<MeshFilter>();
		if(mF)
			mF.sharedMesh = null;
			
		mesh = null;
	
		if ( !gameObject.GetComponent("MeshFilter") )
			gameObject.AddComponent("MeshFilter");
		
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		if ( meshRenderer == null )
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
		
       	if(!mesh)
			mesh = GetComponent<MeshFilter>().sharedMesh;
		
		if(!mesh)
		{
			mesh = new Mesh();
			mesh.name = "LineMesh" + gameObject.name;
			GetComponent<MeshFilter>().sharedMesh = mesh;
		}
		
		//RecalculateMesh();
	}
	
	public void RecalculateMesh()
	{
		if ( positions == null )
			return;
		
		int pointCount = positions.Length;
		
		if ( pointCount < 2 )
			return;
		
		allVertices = new Vector3[ pointCount * 2 ];
		allUVs = new Vector2[pointCount * 2 ];
		allTriangles = new int[ pointCount * 6 ];

		if ( !gameObject.GetComponent("MeshFilter") )
			gameObject.AddComponent("MeshFilter");
		
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		if ( meshRenderer == null )
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
		
       	if(!mesh)
			mesh = GetComponent<MeshFilter>().sharedMesh;
		
		if(!mesh)
		{
			mesh = new Mesh();
			mesh.name = "LineMesh" + gameObject.name;
			GetComponent<MeshFilter>().sharedMesh = mesh;
		}
        mesh.Clear();
		
		float halfWidth = startWidth / 2.0f;
		
		Vector3 lastPoint1 = new Vector3();
		Vector3 lastPoint2 = new Vector3();
		
		for( int i = 0; i < pointCount - 1; ++i)
		{
			Vector3 point = positions[ i ] - gameObject.transform.position;
			Vector3 nextPoint = positions[ i + 1 ] - gameObject.transform.position;
			Vector3 delta = nextPoint - point;
			float angle = Mathf.Atan2( delta.z, delta.x  ) * Mathf.Rad2Deg;
			Quaternion quat = Quaternion.AngleAxis( angle, -Vector3.up );
			Vector3 tmpVertice = quat * new Vector3(  0, 0, halfWidth);
			
			if ( i != 0 )
			{
				allVertices[ 2 * i ] = lastPoint1;
				allVertices[ 2 * i + 1 ] = lastPoint2;
				lastPoint1 = nextPoint + tmpVertice;
				lastPoint2 = nextPoint - tmpVertice;
			}
			else
			{
				allVertices[ 2 * i ] = point + tmpVertice;
				allVertices[ 2 * i + 1 ] = point - tmpVertice;
				lastPoint1 = nextPoint + tmpVertice;
				lastPoint2 = nextPoint - tmpVertice;
			}
			
			Vector2 uv1 = new Vector2( i / (float)pointCount, 0 );
			Vector2 uv2 = new Vector2( i / (float)pointCount, 1 ); 
			allUVs[ 2 * i ] = uv1;
			allUVs[ 2 * i + 1 ] = uv2;
			
			int baseIdx = i * 2 ;
			
			allTriangles[i * 6] = baseIdx;
			allTriangles[i * 6 + 1] = baseIdx + 1; 
			allTriangles[i * 6 + 2] = baseIdx + 2;
			allTriangles[i * 6 + 3] = baseIdx + 2;
			allTriangles[i * 6 + 4] = baseIdx + 1;
			allTriangles[i * 6 + 5] = baseIdx + 3;
			
			if ( i == pointCount - 2 )
			{
				int idx = i + 1;
				
				allVertices[ 2 * idx   ] = lastPoint1;
				allVertices[ 2 * idx + 1 ] = lastPoint2;
				allUVs[ 2 * idx  ] = new Vector2( 1, 0 );
				allUVs[ 2 * idx + 1  ] = new Vector2( 1, 1 ); 
			}
		}
	
        mesh.vertices = allVertices;
        mesh.uv = allUVs;
        mesh.triangles = allTriangles;
	}
	
	void Update()
	{
		if ( !running )
			return;
		
		if ( mesh == null )
			return;
		
		if ( meshRenderer == null )
			return;
		
		RecalculateMesh();
	}
	
	public void SetWidth( float width )
	{
		startWidth = width;
	}
	
	public Material GetMaterial()
	{
		if ( meshRenderer == null )
			return null;
		
		return meshRenderer.material;
	}
	
	public void SetVertexCount( int c )
	{
		positions = new Vector3[c];
	}
	
	public void SetPosition( int idx, Vector3 pos )
	{
		positions[idx] = pos;
	}
	
}
