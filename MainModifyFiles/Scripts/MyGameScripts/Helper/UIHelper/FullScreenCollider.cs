using UnityEngine;
using System.Collections;

public class FullScreenCollider : MonoBehaviour
{
	
	void Start()
	{
		
		GameObject go = this.gameObject;

		BoxCollider box = go.GetComponent<BoxCollider>();
		
		if(box == null) box = go.AddComponent<BoxCollider>();
		
		if(box != null)
		{
			float factor = UIRoot.GetPixelSizeAdjustment(go);
			
			float newWidth = Screen.width * factor;
			float newHeight = Screen.height * factor;
			
			Vector3 size = box.size;
			size.x = newWidth;
			size.y = newHeight;
			box.size = size;
			
			GameObjectExt.GetMissingComponent<UIWidget>(go);
		}
		
		Destroy (this);
		
	}
	
	
}
