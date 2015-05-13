using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class LocalLoadCom : MonoBehaviour 
{
    private string _downLoadPath = string.Empty;

    private string _assetsTargetPath = string.Empty;

    private const string unity3dEx = ".unity3d";
    public void Setup(string assetsTargetPath ,string downLoadPath)
    {
        this._assetsTargetPath = assetsTargetPath;
        this._downLoadPath = downLoadPath;
    }

    public void LoadLocalAssets(Queue<AssetPackage> needToUpdatePrefebQueue, 
                                Action< AssetPackage > localUpdatePrefab, 
                                Action< string > errorPrefabFunc, 
                                Action localFinish )
    {
        StartCoroutine(_LoadLocalAssets(needToUpdatePrefebQueue, localUpdatePrefab, errorPrefabFunc, localFinish));
    }

    private int returnNumber = 10;
    private IEnumerator _LoadLocalAssets(Queue<AssetPackage> _needToUpdatePrefebQueue,
                                         Action< AssetPackage > localUpdatePrefab,
                                         Action<string> errorPrefabFunc, 
                                         Action localFinish )
    {
        if (_needToUpdatePrefebQueue != null)
        {
            int number = 0;
            while (_needToUpdatePrefebQueue.Count > 0)
            {
                number++;
                AssetPackage updatePrefab = _needToUpdatePrefebQueue.Dequeue();

                string url      = _assetsTargetPath + updatePrefab.name + unity3dEx;
                string downPath = _downLoadPath + updatePrefab.name + unity3dEx;

                bool result = false;
                if (Application.platform != RuntimePlatform.Android)
                {
                    result = CopyAssetData(url, downPath, !updatePrefab.assetPrefab.needDecompress);
                }
                else
                {
                    result = BaoyugameSdk.copyAssetAs(url, downPath, !updatePrefab.assetPrefab.needDecompress ); 
                }

                if (!result)
                {
                    //GameDebuger.Log("Copy Asset of " + updatePrefab.assetName + "Error ");
                    if (errorPrefabFunc != null) errorPrefabFunc(updatePrefab.name);
                }
                else
                {
                    if (localUpdatePrefab != null) localUpdatePrefab(updatePrefab);
                }

                if (number >= returnNumber)
                {
                    number = 0;
                    GC.Collect();
                    yield return new WaitForEndOfFrame();
                }
            }

            if (localFinish != null) localFinish();
        } 
    }


    /// <summary>
    /// 获取资源数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private bool CopyAssetData(string sourcePath, string destPath, bool isUncompress = false )
    {
        //GameDebuger.Log(sourcePath);
        if (File.Exists(sourcePath))
        {
            try
            {
                //GameDebuger.Log("Copying " + sourcePath);

                string destFolder = destPath.Substring(0, destPath.LastIndexOf("/"));
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                if (File.Exists(destPath))
                {
                    File.Delete(destPath);
                }

                
                FileStream stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader reader = new BinaryReader(stream);
                byte[] buffer = reader.ReadBytes((int)stream.Length);
                reader.Close();
                stream.Close();

                stream = new FileStream(destPath, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(stream);

                if (isUncompress)
                {
                    buffer = ZipLibUtils.Uncompress(buffer);
                }

                writer.Write(buffer);
                writer.Close();
                stream.Close();

				iOSUtility.ExcludeFromBackupUrl(destPath);

                return true;

            }
            catch (IOException e)
            {
                Debug.LogException(e);
                //Debug.LogException(e);
            }
        }

        return false;
    }


}
