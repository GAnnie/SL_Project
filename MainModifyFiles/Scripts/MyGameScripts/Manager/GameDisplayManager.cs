// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  GameDisplayManager.cs
// Author   : senkay
// Created  : 5/22/2013 9:56:11 AM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;

public class GameDisplayManager
{
    public enum DisplayLevel
    {
        LOW = 0,
        MID,
        HIGH,
    }

    private static readonly GameDisplayManager instance = new GameDisplayManager();
    public static GameDisplayManager Instance
    {
        get
        {
            return instance;
        }
    }

    public DisplayLevel currentLevel = DisplayLevel.LOW;

    public void SetDisplayLevel(DisplayLevel level)
    {
        currentLevel = level;
    }

    public DisplayLevel GetDisplayLevel()
    {
        return currentLevel;
    }
}
