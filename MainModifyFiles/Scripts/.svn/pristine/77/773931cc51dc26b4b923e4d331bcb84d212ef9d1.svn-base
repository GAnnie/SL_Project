// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  ALLShaderLoader.cs
// Author   : wenlin
// Created  : 2013/7/2 
// Purpose  : 
// **********************************************************************
using System;
using UnityEngine;

public class ALLShaderLoader
{
    private static ALLShaderLoader _instance = null;
    public static ALLShaderLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ALLShaderLoader();
            }
            return _instance;
        }
    }

    private ALLShaderLoader() 
    {

    }
	
	
	private AssetBundle allShaderAssetBundle = null;
	private UnityEngine.Object allShaderObj = null;
	public void Setup( AssetBundle allShaderLoader)
	{
		allShaderAssetBundle = allShaderLoader;
        //allShaderObj = allShaderAssetBundle.mainAsset;

//#if UNITY_DEBUG
//        GameDebuger.Log( " =========Before Warmup ==================" );
//        UnityEngine.Object []objs =  Resources.FindObjectsOfTypeAll(typeof(Shader));
//        foreach (UnityEngine.Object obj in objs)
//        {
//            GameDebuger.Log( "Shader : " + obj.name);
//        }
//        GameDebuger.Log( "END" );
//        DateTime dateTime = DateTime.Now;
//#endif

        //WarmUp All Shader
        //Shader.WarmupAllShaders();


//#if UNITY_DEBUG
//        TimeSpan span = DateTime.Now.Subtract(dateTime);
//        GameDebuger.Log("Warmup Shader Time : " + span.Milliseconds.ToString() + "( ms )");

//        GameDebuger.Log(" =========After Warmup ==================");
//        objs = Resources.FindObjectsOfTypeAll(typeof(Shader));
//        foreach (UnityEngine.Object obj in objs)
//        {
//            GameDebuger.Log("Shader : " + obj.name);
//        }
//        GameDebuger.Log("END");
//#endif
	}
	
	
	/// <summary>
	/// Releases all resource used by the <see cref="ALLShaderLoader"/> object.
	/// </summary>
	/// <remarks>
	/// Call <see cref="Dispose"/> when you are finished using the <see cref="ALLShaderLoader"/>. The <see cref="Dispose"/>
	/// method leaves the <see cref="ALLShaderLoader"/> in an unusable state. After calling <see cref="Dispose"/>, you must
	/// release all references to the <see cref="ALLShaderLoader"/> so the garbage collector can reclaim the memory that
	/// the <see cref="ALLShaderLoader"/> was occupying.
	/// </remarks>
	public void Dispose()
	{
		if ( allShaderObj != null )
		{
            allShaderObj = null;
			GameObject.DestroyImmediate( allShaderObj );
		}
		
		if ( allShaderAssetBundle != null )
		{
			allShaderAssetBundle.Unload( true );
		}
		
	}
}

