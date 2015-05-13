
using UnityEngine;
using System.Collections;

public class FXBBSimple : MonoBehaviour
{
	public float _MinScale = 1.0f;
	public float _MaxScale = 1.0f;
	public float _SpdScale = 5;

	public string _ClrName = "";

	public float _SpdBlink = 5;
	public float _MinAlpha = 0.5f;
	public float _MaxAlpha = 1.0f;

	Camera _Camera = null;
	float _Time = 0;

	void Start ()
	{
		if (null == _Camera) {
//          _Camera = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
			
			Object[] objs = GameObject.FindObjectsOfType (typeof(Camera));
			foreach (Camera c in objs) {
				if (c.name.Contains ("Main")) {
					_Camera = c as Camera;
					break;
				}
			}
		}
	}

	void Update ()
	{
		if (null != _Camera) {
			_Time += Time.deltaTime;
			
			{
				float scl = (Mathf.Sin (_Time * _SpdScale) + 1) * 0.5f;
				scl = Mathf.Lerp (_MinScale, _MaxScale, scl);
				
				transform.localScale = Vector3.one * scl;
				
				transform.LookAt (_Camera.transform.position);
			}
			
			if (_ClrName.Length > 0 && _SpdBlink > 0) {
				Color c = renderer.material.GetColor (_ClrName);
				
				float fac = (Mathf.Sin (_Time * _SpdBlink) + 1) * 0.5f;
				c.a = Mathf.Lerp (_MinAlpha, _MaxAlpha, fac);
				
				renderer.material.SetColor (_ClrName, c);
			}
		}
	}
}
