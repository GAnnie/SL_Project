// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  AutoFullScreen.cs
// Author   : SK
// Created  : 2013/2/21
// Purpose  : 让界面自动拉伸至全屏
// **********************************************************************
using UnityEngine;
using System.Collections;

public class AutoFullScreen : MonoBehaviour
{
	public bool scaleWidth = true;
	public bool scaleHeight = true;

	public bool boxCollider = false;

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
		
		float newWidth = Screen.width * factor + 10f;
		float newHeight = Screen.height * factor + 10f;

		UIWidget widget = this.GetComponent<UIWidget> ();
		if (widget != null) {
			widget.SetDimensions (scaleWidth ? (int)newWidth : widget.width, scaleHeight ? (int)newHeight : widget.height);
		} else {
			trans.localScale = new Vector3 (scaleWidth ? newWidth : trans.localScale.x, scaleHeight ? newHeight : trans.localScale.y, trans.localScale.z);
		}

		if (boxCollider)
		{
			BoxCollider collider = this.GetComponent<BoxCollider>();
			collider.size = new Vector3(scaleWidth ? (int)newWidth : collider.size.x, scaleHeight ? (int)newHeight : collider.size.y, 1);
		}
	}

#if UNITY_EDITOR
	void Update()
	{
		UpdateOne();
	}
#endif
}

