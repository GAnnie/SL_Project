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

class PetFaceIconLoader
{
    private static readonly PetFaceIconLoader instance = new PetFaceIconLoader();
    public static PetFaceIconLoader Instance
    {
        get
        {
            return instance;
        }
    }
	
	static public Vector3 SIZE = new Vector3(60,60,1); //face2
	static public Vector3 SIZE3 = new Vector3( 88 , 88 , 1 );
	
    private Dictionary<int,KeyValuePair <Texture2D,int> > _ResourceMap;

    private PetFaceIconLoader()
    {
        _ResourceMap = new Dictionary<int, KeyValuePair <Texture2D, int>>();
	}
	
	public void LoadToUITexture(UITexture uiTexture, int petId){
		Texture2D texture2D = Load(petId);
		uiTexture.mainTexture = texture2D;
		//uiTexture.transform.localScale = SIZE;
	}	

	public void LoadToUITextrueForFace3( UITexture uiTexture, int petId )
	{
		Texture2D texture2D = LoadPetFace3(petId);
		uiTexture.mainTexture = texture2D;
		//uiTexture.transform.localScale = SIZE3;
	}

	private Texture2D LoadPetFace3( int petId )
	{
		if (petId > 10000){
			petId -= 10000;
		}
		Texture2D tex = null;
		if( petId < 0 )
		{
			tex =	ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace3/Face3_nothing" , "png") as Texture2D;
		}
		else
		{
			tex =	ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace3/Face3_" + petId, "png") as Texture2D;
		}
		 
		if (tex == null)
		{
			Debug.LogError(string.Format("face3_{0}不存在!", petId));
			string facestr = "face3_nothing";
			tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace3/"+facestr, "png") as Texture2D;
		}
		else
		{
			//KeyValuePair <Texture2D, int> pair = new KeyValuePair <Texture2D, int>(tex, 1);
			//_ResourceMap.Add(petId, pair);
		}
		
		return tex;
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