// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  PetFaceIcon1Loader.cs
// Author   : willson
// Created  : 2013/8/9 20:04:03
// Purpose  : 
// **********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PetFaceIcon1Loader
{
    private static readonly PetFaceIcon1Loader instance = new PetFaceIcon1Loader();
    public static PetFaceIcon1Loader Instance
    {
        get
        {
            return instance;
        }
    }

	static public Vector3 SIZE_1 = new Vector3(109,109,1); //face1
	static public Vector3 SIZE_3 = new Vector3(256,128,1); //face3
	static public Vector3 SIZE_4 = new Vector3(357,109,1); //face4
	
    private Dictionary<int, KeyValuePair<Texture2D, int>> _ResourceMap;
    private Dictionary<int, KeyValuePair<Texture2D, int>> _ResourcePopulationMap;

    private PetFaceIcon1Loader()
    {
        _ResourceMap = new Dictionary<int, KeyValuePair<Texture2D, int>>();
        _ResourcePopulationMap = new Dictionary<int, KeyValuePair<Texture2D, int>>();
	}
	
//	public void LoadToUITexture(UITexture uiTexture, int petId){
//		Texture2D texture2D = Load(petId);
//		uiTexture.mainTexture = texture2D;
//		uiTexture.transform.localScale = SIZE_1;
//	}
//		
//	
//    private Texture2D Load(int petId)
//    {
//		if (petId > 10000){
//			petId -= 10000;
//		}
//		
//        if (_ResourceMap.ContainsKey(petId) == true)
//        {
//            KeyValuePair<Texture2D, int> pair = _ResourceMap[petId];
//            _ResourceMap[petId] = new KeyValuePair<Texture2D, int>(pair.Key, pair.Value + 1);
//            return pair.Key;
//        }
//        else
//        {
//			Texture2D tex = null;
//			
//			if (petId == 0){
//	            tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace1/face_nothing", "png") as Texture2D;
//	            if (tex != null)
//	            {
//		            KeyValuePair<Texture2D, int> pair = new KeyValuePair<Texture2D, int>(tex, 1);
//		            _ResourceMap.Add(petId, pair);
//	            }			
//			}else{
//	            tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace1/" + AppUtils.getPetFaceIcon(petId), "png") as Texture2D;
//	            if (tex == null)
//	            {
//	                tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetFace1/face1_1001", "png") as Texture2D;
//	            }
//				else
//				{
//		            KeyValuePair<Texture2D, int> pair = new KeyValuePair<Texture2D, int>(tex, 1);
//		            _ResourceMap.Add(petId, pair);
//				}				
//			}
//
//			
//			return tex;							
//        }
//    }
//
//    public void UnLoad(int petId)
//    {
//		if (petId > 10000){
//			petId -= 10000;
//		}		
//		
//        if (_ResourceMap.ContainsKey(petId) == true)
//        {
//            KeyValuePair<Texture2D, int> pair = _ResourceMap[petId];
//            if (pair.Value > 0)
//            {
//                _ResourceMap[petId] = new KeyValuePair<Texture2D, int>(pair.Key, pair.Value - 1);
//            }
//            else
//            {
//                KeyValuePair<Texture2D, int> pair2 = _ResourceMap[petId];
//                Texture2D tex = pair.Key;
//                if (tex != null)
//                {
//                    Resources.UnloadAsset(tex);
//                }
//                _ResourceMap.Remove(petId);
//            }
//        }
//    }
//	
//
//	public void LoadPopulationToUITexture(UITexture uiTexture, Pet pet){
//		Texture2D texture2D = LoadPopulation(pet);
//		uiTexture.mainTexture = texture2D;
//		if (pet.population == Monster.PopulationType_Medium)
//		{
//			uiTexture.transform.localScale = SIZE_3;		
//		}
//		else if (pet.population == Monster.PopulationType_Big)
//		{
//			uiTexture.transform.localScale = SIZE_4;		
//		}
//		else{
//			uiTexture.transform.localScale = SIZE_1;		
//		}
//	}	
//	
//    private Texture2D LoadPopulation(Pet pet)
//    {
//        if (pet.population == Monster.PopulationType_Small)
//        {
//            return Load(pet.id);
//        }
//        else
//        {
//            if (_ResourcePopulationMap.ContainsKey(pet.id) == true)
//            {
//                KeyValuePair<Texture2D, int> pair = _ResourcePopulationMap[pet.id];
//                _ResourcePopulationMap[pet.id] = new KeyValuePair<Texture2D, int>(pair.Key, pair.Value + 1);
//                return pair.Key;
//            }
//            else
//            {
//				string faceStr = "";
//				if (pet.population == Monster.PopulationType_Medium){
//					faceStr = "face3_"+pet.id;
//				}else if (pet.population == Monster.PopulationType_Big){
//					faceStr = "face4_"+pet.id;
//				}
//				
//                Texture2D tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetLineupFace/" + faceStr, "png") as Texture2D;
//                if (tex == null)
//                {
//					if (pet.population == Monster.PopulationType_Medium){
//						faceStr = "face3_1002";
//					}else if (pet.population == Monster.PopulationType_Big){
//						faceStr = "face4_1003";
//					}
//					
//                    tex = ResourceLoader.Load(PathHelper.IMAGES_PATH + "PetLineupFace/" + faceStr, "png") as Texture2D;
//                }
//				else
//				{
//	                KeyValuePair<Texture2D, int> pair = new KeyValuePair<Texture2D, int>(tex, 1);
//	                _ResourcePopulationMap.Add(pet.id, pair);					
//				}
//
//                return tex;
//            }
//        }
//    }
//
//    public void UnLoadPopulation(Pet pet)
//    {
//        if (pet.population == Monster.PopulationType_Small)
//        {
//            UnLoad(pet.id);
//        }
//        else
//        {
//            if (_ResourcePopulationMap.ContainsKey(pet.id) == true)
//            {
//                KeyValuePair<Texture2D, int> pair = _ResourcePopulationMap[pet.id];
//                if (pair.Value > 0)
//                {
//                    _ResourcePopulationMap[pet.id] = new KeyValuePair<Texture2D, int>(pair.Key, pair.Value - 1);
//                }
//                else
//                {
//                    KeyValuePair<Texture2D, int> pair2 = _ResourcePopulationMap[pet.id];
//                    Texture2D tex = pair.Key;
//                    if (tex != null)
//                    {
//                        Resources.UnloadAsset(tex);
//                    }
//                    _ResourcePopulationMap.Remove(pet.id);
//                }
//            }
//        }
//    }
}