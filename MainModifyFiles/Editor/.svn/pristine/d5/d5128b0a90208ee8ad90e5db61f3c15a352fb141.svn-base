// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  Test.cs
// Author   : willson
// Created  : 2014/12/10 
// Porpuse  : 
// **********************************************************************

using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Collections.Generic;
using LITJson;

public class AutoCreateWeaponConfig : Editor {
	
	private static WeaponConfig weaponConfig;
	
	[MenuItem("Tools/AutoCreateWeaponConfig", false, 2)]
	static void DoAutoCreateWeaponConfig()
	{
		weaponConfig = new WeaponConfig();
		weaponConfig.list = new List<WeaponBindConfig>();
		
		DirectoryInfo directoryInfo = new DirectoryInfo(PetPrefabPath);
		
		foreach (DirectoryInfo d in directoryInfo.GetDirectories())
		{
			if(d.Name.IndexOf("svn") == -1 && d.Name.IndexOf("Template") == -1)
			{
				DoCreateAnimationAssets(d.Name);
			}
		}
		
		//DoCreateAnimationAssets("pet_11");
		
		SaveConfig();
		
		Debug.Log(">>>>>>>>>>>  AutoCreateWeaponConfig 完成,请按 \"Ctrl + S\" 保存");
	}
	
	private const string Config_WritePath = "Assets/GameResources/ConfigFiles/WeaponConfig/WeaponConfig.bytes";
	private const string Config_ReadPath = "ConfigFiles/WeaponConfig/WeaponConfig";
	
	static void SaveConfig ()
	{
		bool isCompress = false;
		
		string json = JsonMapper.ToJson (weaponConfig);
		byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes (json);
		if (isCompress) {
			buff = ZipLibUtils.Compress (buff);
		}
		
		if (json.Length > 0) {
			DataHelper.SaveFile (Config_WritePath, buff);
		}
		
		AssetDatabase.Refresh();
		
		weaponConfig = null;
		
		weaponConfig = DataHelper.GetJsonFile<WeaponConfig>(Config_ReadPath, "bytes", false);
		WeaponBindConfig config = weaponConfig.list[0];
		//DataHelper.SaveJsonFile (configInfo, BattleConfig_WritePath, false);
	}
	
	static string PetPrefabPath = "Assets/GameResources/ArtResources/Characters/Pet/";
	
	static void DoCreateAnimationAssets(string actorName)
	{
		// A.判断 Prefab 是否存在
		string prefabDirectory =  string.Format(PetPrefabPath + "{0}/Prefabs/{0}.prefab", actorName);
		GameObject prefab = AssetDatabase.LoadAssetAtPath(prefabDirectory,typeof(GameObject)) as GameObject;
		if(prefab != null)
		{
			GameObject go = GameObject.Instantiate (prefab) as GameObject;
			DoUpdateModelWeapon(go, actorName, "Bip001/Bip001 Prop1");
			DoUpdateModelWeapon(go, actorName, "Bip001/Bip001 Prop2");
			GameObject.DestroyImmediate(go);
		}
	}
	
	static void DoUpdateModelWeapon(GameObject go, string actorName, string bip001Name)
	{
		Transform tran = go.transform.Find (bip001Name);
		if (tran != null)
		{
			Debug.Log(go.ToString() + " " + bip001Name);
			
			//Vector3 newPosition = tran.InverseTransformPoint(Vector3.zero);
			//Vector3 newEulerAngles = new Vector3(0f,-90f,-90f);

			GameObject newGo = new GameObject();
			newGo.transform.parent = tran;

			Vector3 newPosition = newGo.transform.localPosition;
			Vector3 newEulerAngles = newGo.transform.localEulerAngles;

			Debug.Log("newLocalPosition="+newPosition.ToString());
			Debug.Log("newEulerAngles1="+newEulerAngles.ToString());

			WeaponBindConfig config = new WeaponBindConfig();
			config.key = actorName + "/" + bip001Name;
			config.localPosition = newPosition;
			config.localEulerAngles = newEulerAngles;
			
			weaponConfig.list.Add(config);
		}
	}
}