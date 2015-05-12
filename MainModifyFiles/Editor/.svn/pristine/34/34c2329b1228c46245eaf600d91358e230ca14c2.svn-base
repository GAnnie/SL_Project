// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  OptimizationTool.cs
// Author   : wenlin
// Created  : 2013/5/30 12:43:33
// Purpose  : 
// **********************************************************************
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;


public class OptimizationTool
{
	[MenuItem("OptimizationTool/Get Dependecies")]
    static void GetDependecies()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj != null)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            Debug.Log("Obj Path : " + path);
            string[] dependeciesPath = AssetDatabase.GetDependencies(new string[1] { path });

            foreach (string name in dependeciesPath)
            {
                Debug.Log("Name :" + name);
            }
        }
    }

	[MenuItem("OptimizationTool/Get GetInstanceID")]
    static void GetOBJGetInstanceID()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj != null)
        {
            Debug.Log(" Obj  : " + obj.name + " 's InstanceID - " + obj.GetInstanceID());
        }
    }

	[MenuItem("OptimizationTool/Turn Back CommonScene ")]
    static void TurnBackCommonScene()
    {
        _TurnBack("SceneLayer");
    }

	[MenuItem("OptimizationTool/Turn Back Battle Scene ")]
    static void TurnBackBattleScene()
    {
        _TurnBack("BattleStage");
    }


    static void _TurnBack(string layerName)
    {
        List<string> _groupNameList = new List<string>();
        _groupNameList.Add("Grass");
        _groupNameList.Add("Sceneobj");
        _groupNameList.Add("Effect");
        _groupNameList.Add("Tree");
        //_groupNameList.Add("Staticobj");

        GameObject sceneLayer = GameObject.Find(layerName);
        GameObject idLayer = sceneLayer.transform.GetChild(0).gameObject;
        if (sceneLayer == null)
        {
            Debug.Log("Can not find SceneLayer ");
            return;
        }

        GameObject copyparent = CopyGameObject(sceneLayer);
        GameObject copyIdLayer = CopyGameObject(idLayer.gameObject, copyparent);
        Dictionary<string, string> totalPrefabName = GetAssetFromGroup("Assets/GameResources", "prefab");

        foreach (string name in _groupNameList)
        {
            Transform sourceChildTransform = idLayer.transform.FindChild(name);
            if (sourceChildTransform != null)
            {
                GameObject sourceChild = sourceChildTransform.gameObject;
                GameObject targetChild = CopyGameObject(sourceChild, copyIdLayer);
                CopyChildGameObject(sourceChild, targetChild, totalPrefabName);
            }
        }
    }

    private static GameObject CopyGameObject(GameObject source, GameObject parent = null)
    {
        GameObject go = new GameObject();

        go.name = source.name;
        go.transform.position = source.transform.position;
        go.transform.rotation = source.transform.rotation;
        go.transform.localScale = source.transform.localScale;

        if (parent != null)
        {
            go.transform.parent = parent.transform;
        }

        return go;
    }

    private static void CopyPrefab(GameObject source, GameObject parent, string prefabPath)
    {

        UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(prefabPath);
        if (obj != null)
        {
            GameObject child = PrefabUtility.InstantiatePrefab(obj) as GameObject;
            child.name = source.name;
            child.transform.position = source.transform.position;
            child.transform.rotation = source.transform.rotation;
            child.transform.localScale = source.transform.localScale;

            if (parent != null)
            {
                child.transform.parent = parent.transform;
            }


            int sourceChildNum = source.transform.childCount;
            for (int i = 0; i < sourceChildNum; i++)
            {
                GameObject sourceChildGO = source.transform.GetChild(i).gameObject;
                Transform targetChildGOTransform = child.transform.FindChild(sourceChildGO.name);
                GameObject targetChildGO = null;
                if (targetChildGOTransform == null)
                {
                    Debug.Log("targetChildGOTransform == null  : " + sourceChildGO.name);
                    continue;
                }
                else
                {
                    targetChildGO = targetChildGOTransform.gameObject;
                }




                targetChildGO.transform.position = sourceChildGO.transform.position;
                targetChildGO.transform.rotation = sourceChildGO.transform.rotation;
                targetChildGO.transform.localScale = sourceChildGO.transform.localScale;

                MeshRenderer render = sourceChildGO.GetComponent<MeshRenderer>();
                MeshRenderer targetRender = targetChildGO.GetComponent<MeshRenderer>();
                if (render != null && targetRender != null)
                {
                    if (render.sharedMaterial == null)
                    {
                        Debug.Log("render.sharedMaterial == null  : " + render.name);
                    }
                    else
                    {
                        targetRender.sharedMaterial = render.sharedMaterial;
                    }

                }

                Animation animation = sourceChildGO.GetComponent<Animation>();
                if (animation != null)
                {
                    Animation targetAnimation = targetChildGO.GetComponent<Animation>();
                    if (targetAnimation == null)
                    {
                        targetAnimation = targetChildGO.AddComponent<Animation>();
                    }
                }
            }
        }
    }


    private static void CopyChildGameObject(GameObject source, GameObject target, Dictionary<string, string> totalPrefabName)
    {
        int childNum = source.transform.childCount;

        for (int i = 0; i < childNum; i++)
        {
            GameObject childGO = source.transform.GetChild(i).gameObject;
            string childName = childGO.name;

            if (totalPrefabName.ContainsKey(childName))
            {
                CopyPrefab(childGO, target, totalPrefabName[childName]);
            }
        }
    }


    private static Dictionary<string, string> GetAssetFromGroup(string path, params string[] extension)
    {
        if (!Directory.Exists(path))
        {
            Debug.Log("Directory  : " + path + " is not Exist");
            return null;
        }

        Dictionary<string, string> assetDic = new Dictionary<string, string>();
        DirectoryInfo info = new DirectoryInfo(path);
        ListFiles(info, assetDic, extension);
        return assetDic;
    }

    /// <summary>
    /// Lists the files.
    /// </summary>
    /// <param name='info'>
    /// Info.
    /// </param>
    /// <param name='assetDic'>
    /// Asset dic.
    /// </param>
    /// 
    private static void ListFiles(FileSystemInfo info, Dictionary<string, string> assetDic, params string[] extensions)
    {
        if (!info.Exists) return;

        DirectoryInfo dir = info as DirectoryInfo;
        if (dir == null) return;

        FileSystemInfo[] files = dir.GetFileSystemInfos();
        if (files != null)
        {
            for (int i = 0; i < files.Length; i++)
            {
                //is file
                if (files[i] is FileInfo)
                {
                    FileInfo file = files[i] as FileInfo;

                    string fileFullName = file.FullName.Replace("\\", "/");
                    int dotIndex = fileFullName.LastIndexOf(".");

                    string extension = fileFullName.Substring(dotIndex + 1);

                    foreach (string ext in extensions)
                    {
                        if (extension == ext)
                        {
                            if (!assetDic.ContainsKey(file.Name.Substring(0, file.Name.IndexOf("."))))
                            {
                                assetDic.Add(
                                        file.Name.Substring(0, file.Name.IndexOf(".")),
                                        fileFullName.Substring(fileFullName.IndexOf("Assets"))
                                        );
                            }
                        }
                    }
                }

                // is Directory
                else
                {
                    ListFiles(files[i], assetDic, extensions);
                }
            }
        }

    }


	[MenuItem("OptimizationTool/Export NavMeshToObj ")]
    static void ExportNavMeshToObj()
    {
        Vector3[] verts;
        int[] indes;

        NavMesh.Triangulate(out verts, out indes);

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = indes;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();


        GameObject navMeshFilter = new GameObject("navMeshFilter");
        MeshFilter meshFilter = (navMeshFilter.AddComponent<MeshFilter>() as MeshFilter);
        meshFilter.sharedMesh = mesh;


        MeshRenderer meshRenderer = navMeshFilter.AddComponent<MeshRenderer>();
        Material material = new Material(Shader.Find("Unlit/Transparent"));
        meshRenderer.sharedMaterial = material;

        string sceneId = EditorApplication.currentScene;
        int beignIndex = sceneId.LastIndexOf("/");
        int endIndex = sceneId.LastIndexOf(".");
        if (beignIndex == -1 || endIndex == -1) return;

        sceneId = sceneId.Substring(beignIndex + 1, endIndex - beignIndex - 1);

        String exportPath = EditorUtility.SaveFilePanel("Save Navmesh", "", sceneId, "obj");
        if (exportPath.Length > 0)
        {
            ObjExporter.MeshToFile(meshFilter, exportPath);
        }
    }
}

