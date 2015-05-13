using UnityEngine;
using System.Collections;
using System;

public class BattleLaunchTimer : MonoBehaviourBase
{
	public static int MAX_INSTRUCTION_TIME = 30;
	public static int AUTO_TIME = 26;
	
	float timeCounter = 0.0f;
	
	bool isEnable = false;

	public event Action OnFinishedDelegate;

	public event Action OnAutoTimeFinish;

	//---UI
	public UILabel timeLabel;
	
	//Test
	[System.NonSerializedAttribute]
	public BattleController bc;
	private float syncTimerCounter = 0;
	//-----------------
	
	void Awake()
	{
		timeLabel = GetComponentInChildren< UILabel >();
		
		ResetTimer();
	}

	public void LaunchTimer(int time, int cancelAutoSec)
	{
		this.gameObject.SetActive(true);

		AUTO_TIME = MAX_INSTRUCTION_TIME - cancelAutoSec - 1;
		_waitingAutoTime = true;

		ResetTimer(time);
		EnableTimer( true );
		
		// test only 
		if ( bc != null )
		{
			int num = 0;//bc.GetPlayerUnit( true );
			
			int enemyNum = bc.GetMonsterList( true, MonsterController.MonsterSide.Enemy ).Count;
			int playerNum = bc.GetMonsterList( true, MonsterController.MonsterSide.Player ).Count;
				
			num = enemyNum + playerNum;
			
			syncTimerCounter = num * 3 + MAX_INSTRUCTION_TIME;
		}
		//-----------------
	}

	public void EnableTimer( bool enable )
	{
		isEnable = enable;
	}
	
	public void StopTimer()
	{
		EnableTimer( false );
		timeCounter = 0;
//		gameObject.SetActive( false );
	}
	
	void ResetTimer(int time = 0)
	{
		if (time == 0){
			timeCounter = MAX_INSTRUCTION_TIME;
		}else{
			timeCounter = time;
		}
	}

	public int GetSeconds()
	{
		return Mathf.FloorToInt( timeCounter );
	}
	
	private int lastSecond = 0;

	private bool _waitingAutoTime = true;
	
	void Update()
	{
		if ( isEnable )
		{
			timeCounter -= Time.deltaTime;

			if (timeCounter <= AUTO_TIME && _waitingAutoTime)
			{
				_waitingAutoTime = false;
				if ( OnAutoTimeFinish != null )
					OnAutoTimeFinish();
			}

			if ( timeCounter <= 0 )
			{
				timeCounter = 0;
				
				if ( OnFinishedDelegate != null )
					OnFinishedDelegate();
				
				EnableTimer( false );
			}
			
			timeLabel.text = GetSeconds().ToString();// + "/" + Mathf.FloorToInt( syncTimerCounter ).ToString();
		}
		
		// test
		syncTimerCounter -= Time.deltaTime;
		timeLabel.text = GetSeconds().ToString();// + "/" + Mathf.FloorToInt( syncTimerCounter ).ToString();
		//------------------		
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

	public void DestroyIt(){
		OnAutoTimeFinish = null;
		OnFinishedDelegate = null;
	}
}
