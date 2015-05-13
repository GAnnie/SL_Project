
using UnityEngine;
using System.Collections;

public class FXArrow : MonoBehaviour
{
	public float _Speed = 3;
	public float _SpdBlink = 5;

	float _Pos;
	float _Time = 0;

	void Awake ()
	{
		_Pos = 0;
	}

	void Update ()
	{
		float timeDelta = Time.deltaTime;
		
		_Time += timeDelta;
		
		float y = Mathf.Sin (_Time * _SpdBlink);
		if (y > -0.35f)
			y = -0.5f;
		else
			y = 0;
		
		Matrix4x4 mat = Matrix4x4.TRS (new Vector3 (_Pos, y, 0), Quaternion.identity, new Vector3 (1, 0.5f, 1));
		renderer.material.SetMatrix ("_TexMatrix", mat);
		
		_Pos += timeDelta * _Speed;
	}
}
