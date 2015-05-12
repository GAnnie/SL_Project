// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  Scripts/Proxy/ProxyBattleReady
// Author   : LP
// Created  : 2013/1/31
// Purpuse  : 
// **********************************************************************
// 
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class TextureImporterSettingsWin : EditorWindow
{
	[MenuItem("OptimizationTool/Texture Settings")]
    public static void ShowWin()
    {
        EditorWindow.GetWindow(typeof(TextureImporterSettingsWin));
        curModel = Texture_Setting_Model.CHANG_MODEL;
    }

    public static void ShowWinWithCheck()
    {
        EditorWindow.GetWindow(typeof(TextureImporterSettingsWin));
        curModel = Texture_Setting_Model.CHECK_MODEL;
    }

    private enum Texture_Setting_Model
    {
        CHECK_MODEL,
        CHANG_MODEL
    }

    private static Texture_Setting_Model curModel = Texture_Setting_Model.CHANG_MODEL;


    private enum SetTextureType
    {
        Type_Texture,
        Type_GUI
    }

    private struct TextureInfo
    {
        public string path;

        public SetTextureType type;

        public TextureInfo(string path, SetTextureType type)
        {
            this.path = path;

            this.type = type;
        }
    }

	//All Texture List
    private List<TextureInfo> allTexturePath = null;
	
	//Texture Size Level 
	private int textureSizeLevel = 0;
	//Min Texture Size
	private const int minTextureSizeLevel = 0;
	//MAX Texture Size
	private const int maxTextureSizeLevel = 4;

    private bool isAllChange = false;

	private Vector2 totalscrollPos;
	private const string ResourcePath = "GameResources/ArtResources/Models";
	private const string EffectPath   = "GameResources/ArtResources/Effects";
	private const string ImagesPath   = "GameResources/ArtResources/Images";
    //private const string UIPath       = "UI";
    private string[] NeglectPath = new string[2]
    {
        "Assets/GameResources/ArtResources/Models/Scene/Object/Terrain/Textures",
		"Assets/GameResources/ArtResources/Images"
    };

	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		{
			totalscrollPos = EditorGUILayout.BeginScrollView(totalscrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));
			{
				ShowAllTexturePathList();

                if (curModel == Texture_Setting_Model.CHANG_MODEL)
                {
                    ShowTextureLevel();
                    ShowTextureFormat();

                    GUI.color = Color.green;
                    if (GUILayout.Button(" ===== 筛选图片格式进行转换 ======= ", GUILayout.Height(50)))
                    {
                        isAllChange = false;
                        ChangeTextureSize();
                    }

                    GameEditorUtils.Space(2);
                    GUI.color = Color.red;
                    if (GUILayout.Button(" ===== 状况所有图片 ========", GUILayout.Height(50)))
                    {
                        isAllChange = true;
                        ChangeTextureSize();
                    }

                }
                else if ( curModel == Texture_Setting_Model.CHECK_MODEL )
                {
                    ShowCurrentTextureFormat();
                }
				GUI.color = Color.white;
			}
			EditorGUILayout.EndScrollView();
		}
		EditorGUILayout.EndVertical();
	}
			
	/// <summary>
	/// Show All Texture Path
	/// </summary>
	private Vector2 totalscrollPos2;
	private void ShowAllTexturePathList()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField( " ====================================== " );		
		if ( allTexturePath!= null )
		{
				totalscrollPos2 = EditorGUILayout.BeginScrollView(totalscrollPos2, GUILayout.Width(800), GUILayout.Height(200));
				{
					foreach( TextureInfo info in allTexturePath )
					{
						EditorGUILayout.LabelField(info.path );							
					}
				}
				EditorGUILayout.EndScrollView();
		}
		else
		{
			GUI.color = Color.red;
			EditorGUILayout.LabelField( "Click < Update Button >  to Update All Texture Message !!" );	
			GUI.color = Color.white;
		}
		EditorGUILayout.LabelField( " ====================================== " );
		
		
		if( GUILayout.Button( "Update All Texture" ))
		{
			string path = Application.dataPath + "/"+ ResourcePath ;
			string effectPath = Application.dataPath + "/"+ EffectPath ;
            string imagesPath = Application.dataPath + "/" + ImagesPath;
            //string uiPath     = Application.dataPath + "/" + UIPath;
            allTexturePath = new List<TextureInfo>();
            allTexturePath.AddRange(UpdateGroup(path, SetTextureType.Type_Texture));
            allTexturePath.AddRange(UpdateGroup(effectPath, SetTextureType.Type_Texture));
            allTexturePath.AddRange(UpdateGroup(imagesPath, SetTextureType.Type_GUI));
            //allTexturePath.AddRange(UpdateGroup(uiPath, SetTextureType.Type_GUI));

            if (checkTextureFormatList != null) checkTextureFormatList.Clear();
            checkTextureFormatList = null;
		}				
	}

    private List<TextureInfo> UpdateGroup(string path, SetTextureType type)
    {
        List<TextureInfo> textureInfoList = new List<TextureInfo>();
        List<string> pathList = GameEditorUtils.GetAssetsList(path, "png", "jpg", "tga");

        foreach( string textruePath in pathList )
        {
            textureInfoList.Add(new TextureInfo(textruePath, type));
        }

        return textureInfoList;
    }

    /// <summary>
    /// 显示当前贴图的格式
    /// </summary>

    private class Check_Texture_Format
    {
        public UnityEngine.Object obj;
        public TextureImporterFormat format;

        public Check_Texture_Format(UnityEngine.Object obj, TextureImporterFormat format)
        {
            this.obj = obj;
            this.format = format;
        }
    }

    private bool isTextureChang = false;
    private Vector2 scrollPos4;
    private List<Check_Texture_Format> checkTextureFormatList = null;
    private void ShowCurrentTextureFormat()
    {
        if (allTexturePath == null)
        {
            return;
        }

        if (isTextureChang)
        {
            checkTextureFormatList.Clear();
            checkTextureFormatList = null;
        }


        if (checkTextureFormatList == null)
        {
            checkTextureFormatList = new List<Check_Texture_Format>();
            foreach (TextureInfo texturePath in allTexturePath)
            {

                UnityEngine.Object texture = AssetDatabase.LoadMainAssetAtPath(texturePath.path);

                TextureImporter importer = TextureImporter.GetAtPath(texturePath.path) as TextureImporter;
                if (importer == null)
                {
                    Debug.Log(" Can not get TextureImporter : " + texturePath);
                    continue;
                }

                int maxTextureSize = 0;
                TextureImporterFormat format;
                int compressQulity = 0;

                importer.GetPlatformTextureSettings("Android", out maxTextureSize, out format, out compressQulity);

                checkTextureFormatList.Add(new Check_Texture_Format(texture, format));
            }
        }


        GameEditorUtils.ShowList<Check_Texture_Format>(checkTextureFormatList, ShowText_format, ref scrollPos4, 800, 400);

    }

    private void ShowText_format(Check_Texture_Format textureFormat )
    {
        if (textureFormat.obj == null)
        {
            isTextureChang = true;
            return;
        }
        EditorGUILayout.BeginHorizontal();
        {
            if (textureFormat.format != TextureImporterFormat.RGBA16 && textureFormat.format != TextureImporterFormat.ETC_RGB4)
            {
                GUI.color = Color.red;
            }
            else
            {
                GUI.color = Color.green;
            }

            EditorGUILayout.LabelField(textureFormat.obj.name, GUILayout.Width(200));
            EditorGUILayout.LabelField("----------------------", GUILayout.Width(100));
            if (GUILayout.Button("Select", GUILayout.Width(100)))
            {
                GameEditorUtils.LocalToObjbect(textureFormat.obj);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

	/// <summary>
	/// Shows the texture level.
	/// </summary>
	private  void ShowTextureLevel()
	{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField( "TextureSize Reduce Level ---- "); 
			EditorGUILayout.LabelField( "0 : Use Original Size of Texture || 1 : Use half of texture size ||  And So on	" );
			textureSizeLevel = EditorGUILayout.IntSlider( "TextureSize Reduce Level ",
														 textureSizeLevel,
														 minTextureSizeLevel,
														 maxTextureSizeLevel);
			EditorGUILayout.Space();
	}


    private enum Alpha_Texture_Format_Type
    {
        Texture_DXT_5,
        Texutre_RGBA_16
    }
    
    private Alpha_Texture_Format_Type curTextureFormatType = Alpha_Texture_Format_Type.Texutre_RGBA_16;
   
    
    /// <summary>
    /// Show texture format
    /// </summary>
    private void ShowTextureFormat()
    {
        GameEditorUtils.Space(2);
        EditorGUILayout.LabelField("TextureSize format ---- ");
        curTextureFormatType = (Alpha_Texture_Format_Type)EditorGUILayout.EnumPopup("Texture Format : ", curTextureFormatType);
        GameEditorUtils.Space(2);
    }

	
	private void ChangeTextureSize ()
	{
        if ( allTexturePath == null )
        {
            return ;			
        }
		
		
		int index = 0;
		//TextureInfo texturePath = new TextureInfo("Assets/GameResources/ArtResources/Images/PetFace2/face2_2.png",SetTextureType.Type_GUI);
        //string texturePath = "Assets/GameResources/ArtResources/Models/Scene/Object/Build/1408/texture/build_1408_tex_001.png";
        
		foreach( TextureInfo texturePath in allTexturePath )
		{
			index ++;
			Texture texture = AssetDatabase.LoadMainAssetAtPath( texturePath.path ) as Texture;
			TextureImporter importer = TextureImporter.GetAtPath( texturePath.path ) as TextureImporter;
			if ( importer  == null )
			{
                Debug.LogError( " Can not get TextureImporter : " + texturePath );
                continue;
			}
			
			
			int originalWith   = 0;
			int originalHeight = 0;
			int maxTextureSize = 0;
			TextureImporterFormat format;
			int compressQulity = 0;
            bool isNeglect = false;
            bool isNpotPic = false;
			
			
			//判断是否过滤压缩的图片
			string folder = texturePath.path.Substring(0, texturePath.path.LastIndexOf("/"));
			foreach(string path in NeglectPath )
            {
                if( folder.StartsWith(path) )
                {
                    isNeglect = true;
                    break;
                }
            }
			
			
			//**********************************************************************************************************
			// 如果是安卓平台， SetTextureType == Type_GUI, 则直接设置为TextureImporterType.GUI, 并且是不会进行大小压缩
			//				  SetTextureType == Type_Texture, 则设置为TexuurImporterType.Advanced, 大小根据具体的图片
			//				  调整信息来调整。
			//**********************************************************************************************************
			//如果苹果平台， SetTextureType == Type_GUI 和 etTextureType == Type_Texture 的贴图都是设置成为TexuurImporterType.Advanced
			//				的类型， 并且如果贴图不是2的N次幂， 这需要进行ToNear的设置
			//**********************************************************************************************************
			
#if UNITY_ANDROID
			if (!isAllChange)
            {
                if (importer.textureType == TextureImporterType.Advanced && texturePath.type == SetTextureType.Type_Texture)
                {
                    continue;
                }
				
                if (importer.textureType == TextureImporterType.GUI && texturePath.type == SetTextureType.Type_GUI)
                {
                    continue;
                }
            }
			
			
			if( texturePath.type == SetTextureType.Type_Texture )
			{
				importer.textureType = TextureImporterType.Advanced;
			}
			else if( texturePath.type == SetTextureType.Type_GUI )
			{
				importer.textureType = TextureImporterType.GUI;
			}
			
			
#elif UNITY_IPHONE
			if (!isAllChange)
            {
                if (importer.textureType == TextureImporterType.Advanced)
                {
                    continue;
                }
            }		
			
	        importer.textureType = TextureImporterType.Advanced;
            if (texturePath.type == SetTextureType.Type_GUI)
            {
                importer.mipmapEnabled = false;
            }
#endif
			
			
            GetTexutreOriginalSize(importer, ref originalWith, ref originalHeight);
            isNpotPic = IsPowerOfTwo(originalWith, originalHeight);

			string substring = texturePath.path.Substring(texturePath.path.LastIndexOf("/")+1);
			substring        = substring.Substring( 0, substring.IndexOf( ".") );
			
			if( substring.ToLower().EndsWith("_not"))
			{
            	maxTextureSize = GetChangeTextureSize(originalWith > originalHeight ? originalWith : originalHeight,  0);
			}
			else
			{
            	maxTextureSize = GetChangeTextureSize(originalWith > originalHeight ? originalWith : originalHeight, isNeglect ? 0 : textureSizeLevel);
			}


#if UNITY_ANDROID
            importer.SetPlatformTextureSettings("Android", maxTextureSize, TextureImporterFormat.AutomaticCompressed);
#elif UNITY_IPHONE
            if (isNpotPic)
            {
                importer.npotScale = TextureImporterNPOTScale.ToNearest;
            }

			importer.SetPlatformTextureSettings("iPhone",  maxTextureSize,TextureImporterFormat.AutomaticCompressed );
#endif
			
			
			
			
			
			

            AssetDatabase.ImportAsset(texturePath.path,ImportAssetOptions.ForceUpdate );
            EditorUtility.DisplayProgressBar("Transform ",
                                             "Transform texture......." + index.ToString() + "/" + allTexturePath.Count.ToString(),
                                             (float)(index) / allTexturePath.Count);
		}
		
		EditorUtility.ClearProgressBar();
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		
		EditorUtility.DisplayDialog( "Tips " , "Transform finish " , "OK");
	}
	
	
	/// <summary>
	/// Gets the size of the texutre original.
	/// </summary>
	/// <param name='imp'>
	/// Imp.
	/// </param>
	/// <param name='wight'>
	/// Wight.
	/// </param>
	/// <param name='height'>
	/// Height.
	/// </param>
	private void GetTexutreOriginalSize( TextureImporter importer , ref int wight , ref int height )
	{
		//============================= GET Original size of Textur ======================================== 
		
		
		MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance );
		
		object[] args = new object[2] { 0, 0 };
	
		mi.Invoke(importer, args);
		
        wight = (int)args[0];
        height = (int)args[1];
		//==================================================================================================
	}


    /// 判断是否2的N次幂
    /// <param name="originWith"></param>
    /// <param name="originHeight"></param>
    /// <returns></returns>
    private bool IsPowerOfTwo(int originWith, int originHeight)
    {
        return !((isN(originWith)) && (isN(originHeight)));
    }

	/// <summary>
	/// Gets the size of the change texture.
	/// </summary>
	/// <returns>
	/// The change texture size.
	/// </returns>
	/// <param name='originalsize'>
	/// Originalsize.
	/// </param>
	/// <param name='level'>
	/// Level.
	/// </param>
    private int GetChangeTextureSize(int originalsize, int level)
	{
		int returnSize = 0;
		if ( isN(originalsize) )//( originalsize % 2 ) == 0 )
		{
			if ( level == 0) 	
				returnSize = originalsize;
			else 
			{
				returnSize = originalsize /  ( 1 << (level));
			}
		}
		else
		{
			int numLevel = 0;
			while( originalsize > 0)
			{
				originalsize = originalsize >> 1;
				numLevel ++;
			}
			
			returnSize = ( 1 << ( numLevel ) );

            if (level > 0)
			{
				returnSize = ( 1 << ( numLevel) ) / ( 1 << (level));
			}
		}
		return returnSize;
	}
	
	
	private bool isN( int n )	
	{
		return n>0?((n & ((~n) + 1))==n?true:false):false;
	}
	
}


