using UnityEngine;
using System.Collections;

public class AutoBasedOnFullHeight : MonoBehaviour
{
	
	// Use this for initialization
	void Start ()
	{
		UpdateOne ();
	}
	
	[ContextMenu("Execute")]
	public void UpdateOne ()
	{
		Transform trans = this.transform;
		float factor = UIRoot.GetPixelSizeAdjustment (this.gameObject);
		
		float newHeight = Screen.height * factor + 10f;
		
		UIWidget widget = this.GetComponent<UIWidget> ();
		if (widget != null) {
			widget.height = (int)newHeight;
		}
	}
}

