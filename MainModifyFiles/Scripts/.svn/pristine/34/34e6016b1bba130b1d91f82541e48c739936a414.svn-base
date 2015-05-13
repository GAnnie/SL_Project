using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ModelHSV : MonoBehaviour
{
	public Vector3 _RHueShift = new Vector3 (0, 1, 1);
	public Vector3 _GHueShift = new Vector3 (0, 1, 1);
	public Vector3 _BHueShift = new Vector3 (0, 1, 1);

	private Renderer _renderer;
	// Use this for initialization
	void Awake ()
	{
		_renderer = this.renderer;
		//_renderer.material.shader = Shader.Find("Baoyu/Unlit/Xray-Hue");
	}

	private bool NeedUpdate = false;
	void Update()
	{
		if (NeedUpdate)
		{
			NeedUpdate = false;
			UpdateModelHueMatrix();
		}
	}

	void OnDestroy(){
		DestroyImmediate(_renderer.material);
	}

	//Called in the editor only
	void OnValidate ()
	{
		_RHueShift = ValidateHueShift (_RHueShift);
		_GHueShift = ValidateHueShift (_GHueShift);
		_BHueShift = ValidateHueShift (_BHueShift);
		UpdateModelHueMatrix ();
	}

	//1,1,1;2,2,2;3,3,3
	public void SetupColorParams(string colorParams){
		if(string.IsNullOrEmpty(colorParams))
			return;

		string[] splits = colorParams.Split (';');
		if(splits.Length != 3)
		{
			Debug.LogError ("colorParams is error");
			return;
		}

		SetupHueShift (Vector3Helper.Parse(splits[0]), Vector3Helper.Parse(splits[1]), Vector3Helper.Parse(splits[2]));
	}

	public void SetupHueShift(Vector3 rHueShift,Vector3 gHueShift,Vector3 bHueShift){
		_RHueShift = ValidateHueShift (rHueShift);
		_GHueShift = ValidateHueShift (gHueShift);
		_BHueShift = ValidateHueShift (bHueShift);
		UpdateModelHueMatrix();
	}

	private Vector3 ValidateHueShift (Vector3 hueShift)
	{
		hueShift.x = Mathf.Clamp (hueShift.x, 0f, 360f);
		hueShift.y = Mathf.Clamp (hueShift.y, 0f, 2f);
		hueShift.z = Mathf.Clamp (hueShift.z, 0f, 2f);
		return hueShift;
	}

	private void UpdateModelHueMatrix ()
	{
		if (_renderer != null) {
			Material material = _renderer.material;
			material.SetMatrix ("_RHueShift", GenerateHSVMatrix (_RHueShift.x, _RHueShift.y, _RHueShift.z));
			material.SetMatrix ("_GHueShift", GenerateHSVMatrix (_GHueShift.x, _GHueShift.y, _GHueShift.z));
			material.SetMatrix ("_BHueShift", GenerateHSVMatrix (_BHueShift.x, _BHueShift.y, _BHueShift.z));
		}
	}

	private Matrix4x4 GenerateHSVMatrix (float H, float S, float V)
	{
		float VSU = V * S * Mathf.Cos (Mathf.Deg2Rad * H);
		float VSW = V * S * Mathf.Sin (Mathf.Deg2Rad * H);

		Matrix4x4 T_HSV = Matrix4x4.zero;
		T_HSV.SetRow (0, new Vector4 (.299f * V + .701f * VSU + .168f * VSW, .587f * V - .587f * VSU + .330f * VSW, .114f * V - .114f * VSU - .497f * VSW));
		T_HSV.SetRow (1, new Vector4 (.299f * V - .299f * VSU - .328f * VSW, .587f * V + .413f * VSU + .035f * VSW, .114f * V - .114f * VSU + .292f * VSW));
		T_HSV.SetRow (2, new Vector4 (.299f * V - .3f * VSU + 1.25f * VSW, .587f * V - .588f * VSU - 1.05f * VSW, .114f * V + .886f * VSU - .203f * VSW));
		return T_HSV;
	}
}
