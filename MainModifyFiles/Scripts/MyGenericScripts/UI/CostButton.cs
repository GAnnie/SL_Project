using UnityEngine;
using System.Collections;

public class CostButton : MonoBehaviour 
{
	private UILabel _nameLabel;
	private UILabel _costLabel;
	private UISprite _costIconSprite;

	private int _lastLen = 0;
	private int _newLen = 0;
	
	public bool autoAdjust = false;
	
	void Awake()
	{
		_nameLabel = this.transform.Find("NameLabel").GetComponent<UILabel>();
		_costLabel = this.transform.Find("CostLabel").GetComponent<UILabel>();
		_costIconSprite = this.transform.Find("CostIconSprite").GetComponent<UISprite>();
		if (_nameLabel == null)
		{
			this.enabled = false;
		}
	}
	
	// Use this for initialization
	void Start () {
		_nameLabel.text = _nameLabel.text.Replace(" ","");
		_newLen = _nameLabel.text.Length;
		
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
				_nameLabel.spacingX = 20;
			}
			else if (_lastLen == 3)
			{
				_nameLabel.spacingX = 8;
			}
			else
			{
				_nameLabel.spacingX = 2;
			}
		}
	}

	public string NameStr
	{
		get
		{
			return _nameLabel.text;
		}
		set
		{
			_nameLabel.text = value;
			ReAdjust();
		}
	}

	public int Cost
	{
		get
		{
			return int.Parse(_costLabel.text);
		}
		set
		{
			_costLabel.text = value.ToString();
		}
	}


	public string CostStr
	{
		get
		{
			return _costLabel.text;
		}
		set
		{
			_costLabel.text = value;
		}
	}

	public string CostIconSprite
	{
		get
		{
			return _costIconSprite.spriteName;
		}
		set
		{
			if(_costIconSprite.spriteName != value)
			{
				_costIconSprite.spriteName = value;
			}
		}
	}
}
