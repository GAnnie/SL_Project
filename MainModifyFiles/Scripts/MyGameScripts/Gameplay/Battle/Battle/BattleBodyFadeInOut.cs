using UnityEngine;
using System.Collections;

public class BattleBodyFadeInOut : MonoBehaviour
{
	public float alpha = 1.0f;
	public bool running = false;
	private Material petMaterial = null;
	private float direction = 1.0f;
	private SkinnedMeshRenderer meshRenderer = null;
	
	public delegate void OnFadeEffectFnishDelegate();
	public OnFadeEffectFnishDelegate onFadeEffectFnishDelegate;	
	
	private Shader originShader; //模型原shader
	
	public void Init( Material _petMaterial, SkinnedMeshRenderer mr )
	{
		petMaterial = _petMaterial;
		originShader = petMaterial.shader;
		meshRenderer = mr;
	}
	
	public void FadeIn()
	{
		meshRenderer.enabled = true;
		alpha = 0.00001f;
		direction = 1.0f;
	}
	
	public void FadeOut()
	{
		meshRenderer.enabled = true;
		alpha = 0.9999999f;
		direction = -1.0f;
	}
	
	public void Pause()
	{
		running = false;
	}
	
	public void Run()
	{
		if ( petMaterial == null )
			return;
		
		Shader shader = Shader.Find("Baoyu/Baoyu_Alpha");
		if( shader != null )
			petMaterial.shader = shader;
		
		petMaterial.SetColor("_Color", new Color( 1,1,1, alpha ) );
		
		running = true;
	}

	public bool isRunning()
	{
		return running;
	}

	void Update ()
	{
		if ( !running )
			return;
		
		if ( petMaterial ==  null || meshRenderer == null){
			running = false;
			return;
		}		
		
		alpha += direction * Time.deltaTime;
		
		if ( alpha < 0 )
		{
			alpha = 0;
			running = false;
			
			if (onFadeEffectFnishDelegate != null){
				onFadeEffectFnishDelegate();
				onFadeEffectFnishDelegate = null;
			}
		}
		else if ( alpha > 1 )
		{
			alpha = 1;
			
			meshRenderer.enabled = true;
			petMaterial.shader = originShader;
			
			running = false;
			
			if (onFadeEffectFnishDelegate != null){
				onFadeEffectFnishDelegate();
				onFadeEffectFnishDelegate = null;
			}			
		}
		
		if ( petMaterial ==  null || meshRenderer == null){
			running = false;
			return;
		}
		
		petMaterial.SetColor("_Color", new Color( 1,1,1, alpha ) );
	}
	
	public void Destroy(){
		if (petMaterial != null){
			meshRenderer.enabled = true;
			petMaterial.shader = originShader;
			petMaterial = null;
		}
	}
}
