using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class CircleProgressBar : MonoBehaviour
{
	private int elements = 8; //how many quads should the full circle consist of?
	public float radius = 50f; //Radius of the circle
	
    //private int savedElements = 0;    //GET WARNING
    //private float savedRadius = 0f;   //GET WARNING
    //private float savedPercent = 0f;  //GET WARNING
	
	private Vector3[] allVertices = new Vector3[0];
	private Vector2[] allUVs = new Vector2[0];
	private int[] allTriangles = new int[0];
	
	private Vector2 uv1 = new Vector2(0.5f, 0.5f);
    //private Vector2 uv2 = new Vector2(1f, 1f);        //GET WARNING
	
	public Mesh mesh;
	public bool createNewMeshInAwake = true;
	
    //private bool busy = false;        //GET WARNING
	
	public float percent = 0.0f;
	
	void Awake()
	{
		if(createNewMeshInAwake)
		{
			MeshFilter mF = transform.GetComponent<MeshFilter>();
			if(mF)
				mF.sharedMesh = null;
			
			mesh = null;
			
			RecalculateMesh();
		}
	}
	
	public void RecalculateMesh()
	{
		if ( percent > 1 )
			percent = 1;
		else if ( percent < 0 )
			percent = 0;
		
        //savedPercent = percent;       //GET WARNING

		if(elements <= 2)
		{
			Debug.LogWarning("Number of elements can't be < 3", gameObject);
			elements = 3;
		}
		
		float degreeStep = 360f / elements;

		float endAngle = percent * 360.0f;
		int loopCount = (int) ( endAngle / degreeStep ) + 1;
		
		int assignment = elements + 1;
		
		allVertices = new Vector3[ assignment + 1];
		allUVs = new Vector2[assignment + 1];
		allTriangles = new int[assignment * 6];			

		if ( !gameObject.GetComponent("MeshFilter") )
			gameObject.AddComponent("MeshFilter");
		
		if ( !gameObject.GetComponent("MeshRenderer") )
			gameObject.AddComponent("MeshRenderer");
		
       	if(!mesh) mesh = GetComponent<MeshFilter>().sharedMesh;
		if(!mesh)
		{
			mesh = new Mesh();
			mesh.name = "CircleProgressMesh" + gameObject.name;
			GetComponent<MeshFilter>().sharedMesh = mesh;
		}
        mesh.Clear();
	
		allVertices[0] = Vector3.zero;
		allUVs[0] = uv1;
		
		float deg = 0f;

		Quaternion quat = Quaternion.identity;
		
		int i = 1;
		
		for( i = 1; i <= loopCount; ++i)
		{
			quat = Quaternion.AngleAxis(deg, -Vector3.forward);
			
			Vector3 tmpVertice = quat * new Vector3(0f, radius * 2, 0f);
			
			float x = tmpVertice.x;
			float y = tmpVertice.y;
			
			if ( x > radius )
				x = radius;
			else if ( x < -radius )
				x = -radius;
			if ( y > radius )
				y = radius;
			else if ( y < -radius )
				y = -radius;
			
			tmpVertice = new Vector3( x, y, 0 );
			allVertices[i] = tmpVertice;
			
			tmpVertice /= radius * 2.0f;
			tmpVertice += new Vector3( 0.5f, 0.5f, 0 );
			Vector2 tmpUV = new Vector2( tmpVertice.x, tmpVertice.y );
			allUVs[i] = tmpUV;
			
			allTriangles[(i-1) * 3] = 0;
			allTriangles[(i-1) * 3 + 1] = i; 
			
			if( i <= elements )
				allTriangles[(i-1) * 3 + 2] = i + 1;
			else
				allTriangles[(i-1) * 3 + 2] = 1;
			
			deg += degreeStep;
		}
		
		if ( endAngle > ( loopCount - 1 ) * degreeStep )
		{
			quat = Quaternion.AngleAxis( endAngle, -Vector3.forward);
			
			Vector3 tmpVertice = quat * new Vector3(0f, radius * 2, 0f);
			
			float x = tmpVertice.x;
			float y = tmpVertice.y;
			
			if ( x > radius )
				x = radius;
			else if ( x < -radius )
				x = -radius;
			if ( y > radius )
				y = radius;
			else if ( y < -radius )
				y = -radius;
			
			tmpVertice = new Vector3( x, y, 0 );
			allVertices[i] = tmpVertice;
			
			tmpVertice /= radius * 2.0f;
			tmpVertice += new Vector3( 0.5f, 0.5f, 0 );
			
			Vector2 tmpUV = new Vector2( tmpVertice.x, tmpVertice.y );
				
			allUVs[i] = tmpUV;
			allTriangles[(i-1) * 3 + 2] = i; 
		}
	
        mesh.vertices = allVertices;
        mesh.uv = allUVs;
        mesh.triangles = allTriangles;
	}
}
