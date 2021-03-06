﻿// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  NavigationArea.cs
// Author   : willson
// Created  : 2014/12/16 
// Porpuse  : 
// **********************************************************************
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using com.nucleus.h1.logic.core.modules.scene.dto;

public class NavigationArea
{
	private Vector3 _posTopLeft;
	private Vector3 _posBottomRight;

	private List< List<NavigationPoin> > _poins;

	public NavigationArea()
	{
		Vector3[] vertices = null;
		int[] indices = null;
		
		NavMesh.Triangulate(out vertices,out indices);

		float topLeft_x = 0;
		float topLeft_z = 0;

		float bottomRight_x = 0;
		float bottomRight_z = 0;

		// 分析 vertices 取得 TopLeft BottomRight
		foreach(Vector3 pos in vertices)
		{
			if(pos.x < topLeft_x)
			{
				topLeft_x = pos.x;
			}

			if(pos.z > topLeft_z)
			{
				topLeft_z = pos.z;
			}

			//////

			if(pos.x > bottomRight_x)
			{
				bottomRight_x = pos.x;
			}

			if(pos.z < bottomRight_z)
			{
				bottomRight_z = pos.z;
			}
		}

		_posTopLeft = new Vector3((int)topLeft_x,0,(int)topLeft_z);
		_posBottomRight = new Vector3((int)bottomRight_x,0,(int)bottomRight_z);

		_poins = new List<List<NavigationPoin>>();
	}

	public void CreateMoveNavigation()
	{
        // 隐藏 way all 显示 way
        string map = "World_" + WorldManager.Instance.GetModel().GetSceneDto().sceneMap.resId;
        Transform sceneRoot = LayerManager.Instance.SceneLayer.transform.Find(map);

        if(sceneRoot == null)
        {
            TipManager.AddTip("地图文件错误,SceneLayer层 找不到地图文件: " + map);
            GameDebuger.LogWithColor("地图文件错误,SceneLayer层 找不到地图文件: " + map);
            return;
        }

        Transform wayAlltf = sceneRoot.Find("way_all");
        if(wayAlltf == null)
        {
            TipManager.AddTip(string.Format("地图文件错误,{0} 找不到地图GameObject: way_all", map));
            GameDebuger.LogWithColor(string.Format("地图文件错误,{0} 找不到地图GameObject: way_all", map));
            return;
        }

        Transform waytf = sceneRoot.Find("way");
        if (waytf == null)
        {
            TipManager.AddTip(string.Format("地图文件错误,{0} 找不到地图GameObject: way", map));
            GameDebuger.LogWithColor(string.Format("地图文件错误,{0} 找不到地图GameObject: way", map));
            return;
        }

        if (wayAlltf.gameObject != null)
            wayAlltf.gameObject.SetActive(false);
        if (waytf.gameObject != null)
            waytf.gameObject.SetActive(true);

		for(int z = (int)_posTopLeft.z;z >= (int)_posBottomRight.z;z--)
		{
			List<NavigationPoin> poins = new List<NavigationPoin>();
			for(int x = (int)_posTopLeft.x;x <= (int)_posBottomRight.x;x++)
			{
				NavigationPoin poin = new NavigationPoin(new Vector3(x,0,z));
				poins.Add(poin);
			}
			_poins.Add(poins);
		}
        // 隐藏 way 显示 way all
        if (wayAlltf.gameObject != null)
            wayAlltf.gameObject.SetActive(true);
        if (waytf.gameObject != null)
            waytf.gameObject.SetActive(false);
	}

	public void OutputFile(string path)
	{
		//path = Application.dataPath + "/Docs/NavigationArea/";
		SceneDto dto = WorldManager.Instance.GetModel().GetSceneDto();
		string fileName = dto.id + ".txt";
		File.Delete(path + fileName); 

		string fileName2 = string.Format("map_{0}.txt",dto.id);
		File.Delete(path + fileName2);

		FileInfo file = new FileInfo(path + fileName); 
		FileInfo fileMap = new FileInfo(path + fileName2); 

		StreamWriter sw = file.CreateText();
		StreamWriter swMap = fileMap.CreateText();

		string str = "";
		for (int z = 0;z < _poins.Count;z++)
		{
			List<NavigationPoin> poins = _poins[z];
			string line = "";
			string lineMap = "";

			for(int x = 0;x < poins.Count;x++)
			{
				line += poins[x].ToString() + ",";
				lineMap += poins[x].canMove()? "1,":" ,";
			}

			line = line.Substring(0,line.Length - 1);
			lineMap = lineMap.Substring(0,lineMap.Length - 1);

			if(z != _poins.Count -1)
			{
				sw.WriteLine(line);
			}
			else
			{
				sw.Write(line);
			}

			swMap.WriteLine(lineMap);
		}

		sw.Close();
		sw.Dispose();

		swMap.Close();
		swMap.Dispose();

		string tips = string.Format("地图:{0} - {1} 行走数据成功成功,请察看文件目录 {2}",dto.name,dto.id,path);
		TipManager.AddTip(tips);
		Debug.Log(tips);
	}
}