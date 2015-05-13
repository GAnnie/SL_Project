using UnityEngine;
using System.Collections;

public class ShadowMotionGenerator : MonoBehaviour
{
	private GameObject shadowMotionPrefab;
	
	public GameObject petPrefab;

	private float generateInterval = 0.05f;
	private float duration;
	private float scale;
	private Color color;
	private float alpha;
	private ShadowMotion.BlendMode blendMode;
	private Shader shader;
		
	private Transform myTransform;
	
	private float lifeTime;
	
	private bool isRunning = false;
	
	private float yRotation = 0.0f;
	// Use this for initialization
	void Start ()
	{
	}
	
	public void Init( GameObject _petPrefab, float _time, float _scale, string colorStr, float _alpha, string _blendMode, float _generateInterval, float _lifeTime, float _yRotation )
	{
		if ( _petPrefab == null )
			return;
		
		petPrefab = _petPrefab;
		
		shadowMotionPrefab = ResourceLoader.Load( PathHelper.SHADOW_MOTION_PATH ) as GameObject;
		if ( shadowMotionPrefab == null )
			return;
		
		myTransform = transform;
		
		duration = _time;
		
		if ( _blendMode == "add" )
		{
			blendMode = ShadowMotion.BlendMode.Additive;
			shader = Shader.Find("Particles/Additive");
		}
		else
		{
			blendMode = ShadowMotion.BlendMode.Normal;
			shader = Shader.Find("Baoyu/Baoyu_Alpha");
		}
		
		color = ColorExt.HexToColor(colorStr);
		alpha = _alpha;
		scale = _scale;
		
		generateInterval = _generateInterval;
		lifeTime = _lifeTime;
		
		yRotation = _yRotation;
		
		StopGenerate();
	}
		
	// Update is called once per frame
	void Update ()
	{
		if ( !isRunning )
			return;
		
		lifeTime -= Time.deltaTime;
		
		if ( lifeTime <= 0 )
		{
			StopGenerate();
			isRunning = false;
		}
	}
	
	public void Run()
	{
		if ( petPrefab == null )
			return;

		if ( shadowMotionPrefab == null )
			return;
		
		
		isRunning = true;
		if (this.gameObject.activeInHierarchy){
			StartCoroutine( Generate() );	
		}
	}
	
	public void StopGenerate()
	{
		isRunning = false;
	}
	
	IEnumerator Generate()
	{
		while ( isRunning )
		{
			GameObject obj = Instantiate( shadowMotionPrefab, myTransform.position, myTransform.localRotation ) as GameObject;
			Transform objTransform = obj.transform;
			objTransform.localPosition = myTransform.localPosition;
			objTransform.localScale = myTransform.localScale;
			objTransform.localRotation = petPrefab.transform.localRotation;
			
			ShadowMotion sm = obj.GetComponent< ShadowMotion >();
			sm.Init( petPrefab, duration, scale, color, alpha, blendMode, shader, yRotation );
			SkinnedMeshRenderer meshRenderer = sm.GetComponentInChildren< SkinnedMeshRenderer >();
			meshRenderer.enabled = true;
			
			yield return new WaitForSeconds( generateInterval );
		}
		
		yield return null;
	}
	
	Color GetColorFromIndex( int index )
	{
		if ( index == 0 )
			return Color.white;
		else if ( index == 1 )
			return Color.red;
		else if ( index == 2 )
			return Color.green;
		else if ( index == 3 )
			return Color.blue;
		else if ( index == 4 )
			return Color.yellow;
		else if ( index == 5 )
			return Color.cyan;
		else if ( index == 6 )
			return Color.magenta;
		
		return Color.white;
	}
}
