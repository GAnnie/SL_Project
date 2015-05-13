// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  BattleTurnController.cs
// Author   : senkay
// Created  : 5/18/2013 11:23:33 AM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnController : MonoBehaviourBase
{
    public UILabel turnLabel;

    public void UpdateTurn(int currentTurn, int maxTurn)
    {
        turnLabel.text = currentTurn + "/" + maxTurn;
    }

	public void Show()
	{
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}
}