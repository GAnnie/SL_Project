using UnityEngine;
using System.Collections;

public class ShadowMotion : MonoBehaviour
{
	public enum BlendMode
	{
		Additive,
		Normal,
	}
	
	private SkinnedMeshRenderer meshRenderer;
	private float time;
	
	private Color color;
	private float a = 0.00001f;
	
	private float maxAlpha = 1;
	
    //private float currentScale = 1.0f;    //GET WARNING
	private float targetScale;
		
	private float direction = 1.0f;
	
	private Transform myTransform;
	
	private GameObject prefab;
	
	private bool isRunning = false;
	
	private BlendMode blendMode;
	
	// Use this for initialization
	void Start ()
	{
	}

	public void Init( GameObject petPrefab, float duration, float scale, Color _color, float _maxAlpha,
			   BlendMode _blendMode, Shader _shader, float yAngle )
	{
		myTransform = transform;
		
		blendMode = _blendMode;
			
		targetScale = scale * transform.localScale.x;
		time = duration;// * 0.5f;
		color = _color;
		
		maxAlpha = _maxAlpha;
		
		GameObject petObject = Instantiate( petPrefab, Vector3.one, Quaternion.identity ) as GameObject;
		
		Animation oldAnimation = petPrefab.GetComponent< Animation >();
		
		string animaName = oldAnimation.clip.name;
		float normalizedTime = oldAnimation[animaName].normalizedTime;
		
		Animation newAnimation = petObject.GetComponent< Animation >();
		newAnimation[animaName].normalizedTime = normalizedTime;
		
		CleanAllEffect(petObject);
		
		Transform petObjTransform = petObject.transform;
		petObjTransform.parent = myTransform;
		petObjTransform.localPosition = Vector3.zero;
		petObjTransform.localScale = petPrefab.transform.localScale;
		
		
		
		petObjTransform.localEulerAngles = new Vector3( 0, yAngle, 0 );
		
		//myTransform.localScale = new Vector3( 1,1,1 );
		myTransform.localScale = new Vector3( targetScale, targetScale, targetScale );
		
		meshRenderer = petObject.GetComponentInChildren< SkinnedMeshRenderer >();

        if (meshRenderer != null)
        {
            meshRenderer.material.shader = _shader;
            meshRenderer.material.SetColor("_TintColor", color);
        }

		
		isRunning = true;
	}
	
	private void CleanAllEffect(GameObject petObject){
		ParticleScaler[] list = petObject.GetComponentsInChildren<ParticleScaler>();
		foreach(ParticleScaler scale in list){
			GameObject.DestroyImmediate(scale.gameObject);
		}
		
//		FS_ShadowSimple shadow = petObject.GetComponentInChildren<FS_ShadowSimple>();
//		if (shadow != null){
//			GameObject.DestroyImmediate(shadow.gameObject);
//		}

		BattleShadow battleShadow = petObject.GetComponentInChildren<BattleShadow>();
		if (battleShadow != null)
		{
			GameObject.DestroyImmediate(battleShadow.gameObject);
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( !isRunning )
			return;

		time -= Time.deltaTime;
		
		if ( a <= 0 || time <= 0 )
		{
			Destroy( gameObject );
		}
		
		float myTime = 1 / time;
	
		a = a +  ( myTime * Time.deltaTime ) * direction;
		
		if ( a >= maxAlpha )
		{
			a = maxAlpha;
			direction = -1.0f;
		}
		
//		currentScale += ( targetScale - currentScale ) * myTime * Time.deltaTime;
//		
//		myTransform.localScale = new Vector3( currentScale, currentScale, currentScale );
		
		color = new Color( color.r, color.g, color.b, a );
		
		if (meshRenderer == null){
			isRunning = false;
			return;
		}
		
		if ( blendMode == BlendMode.Additive )
			meshRenderer.material.SetColor("_TintColor", color );
		else
			meshRenderer.material.SetColor("_Color", color );
	}
}
