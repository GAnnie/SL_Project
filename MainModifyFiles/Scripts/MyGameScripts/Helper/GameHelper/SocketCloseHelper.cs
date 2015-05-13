// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  SocketCloseHelper.cs
// Author   : senkay
// Created  : 5/11/2013 10:44:49 AM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.services;

public class SocketCloseHelper : MonoBehaviour {

    void OnApplicationQuit()
    {
		if (SocketManager.IsOnLink)
        {
			ServiceRequestAction.requestServer(PlayerService.logout());
        }		
		
		SocketManager.Instance.Destroy();
    }

}
