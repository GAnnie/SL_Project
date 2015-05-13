
using UnityEngine;
using System.Collections;

public class FXMgrShadow : MonoBehaviour
{
	public struct Shadow
	{
		public Vector3 lt, rt, rb, lb;
		public Vector4 uv;
		public Color clr;
	}

	public int _Cap = 32;
	public Material _Material = null;

	float _texWidth = 0;
	float _texHeight = 0;

	protected Shadow[] _Buf = null;
	protected int _Size = 0;
	protected Mesh _Mesh = null;

	public void AddShadow (Vector3 pos, Vector3 fwd, Vector3 side, float fwdScale, float sideScale, Vector4 uv, Color clr)
	{
		if (_Size < _Cap) 
		{
			Shadow sp;
			
			sp.lt = pos + fwd * fwdScale - side * sideScale;
			sp.rt = pos + fwd * fwdScale + side * sideScale;
			sp.rb = pos - fwd * fwdScale + side * sideScale;
			sp.lb = pos - fwd * fwdScale - side * sideScale;
			
			sp.uv = uv;
			sp.uv.z += sp.uv.x;
			sp.uv.w += sp.uv.y;
			
			sp.clr = clr;
			
			_Buf[_Size] = sp;
			
			++_Size;
		}
	}

	public void AddShadow (Vector3 lt, Vector3 rt, Vector3 rb, Vector3 lb, Vector4 uv, Color clr)
	{
		if (_Size < _Cap) 
		{
			Shadow sp;
			
			sp.lt = lt;
			sp.rt = rt;
			sp.rb = rb;
			sp.lb = lb;
			
			sp.uv = uv;
			sp.uv.z += sp.uv.x;
			sp.uv.w += sp.uv.y;
			
			sp.clr = clr;
			
			_Buf[_Size] = sp;
			
			++_Size;
		}
	}

	void Awake ()
	{
		if (null != _Material) 
		{
			Texture2D t = _Material.GetTexture ("_MainTex") as Texture2D;
			
			InitImpl (_Material, t, _Cap);
		}
	}

	public void Init (Material mtl, Texture2D tex, int Cap)
	{
		if (null != _Material)
			return;
		InitImpl (mtl, tex, Cap);
	}

	void InitImpl (Material mtl, Texture2D tex, int Cap)
	{
		_Cap = Cap;
		_Material = mtl;

        if (null != tex)
        {
            _texWidth = tex.width;
            _texHeight = tex.height;
        }
		
		// Reset Transform
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		
		// Attach Mesh
		MeshFilter flt = gameObject.AddComponent (typeof(MeshFilter)) as MeshFilter;
		_Mesh = flt.mesh;
		
		MeshRenderer mr = gameObject.AddComponent (typeof(MeshRenderer)) as MeshRenderer;
		mr.material = _Material;
		
		mr.castShadows = false;
		mr.receiveShadows = false;
		
		// Init Buffer
		_Buf = new Shadow[_Cap];
		_Size = 0;
	}

	void LateUpdate ()
	{
		_Mesh.Clear ();
		
		if (_Size <= 0)
			return;
		
		Vector3[] vb = new Vector3[_Size * 4];
		Vector2[] uvs = new Vector2[_Size * 4];
		Color[] clrs = new Color[_Size * 4];
		int[] ib = new int[_Size * 3 * 2];
		
		float wfac = 1.0f / _texWidth;
		float hfac = 1.0f / _texHeight;
		
		for (int i = 0; i < _Size; ++i) 
		{
			Shadow sp = (Shadow)_Buf[i];
			
			vb[i * 4 + 0] = sp.lt;
			vb[i * 4 + 1] = sp.rt;
			vb[i * 4 + 2] = sp.rb;
			vb[i * 4 + 3] = sp.lb;
			
			Vector4 uv = sp.uv;
			
			float y = uv.y;
			uv.y = _texHeight - uv.w - 1;
			uv.w = _texHeight - y - 1;
			
			uv.x *= wfac;
			uv.z *= wfac;
			uv.y *= hfac;
			uv.w *= hfac;
			
			uvs[i * 4 + 0] = new Vector2 (uv.x, uv.y);
			uvs[i * 4 + 1] = new Vector2 (uv.z, uv.y);
			uvs[i * 4 + 2] = new Vector2 (uv.z, uv.w);
			uvs[i * 4 + 3] = new Vector2 (uv.x, uv.w);
			
			clrs[i * 4 + 0] = sp.clr;
			clrs[i * 4 + 1] = sp.clr;
			clrs[i * 4 + 2] = sp.clr;
			clrs[i * 4 + 3] = sp.clr;
			
			ib[i * 6 + 0] = i * 4 + 0;
			ib[i * 6 + 1] = i * 4 + 1;
			ib[i * 6 + 2] = i * 4 + 2;
			
			ib[i * 6 + 3] = i * 4 + 0;
			ib[i * 6 + 4] = i * 4 + 2;
			ib[i * 6 + 5] = i * 4 + 3;
		}
		
		// Load Mesh
		_Mesh.vertices = vb;
		_Mesh.uv = uvs;
		_Mesh.colors = clrs;
		
		_Mesh.triangles = ib;
		
		_Size = 0;
	}
}
