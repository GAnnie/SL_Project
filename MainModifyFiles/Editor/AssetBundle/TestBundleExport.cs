// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TestBundleExport.cs
// Author   : wenlin
// Created  : 2013/4/8 10:27:08
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

class TestBundleExport
{
    [MenuItem("AssetBundle/Export One Scene Assetbundle")]
    static void ExportResourcexb()
    {
        string path;
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (objs.Length == 1)
        {
            string []levels = new string[1]{ AssetDatabase.GetAssetPath(objs[0]) };
            path = EditorUtility.SaveFilePanel("Save Resource", "",  Selection.activeObject.name, "unity3d");
            if (path.Length <= 0)
            {
                return;
            }

            //BuildPipeline.BuildAssetBundle(Selection.activeObject, objs, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
            BuildPipeline.BuildStreamedSceneAssetBundle( levels, path, BuildTarget.Android);

        }

    }

    [MenuItem("AssetBundle/Build AssetBundle From Selection")]
    static void ExportResourceNoTrackUncompressedAssetBundle () 
    {
	    // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", Selection.activeObject.name, "unity3d");
	    if (path.Length != 0)
	    {
		    // Build the resource file from the active selection.
		    BuildPipeline.BuildAssetBundle(Selection.activeObject, 
									       Selection.objects, 
									       path,
									       BuildAssetBundleOptions.CollectDependencies | 
									       BuildAssetBundleOptions.CompleteAssets | 
									       BuildAssetBundleOptions.UncompressedAssetBundle,
									       BuildTarget.Android  );

            int index = path.LastIndexOf(".");
            if (index != -1)
            {
                string compressPath = path.Substring(0, index);
                LZMA_Util.CompressFileLZMA(path, compressPath);
            }

	    }
    }

    [MenuItem("AssetBundle/Compress Assets ")]
    static void CompressAsset()
    {
        string path = EditorUtility.OpenFilePanel("Select Resource", "", "");

        Debug.Log(path);
        if ( !string.IsNullOrEmpty( path ))
        {
            int index = path.IndexOf(".");
            if (index != -1)
            {
                string leftString = path.Substring(0, index);
                string rightString = path.Substring(index + 1);
                string compressPath = leftString + "_compress" + rightString;
                Debug.Log("compressPath : " + compressPath);
                LZMA_Util.CompressFileLZMA(path, compressPath);
                Debug.Log("Compress Asset Finish");
            }
        }
    }

    [MenuItem("AssetBundle/Uncompress Assets ")]
    static void UncompressAsset()
    {
        string path = EditorUtility.OpenFilePanel("Select Resource", "", "");
        if (path.Length != 0)
        {
            int index = path.IndexOf(".");
            if (index != -1)
            {
                string leftString  = path.Substring(0, index);
                string rightString = path.Substring(index + 1);
                string uncompressPath = leftString + "_uncompress" + rightString;
                
                //LZMA_Util.DecompressFileLZMA(path, uncompressPath);
                ZipLibUtils.UnCompressFile(path, uncompressPath);

                Debug.Log("Uncompress Asset Finish");
            }

        }
    }


    [MenuItem("AssetBundle/Split MergePackage")]
    static void SplitMergePackage()
    {
        string filePath = EditorUtility.OpenFilePanel("请选择合拼包", "", "baoyu");
        if (!string.IsNullOrEmpty(filePath))
        {
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open);
                br = new BinaryReader(fs);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return;
            }


            if (fs != null && br != null)
            {
                byte[] bytes = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();

                MergeAssetParser parser = new MergeAssetParser(bytes);

                if (parser.isCorrect)
                {
                    Debug.Log("Asset Number : " + parser.assetNumber);
                    Debug.Log(" success ");

                    string exportFolder = filePath.Substring( 0,filePath.LastIndexOf(".")) + "(SplitFile)/" ;
                    if( !Directory.Exists( exportFolder ))
                    {
                        Directory.CreateDirectory( exportFolder );
                    }

                    for (int i = 0; i < parser.assetNumber; i++)
                    {
                        EditorUtility.DisplayProgressBar("正在进行资源拆分....",
                                                "(" + i + " / " + parser.assetNumber + ")",
                                                ((float)i) / parser.assetNumber);

                        byte[] buff = parser.GetAssetBuffs(i);
                        if (buff != null)
                        {
                            string exportFilePath = exportFolder + i + ".unity3d";
                            try
                            {
                                FileStream fileStream = new FileStream(exportFilePath, FileMode.Create);
                                fileStream.Write(buff, 0, buff.Length);

                                fileStream.Close();
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    }
                    EditorUtility.ClearProgressBar();
                }
            }
        }
    }

}
