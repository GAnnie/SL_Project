using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour
{
	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.3f;

	public int drawDepth = -1000;
	
	public float alpha = 1.0f;
	public float fadeDir = -1.0f;
	
	public delegate void FinishFadeEffect();
	public FinishFadeEffect finishFadeEffect;
	
	public Color fadeColor = Color.black;
	public float fadeAlpha = 1.0f;
	
	public Color textColor;
	
	private GUIStyle style; // The style the text will be displayed at, based en defaultSkin.label.
	public string tip = "";

    public bool _playing;
	
	void Start()
	{
		textColor = Color.white;
		
		if( style == null ){
			style = new GUIStyle();
			style.fontSize = 28;
			style.normal.textColor = textColor;
			style.alignment = TextAnchor.MiddleCenter;
		}		
		
	    //FadeIn();
	}

#if UNITY_DEBUG
    void OnGUI()
    {
        if (_playing == false)
        {
            alpha = 0;
            return;
        }

	    alpha += fadeDir * fadeSpeed * Time.deltaTime; 
		alpha = Mathf.Clamp(alpha, 0, fadeAlpha);
	   
		GUI.color = new  Color( fadeColor.r, fadeColor.g, fadeColor.b, alpha );
	    GUI.depth = drawDepth;
	   
	    GUI.DrawTexture( new Rect( 0.0f, 0.0f, Screen.width+1, Screen.height ), fadeOutTexture);
		
		GUI.color = textColor;
		
		GUI.Label( new Rect(0, 0, Screen.width, Screen.height), tip, style );
		
		if (fadeDir > 0){
			if (alpha >= 1){
				CallFinishFadeEffect();
			}
		}else{
			if (alpha <= 0){
				CallFinishFadeEffect();
                Stop();
			}
		}
	}
#endif	

	private void CallFinishFadeEffect()
	{
		if (finishFadeEffect != null){
			finishFadeEffect();
			finishFadeEffect = null;
		}
	}
	
	//--------------------------------------------------------------------
	
	public void FadeIn()
	{
        _playing = true;

        alpha = 1;
	    fadeDir = -1.0f;
		tip = "";
	}
	
	//--------------------------------------------------------------------
	
	public void FadeOut()
	{
        _playing = true;

        alpha = 0;
	    fadeDir = 1.0f;   
	}

    public void Stop()
    {
        _playing = false;
    }

}
