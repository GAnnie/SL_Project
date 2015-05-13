using UnityEngine;
using System.Collections;

public class BattleCircleAnimation : MonoBehaviour
{
	static private Texture Circle1;
	static private Texture Circle2;
	public MeshRenderer meshRenderer;
	
	public enum TextureType
	{
		Circle1,
		Circle2,
	}
	
	void Awake()
	{
		if (Circle1 == null){
			Circle1 = ResourceLoader.Load( "Textures/BattleTextures/Circle1", "png" ) as Texture;	
		}
		if (Circle2 == null){
			Circle2 = ResourceLoader.Load( "Textures/BattleTextures/Circle2", "png" ) as Texture;
		}		
		
		if ( meshRenderer == null )
			meshRenderer = gameObject.GetComponentInChildren< MeshRenderer >();
	}
	
	void OnEnable()
	{
		float scaleParameter = 1.8f * 0.07f;
		transform.localScale = new Vector3( scaleParameter,scaleParameter,scaleParameter );		
		scaleParameter = 2.1f * 0.07f;
		
		if (this.GetComponent<iTween>() == null){
			iTween.ScaleTo( gameObject, iTween.Hash( "x", scaleParameter, "y", scaleParameter, "z", scaleParameter, "looptype", "pingPong", "time", 1.0f, "easetype", "linear" ) );			
		}
	}
	
	public void SetTexture( TextureType tt )
	{
		if ( meshRenderer == null )
			return;		
		
		if ( tt == TextureType.Circle1 )
			meshRenderer.material.SetTexture("_MainTex", Circle1 );
		else
			meshRenderer.material.SetTexture("_MainTex", Circle2 );
	}
}
