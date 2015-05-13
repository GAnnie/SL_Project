// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  GameAudioItem.cs
// Author   : wenlin
// Created  : 2013/4/16 20:52:26
// Purpose  : 
// **********************************************************************

using System;

public class GameAudioItem
{
    private int         _max_limit = 10;
    private int         _accNumber = 0;
    private string      _itemName = "";
    private bool        _isOutTime = false;
    private AudioItem   _item = null;


    public void AddAccNumber()  { _accNumber++; }
    public void Reset()         { _accNumber = 0; }
    public void SetOutTime()    { _isOutTime = true;}
    public bool IsOvertake      { get { return _accNumber > _max_limit || _isOutTime; } }

    public GameAudioItem( string itemName, AudioItem item, int max_limit= 10 )
    {
        _itemName = itemName;
        _item = item;
        _accNumber = 0;
        _isOutTime = false;

        _max_limit = max_limit;
    }

    public string ItemName  { get { return _itemName; } }
    public AudioItem Item   { get { return _item; } }
}
