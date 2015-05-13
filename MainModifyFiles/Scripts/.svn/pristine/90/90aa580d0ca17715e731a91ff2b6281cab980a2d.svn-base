using UnityEngine;
using System.Collections;

public class ButtonLabelSpacingAdjust : MonoBehaviour {

	private UILabel _label;
	private int _lastLen = 0;
	private int _newLen = 0;

	public bool autoAdjust = false;

	void Awake()
	{
		_label = this.transform.GetComponentInChildren<UILabel>();
		if (_label == null)
		{
			this.enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
		_label.text = _label.text.Replace(" ","");
		_newLen = _label.text.Length;

		r();
	}

	public void ReAdjust()
	{
		Start ();
	}

	// Update is called once per frame
	void Update () {
		if (autoAdjust)
		{
			ReAdjust();
		}

		r();
	}

	void r()
	{
		if (_lastLen != _newLen)
		{
			_lastLen = _newLen;
			
			if (_lastLen == 2)
			{
				_label.spacingX = 20;
			}
			else if (_lastLen == 3)
			{
				_label.spacingX = 8;
			}
			else
			{
				_label.spacingX = 2;
			}
		}
	}
}
