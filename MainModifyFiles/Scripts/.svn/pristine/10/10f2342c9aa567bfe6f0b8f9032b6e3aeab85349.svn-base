// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  PetFaceIcon2Loader.cs
// Author   : senkay
// Created  : 2013/8/23 20:04:03
// Purpose  : 
// **********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PetFaceIcon2Loader
{
    private static readonly PetFaceIcon2Loader instance = new PetFaceIcon2Loader();
    public static PetFaceIcon2Loader Instance
    {
        get
        {
            return instance;
        }
    }
	
	static public Vector3 SIZE = new Vector3(60,60,1); //face2
	
    private Dictionary<int,KeyValuePair <Texture2D,int> > _ResourceMap;

    private PetFaceIcon2Loader()
    {
        _ResourceMap = new Dictionary<int, KeyValuePair <Texture2D, int>>();
	}
	
	public void LoadToUITexture(UITexture uiTexture, int petId){
		Texture2D texture2D = Load(petId);
		uiTexture.mainTexture = texture2D;
		uiTexture.transform.localScale = SIZE;
	}	
	
    private Texture2D Load(int petId)
    {
		if (petId > 10000){
			petId -= 10000;
		}		
		
//        if (_ResourceMap.ContainsKey(petId) == true)
//        {
//            KeyValuePair <Texture2D, int> pair = _ResourceMap[petId];
//            _ResourceMap[petId] = new KeyValuePair<Texture2D, int>(pair.Key, pair.Value + 1);
//            return pair.Key;
//        }
//        else
//        {
//            Texture2D tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace2/face2_" + petId, "png") as Texture2D;
//            if (tex == null)
//            {
//				string facestr = "face2_1001";
//                tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace2/"+facestr, "png") as Texture2D;
//            }
//			else
//			{
//	            KeyValuePair <Texture2D, int> pair = new KeyValuePair <Texture2D, int>(tex, 1);
//	            _ResourceMap.Add(petId, pair);
//			}
//
//            return tex;
//        }

	    Texture2D tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace2/face2_" + petId, "png") as Texture2D;
	    if (tex == null)
	    {
			Debug.LogError(string.Format("face2_{0}不存在!", petId));
			string facestr = "face2_nothing";
	        tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace2/"+facestr, "png") as Texture2D;
	    }
		else
		{
            //KeyValuePair <Texture2D, int> pair = new KeyValuePair <Texture2D, int>(tex, 1);
            //_ResourceMap.Add(petId, pair);
		}

	    return tex;
    }

    public void UnLoad(int petId)
    {
		if (petId > 10000){
			petId -= 10000;
		}		
		
        if (_ResourceMap.ContainsKey(petId) == true)
        {
            KeyValuePair <Texture2D, int> pair = _ResourceMap[petId];
            if (pair.Value > 0)
            {
                _ResourceMap[petId] = new KeyValuePair<Texture2D, int>(pair.Key, pair.Value - 1);
            }
            else
            {
	            KeyValuePair <Texture2D, int> pair2 = _ResourceMap[petId];
	            Texture2D tex = pair.Key;
				if (tex != null){
					Resources.UnloadAsset(tex);
				}
                _ResourceMap.Remove(petId);
            }
        }
    }
}