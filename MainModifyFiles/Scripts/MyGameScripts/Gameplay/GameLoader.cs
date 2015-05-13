// **********************************************************************
// Copyright (c) 2015 cilugame. All rights reserved.
// File     : GameLoader.cs
// Author   : senkay <senkay@126.com>
// Created  : 05/12/2015 
// Porpuse  : 
// **********************************************************************
//
using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour
{
	private static string PATH = "Prefabs/Module/GameStartModule/GameStart";

	void Start ()
	{
		UnityEngine.Object tempPrefab = ResourceLoader.Load( PATH );
		GameObject go = GameObject.Instantiate(tempPrefab as GameObject) as GameObject;
		go.name = "GameStart";
		GameObject.Destroy(this.gameObject);
	}
}

