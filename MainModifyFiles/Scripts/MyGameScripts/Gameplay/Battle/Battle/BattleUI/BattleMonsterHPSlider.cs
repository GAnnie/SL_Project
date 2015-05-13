using UnityEngine;
using System.Collections;

public class BattleMonsterHPSlider : MonoBehaviourBase
{
	static public BattleMonsterHPSlider CreateNew(MonsterController mc)
	{
		GameObject hpSliderPrefab = Resources.Load("Prefabs/Module/Battle/BattleUI/MonsterHPSlider") as GameObject;
		GameObject hpSliderGO = NGUITools.AddChild(LayerManager.Instance.battleHudTextAnchor, hpSliderPrefab);
		UIFollowTarget follower = hpSliderGO.AddComponent<UIFollowTarget>();
		
		follower.gameCamera = LayerManager.Instance.GetBattleFollowCamera();
		follower.uiCamera = LayerManager.Instance.UICamera;
		
		Transform tf = mc.GetMountHUD();
		
		if ( tf == null )
		{
			tf = mc.gameObject.transform;
			follower.offset = new Vector3(0, 2f, 0);
		}else{
			follower.offset = new Vector3(0, 0.4f, 0);
		}
		
		follower.target = tf;
		BattleMonsterHPSlider hpSlider = hpSliderGO.GetComponent<BattleMonsterHPSlider>();
		hpSlider.Setup(mc);
		return hpSlider;
	}

	public UISlider slider;
	public UISprite readySprite;

	private MonsterController monster;

	void Awake()
	{
		//slider = this.gameObject.GetComponent<UISlider>();
		//readySprite = this.transform.Find("ReadySprite").GetComponent<UISprite>();
	}

	public void Setup(MonsterController mc)
	{
		monster = mc;
		ShowReady (false);
	}

//	// Update is called once per frame
//	void Update() {
//		if (monster != null)
//		{
//			float sliderValue = monster.GetHP()/monster.GetMaxHP();
//			if (sliderValue > 0 && sliderValue < 0.01){
//				sliderValue = 0.01;
//			}
//			slider.sliderValue = monster.GetHP()/monster.GetMaxHP();
//		}
//	}

	private float _hpPrecent;
	private float _lerpTime = 0;
	private float _oriFillAmount = -1;
	private float _lerpBaseTime = 0;
	
	void Update()
	{
		if (monster != null)
		{
			ShowReady(monster.NeedReady);

			_hpPrecent = (float)monster.currentHP/(float)monster.maxHP;

			if (slider.sliderValue != _hpPrecent)
			{
				if (_oriFillAmount == -1)
				{
					_lerpTime = 0;
					_oriFillAmount = slider.sliderValue;

					if (_hpPrecent == 0)
					{
						_lerpBaseTime = 4f;
					}
					else
					{
						float modifyPrecent = Mathf.Abs(_oriFillAmount - _hpPrecent);
						_lerpBaseTime = 1f / (2f * modifyPrecent);
					}
				}
				_lerpTime += _lerpBaseTime * Time.deltaTime;
				slider.sliderValue = Mathf.Lerp(_oriFillAmount, _hpPrecent, _lerpTime );
			}
			else
			{
				_oriFillAmount = -1;
				if (_hpPrecent == 0)
				{
					//this.gameObject.SetActive(false);
				}
			}
		}
	}

	public void ShowReady(bool ready)
	{
		readySprite.gameObject.SetActive (ready);
	}

	public void Destroy()
	{
		monster = null;
		GameObject.Destroy(this.gameObject);
	}
}

