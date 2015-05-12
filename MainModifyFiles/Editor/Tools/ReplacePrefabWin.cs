// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  ReplacePrefab.cs
// Author   : wenlin
// Created  : 2013/6/9 11:45:03
// Purpose  : 
// **********************************************************************

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class ReplacePrefabWin : EditorWindow
{
    private static ReplacePrefabWin win = null;

    [MenuItem("Tools/Replace Prefab ")]
    public static void ShowWin()
    {
        if( win == null )
        {
             win = EditorWindow.GetWindow(typeof(ReplacePrefabWin)) as ReplacePrefabWin;
        }
        win.ShowPopup();
    }

    private string sourcePrefabName     = String.Empty;
    private GameObject decsPrefab      = null;
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
            sourcePrefabName = EditorGUILayout.TextField("将 ：",       sourcePrefabName);
            if (GUILayout.Button(" Get...")) { sourcePrefabName = GetSelectObjectName(); }
        EditorGUILayout.EndHorizontal();

        decsPrefab = EditorGUILayout.ObjectField("替换成为 ：", decsPrefab, typeof(GameObject), false) as GameObject;


        if (GUILayout.Button(" Replace One Prefab "))
        {
            if (ReplaceOneObj(sourceObj, decsPrefab))
            {
                sourceObj        = null;
                sourcePrefabName = null;
                decsPrefab       = null;
            }
        }

        if (GUILayout.Button(" Replace All Prefab "))
        {
            if (Replace(sourcePrefabName, decsPrefab))
            {
                sourcePrefabName = null;
                decsPrefab = null;
            }
        }
        EditorGUILayout.EndVertical();
    }

    private UnityEngine.GameObject sourceObj = null;
    private string GetSelectObjectName()
    {
        sourceObj = Selection.activeObject as GameObject;
        if (sourceObj != null)
            return sourceObj.name;
        else
            return "";
    }

    private bool ReplaceOneObj( GameObject  source, GameObject decs )
    {
        if (source == null || decs == null)
        {
            EditorUtility.DisplayDialog("Tips", " Select ......", "OK");
            return false;
        }

        //GameObject prefab = PrefabUtility.GetPrefabParent(decs) as GameObject;
        //if (prefab == null)
        //{
        //    EditorUtility.DisplayDialog("Tips", decs.name + "is not a prefab ", "OK");
        //    return false;
        //}

        GameObject inst = PrefabUtility.InstantiatePrefab(decs) as GameObject;
        inst.transform.parent = source.transform.parent;
        inst.transform.position = source.transform.position;
        inst.transform.rotation = source.transform.rotation;
        inst.transform.localScale = source.transform.localScale;

        source.SetActive(false);

        return true;
    }

    private bool Replace(string source, GameObject decs)
    {
        if (decs == null)
        {
            EditorUtility.DisplayDialog("Tips", " DecsPrefab is null", "OK");
            return false;
        }


        //GameObject prefab = PrefabUtility.GetPrefabParent(decs) as GameObject;
        //if (prefab == null)
        //{
        //    EditorUtility.DisplayDialog("Tips", decs.name + "is not a prefab ", "OK");
        //    return false;
        //}

        List<string> _groupNameList = new List<string>();
        _groupNameList.Add("Grass");
        _groupNameList.Add("Sceneobj");
        _groupNameList.Add("Tree");

        for (int i = 0; i < _groupNameList.Count; i++)
        {
            GameObject layer = GameObject.Find(_groupNameList[i]);
            if (layer != null)
            {
                _ReplaceChild(layer, source, decs);
            }
        }

        return true;
    }

    private void _ReplaceChild(GameObject layer, string sourceName, GameObject prefab)
    {
        List<Transform> childTransformList = new List<Transform>();
        int childCount = layer.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = layer.transform.GetChild(i);
            
            if (child != null && child.name == sourceName)
            {
                childTransformList.Add(child);
            }
        }

        for (int i = 0; i < childTransformList.Count; i++)
        {
            GameObject inst = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            inst.transform.parent = layer.transform;
            inst.transform.position = childTransformList[i].position;
            inst.transform.rotation = childTransformList[i].rotation;
            inst.transform.localScale = childTransformList[i].localScale;

            childTransformList[i].gameObject.SetActive(false);
        }
    }
}
