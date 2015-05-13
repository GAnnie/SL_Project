#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TestModelHue : MonoBehaviour {

	public GameObject modelPrefab;
	private List<Vector3> _hueParamList;
	private ModelHSV _modelHSV;

	void Start(){
		GameObject model = NGUITools.AddChild(this.gameObject,modelPrefab);

		//替换模型材质为pet_xxx_mask
		string matPath = string.Format("Assets/GameResources/"+PathHelper.PET_MATERIALS_PATH,modelPrefab.name,modelPrefab.name+"_mask.mat");
		Material hsvMaterial = Resources.LoadAssetAtPath<Material>(matPath);
		Transform renderRoot = model.transform.Find(modelPrefab.name);
		Renderer renderer = renderRoot.GetComponent<Renderer>();
		renderer.material = hsvMaterial;

		_modelHSV = renderRoot.gameObject.AddMissingComponent<ModelHSV>();

		_hueParamList = new List<Vector3>(3);
		for(int i=0;i<3;++i){
			_hueParamList.Add(new Vector3(0f,1f,1f));
		}
		Selection.activeTransform = _modelHSV.transform;
	}

	void OnGUI(){
		for(int i=0;i<3;++i)
		{
			float hue = DrawSliderInfo("色相",_hueParamList[i].x,360f);
			float saturation = DrawSliderInfo("饱和度",_hueParamList[i].y,2f);
			float value = DrawSliderInfo("明度",_hueParamList[i].z,2f);
			_hueParamList[i] = new Vector3(hue,saturation,value);
			GUILayout.Space(10f);
			if(GUILayout.Button("copy")){
				GameDebuger.clipBoard = string.Format("{0:0.00},{1:0.00},{2:0.00}",hue,saturation,value);
			}
		}

		if(GUI.changed){
			_modelHSV.SetupHueShift(_hueParamList[0],_hueParamList[1],_hueParamList[2]);
		}
	}	

	float DrawSliderInfo(string title,float param,float maxVal){
		GUILayout.Label(string.Format("{0}:{1:0.00}",title,param));
		GUILayout.BeginHorizontal();
		float result = GUILayout.HorizontalSlider(param,0f,maxVal,GUILayout.Width(100f));
//		if(GUILayout.Button("copy")){
//			GameDebuger.clipBoard = string.Format("{0:0.00}",param);
//		}
		GUILayout.EndHorizontal();
		return result;
	}
}

#endif
