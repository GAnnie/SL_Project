using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class SceneFadeEffectController : MonoBehaviour {

	private const string NAME = "Prefabs/window/SceneFadeEffectPrefab";
	private static SceneFadeEffectController _instance;

	public static void Show(SceneDto sceneDto,System.Action onStartLoadMap,System.Action onFinishMap){
		GameObject module = UIModuleManager.Instance.OpenFunModule(NAME,UILayerType.FadeInOut,false);
		if(_instance == null){
			_instance = module.GetMissingComponent<SceneFadeEffectController>();
			_instance.InitView();
		}
		_instance.FadeIn(sceneDto,onStartLoadMap,onFinishMap);
	}
	
	private SceneFadeEffectPrefab _view;
	private System.Action _onStartLoadMap;
	private System.Action _onFinishLoadMap;
	private SceneDto _sceneDto;
	private bool _finishLoadMap = false;

	void InitView(){
		_view = gameObject.GetMissingComponent<SceneFadeEffectPrefab> ();
		_view.Setup (this.transform);
    }
    
	void FadeIn(SceneDto sceneDto,System.Action onStartLoadMap,System.Action onFinishMap){
        _sceneDto = sceneDto;
		_onStartLoadMap = onStartLoadMap;
		_onFinishLoadMap = onFinishMap;

		_view.tipLbl.text = LoadingTipManager.GetLoadingTip();
		UpdateSliderInfo(0f);
		EventDelegate.Set(_view.alphaTween.onFinished,LoadSceneMap);
		_view.alphaTween.PlayForward();
	}

	private float _maxLoadingValue; //地图加载进度最大值
	private float _loadingPercent;
	void LoadSceneMap(){
		if(_onStartLoadMap != null)
			_onStartLoadMap();

		_maxLoadingValue = Random.Range(0.6f,0.9f);
		_loadingPercent = 0f;

		WorldMapLoader.Instance.loadMapFinish = OnLoadMapFinish;
		WorldMapLoader.Instance.loadLevelProgress = (percent)=>{ 
			_loadingPercent = percent;
		};
		WorldMapLoader.Instance.LoadWorldMap(_sceneDto.sceneMap.resId);
	}

	void OnLoadMapFinish(){
		if(_onFinishLoadMap != null)
			_onFinishLoadMap();

		_sceneDto = null;
		_onStartLoadMap = null;
		_onFinishLoadMap = null;

		_finishLoadMap = true;
	}

	private void UpdateSliderInfo(float percent){
		_view.loadingSlider.value = percent;
		_view.loadingLbl.text = _view.loadingSlider.value.ToString("P0");
	}
	
	// Update is called once per frame
	void Update () {
		if(_finishLoadMap){
			//加载完地图，伪造进度条动画，load满后才开始淡出操作
			if(_view.loadingSlider.value < 1f)
				UpdateSliderInfo(_view.loadingSlider.value + Time.deltaTime);
			else
			{
				FadeOut();
			}
		}else if(_loadingPercent <= 0f){
			float maxValue = Mathf.Min(_view.loadingSlider.value + Time.deltaTime,_maxLoadingValue);
			UpdateSliderInfo(maxValue);
		}
	}

	void FadeOut(){
		EventDelegate.Set(_view.alphaTween.onFinished,Close);
		_view.alphaTween.PlayReverse();
	}

	void Close(){
		_finishLoadMap = false;
		UIModuleManager.Instance.HideModule(NAME);
    }
}
