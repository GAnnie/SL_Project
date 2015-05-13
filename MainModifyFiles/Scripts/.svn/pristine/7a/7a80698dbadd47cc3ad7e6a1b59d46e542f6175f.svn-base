using UnityEngine;
using System.Collections;

public class PresureController : MonoBehaviour,IViewController {


	private ScreenPresure _view;
	ScreenPresureInst inst;
	public void InitView(){

		_view = gameObject.GetMissingComponent<ScreenPresure> ();
		_view.Setup (this.transform);

	}

	public void SetData(ScreenPresureInst screenPresureInst){
		inst = screenPresureInst;
		fadeIn = true;
//		TipManager.AddTip("压屏 - 缓入");
		RegisterEvent();
	}

	public void RegisterEvent(){
		EventDelegate.Set(_view.EndBtn.onClick,()=>{
			StoryManager.Instance.EndStory();
		});
	}
	

	public void Dispose(){

	}


	float length;
	bool fadeIn = false;
	bool fadeOut = false;
	float timer;

	void Update(){

		timer += Time.deltaTime;

		if (fadeIn) {
			if(length < inst.length){
				length += Time.deltaTime * (inst.length / inst.needTime);
				_view.Top.bottomAnchor.absolute = (int)-length;
				_view.Bottom.topAnchor.absolute = (int)length;
			}
			else{
				fadeIn = false;
				//按钮出现
				_view.Content.SetActive(true);
			}
		}



		if (timer > inst.delayTime - inst.needTime) {
			//按钮消失
			_view.Content.SetActive(false);
			if(length > 0){
				length -= Time.deltaTime * (inst.length / inst.needTime);
				_view.Top.bottomAnchor.absolute =(int) -length;
				_view.Bottom.topAnchor.absolute = (int)length;
			}
			else{
				fadeOut = false;
				ProxyScreenPresureModule.Close();
			}
		}
	}


	//缓进
	public void FadeIn(float len){

	}

	//缓出
	public void FadeOut(){

	}
}
