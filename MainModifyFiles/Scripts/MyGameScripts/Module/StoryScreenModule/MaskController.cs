using UnityEngine;
using System.Collections;

public class MaskController : MonoBehaviour,IViewController {

	private ScreenMask _view;
	ScreenMaskInst inst;
	public void InitView(){
		_view = gameObject.GetMissingComponent<ScreenMask> ();
		_view.Setup (this.transform);

	}

	public void RegisterEvent(){

	}
	public  void Dispose(){

	}
	void Update(){

		if (fadeIn) {
			if(fadeInTime < inst.fadeTime){
				alpha += Time.deltaTime * changeDlpha;
				FadeIn(alpha);
			}
			else{
				fadeIn = false;
			}
			fadeInTime += Time.deltaTime;
		}




		if (fadeOut) {
			if(fadeOutTime < inst.fadeTime){
				alpha -= Time.deltaTime *changeDlpha;
				FadeOut(alpha);
			}
			else{
				fadeOut = false;
				Close();
			}
			fadeOutTime += Time.deltaTime;
		}

		if (delay) {
			timer += Time.deltaTime;
			if(timer > inst.delayTime - inst.fadeTime){
				delay = false;
				if(inst.fade){
					fadeOut = true;
//					TipManager.AddTip("淡出");
				}
				else{
//					TipManager.AddTip("不是淡出");
					Close();
				}
			}
		}

	}

	
	public void Close(){
		ProxyScreenMaskModule.Close();
	}



	
	float fadeOutTime;
	float fadeInTime;
	bool fadeIn =false;
	bool fadeOut  = false;
	float alpha;
	float changeDlpha;
	float fadeTime;
	bool delay = false;
	float timer;


	public void SetData(ScreenMaskInst screenMaskInst){
		inst = screenMaskInst;
		alpha = 0;
		changeDlpha = inst.alpha / inst.fadeTime;
		fadeTime = screenMaskInst.fadeTime;
		if (inst.fade) {
			fadeIn = true;
//			TipManager.AddTip("淡入");	
		}
		else{
			Normal();
//			TipManager.AddTip("不是淡入");
		}

	}


	//淡入
	public void FadeIn(float a){
		_view.MaskSprite.color = new Color (inst.colorR/255.0f, inst.colorG/255.0f, inst.colorB/255.0f,a/255.0f);
	}

	public void Normal(){
		_view.MaskSprite.color = new Color (inst.colorR/255.0f, inst.colorG/255.0f, inst.colorB/255.0f,inst.alpha/255.0f);
	}



	public void ShowMask(){
		delay =  true;
	}


	//FadeOut  淡出
	public void  FadeOut(float a){
		_view.MaskSprite.color = new Color (inst.colorR/255.0f, inst.colorG/255.0f, inst.colorB/255.0f,a/255.0f);
	}

}
