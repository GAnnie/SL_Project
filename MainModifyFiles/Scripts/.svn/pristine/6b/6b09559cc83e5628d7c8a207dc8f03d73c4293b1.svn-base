// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  MonoTimer.cs
// Author   : wenlin
// Created  : 2013/3/7 
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

public class MonoTimer : MonoBehaviour
{
	private float _firstTime = 0.0f;
	private float _delayTime = 0.0f;
    private bool _isPlaying = false;
	public delegate void MonoTimeEventHandler( ); 
	private MonoTimeEventHandler _timeEventHandler = null;
	
	public int delegateCount=0;
	
	// Use this for initialization
	void Start ()
	{
//		_timeEventHandler = new MonoTimeEventHandler();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    public bool isPlaying { get { return _isPlaying; } }

	public void Setup( float time ,  MonoTimeEventHandler handler )
	{
		_delayTime = time;
		if (handler != null)
		{
			AddHandler(handler);
		}
	}

	public void Setup2Time( float time ,  MonoTimeEventHandler handler )
	{
		_delayTime = time;
		_firstTime = time;
		if (handler != null)
		{
			AddHandler(handler);
		}
	}	
	
	public void AddHandler(MonoTimeEventHandler handler)
	{
		_timeEventHandler += handler;
		delegateCount++;
	}
	
	public void RemoveHandler(MonoTimeEventHandler handler)
	{
		_timeEventHandler -= handler;
		delegateCount--;
	}
	
	public void Play()
	{
        if (_isPlaying) return;

        _isPlaying = true;
        CancelInvoke();
		InvokeRepeating( "TimeHandler", _firstTime, _delayTime);
	}
	
	public void Reset(){
		delegateCount = 0;
		_timeEventHandler = null;
        _isPlaying = false;
		CancelInvoke ();		
	}
	
	public void Stop()
	{
        _isPlaying = false;
		CancelInvoke ();
	}
		
	private void TimeHandler()
	{
		if ( _timeEventHandler != null )
		{
			_timeEventHandler();
		}
	}
	
	public void Destroy(){
		Stop();
		_timeEventHandler = null;
		GameObject.Destroy(this);
	}
}

