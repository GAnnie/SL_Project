// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  Distext.cs
// Author   : senkay
// Created  : 4/12/2013 9:37:02 AM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;

public class Distext
{
    private UILabel _uiLabel;
    private float _breakTime;
    private string _content;
    private string _showContent;

    private int _position;

    private bool _playing;

	public delegate void DistextFinishDelegate();
    private DistextFinishDelegate _distextFinishDelegate = null;

    private MonoTimer _timer;

    public Distext(UILabel uiLabel, float breakTime, DistextFinishDelegate _delegate)
    {
        _uiLabel = uiLabel;
        _uiLabel.text = "";
        _breakTime = breakTime;
        _distextFinishDelegate = _delegate;

        _showContent = "";
        _position = 0;

        _playing = false;

        _timer = TimerManager.GetTimer("Distext");
        _timer.Setup2Time(_breakTime, OnTimer);
    }

    public void Play(string content)
    {
        _uiLabel.text = content;
        _uiLabel.gameObject.SetActive(false);
        _showContent = "";
        _position = 0;

        _content = content;

        if (string.IsNullOrEmpty(content))
        {
            Finish();
        }
        else
        {
            _playing = true;
            _timer.Play();
        }
    }

    private void OnTimer()
    {
        _uiLabel.gameObject.SetActive(true);
        _showContent += _content.Substring(_position, 1);
        _uiLabel.text = _showContent;

        if ((_position+1) >= _content.Length)
        {
            Finish();
        }
        else
        {
            _position++;
        }
    }

    private void Finish()
    {
        _playing = false;
        _timer.Stop();
        if (_distextFinishDelegate != null)
        {
            _distextFinishDelegate();
        }
    }

    //直接跳到最后
    public void GotoEnd()
    {
        _uiLabel.text = _content;
        _uiLabel.gameObject.SetActive(true);
        Finish();
    }

	public bool IsPlaying(){
		return _playing;
	}

    public void Destroy()
    {
        _playing = false;
        _timer.Stop();
        TimerManager.RemoveTimer(_timer);
        _timer = null;
        _distextFinishDelegate = null;
    }
}
