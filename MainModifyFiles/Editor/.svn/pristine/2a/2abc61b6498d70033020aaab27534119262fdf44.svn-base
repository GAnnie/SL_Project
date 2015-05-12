using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public static class EditorHelper
{
	#region GUILayout Helper
	public static void DrawBoxLabel (string title, string text, bool hasCopyBtn, bool needLayout = true)
	{
		if (needLayout) EditorGUILayout.BeginHorizontal ();

		GUILayout.Box (title);
		GUILayout.Space (5f);
		GUILayout.Box (text);
		if (hasCopyBtn && GUILayout.Button ("Copy", GUILayout.Width (60f))) {
			EditorGUIUtility.systemCopyBuffer = text;
		}

		if (needLayout) EditorGUILayout.EndHorizontal ();
	}
	
	static public bool DrawHeader (string text) { return DrawHeader(text,text,false,false); }
	
	static public bool DrawHeader (string text, string key, bool forceOn, bool minimalistic)
	{
		bool state = EditorPrefs.GetBool(key, true);

		GUILayout.Space(3f);
		if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
		GUILayout.BeginHorizontal();
		GUI.changed = false;

		text = "<b><size=11>" + text + "</size></b>";
		if (state) text = "\u25BC " + text;
		else text = "\u25BA " + text;
		if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;

		if (GUI.changed) EditorPrefs.SetBool(key, state);

		GUILayout.Space(2f);
		GUILayout.EndHorizontal();
		GUI.backgroundColor = Color.white;
		if (!forceOn && !state) GUILayout.Space(3f);
		return state;
	}

	static public string DrawTextField(string title, string value, int width=50, bool needLayout = true)
	{
		if (needLayout) EditorGUILayout.BeginHorizontal ();

		GUILayout.Box (title);
		//EditorGUILayout.LabelField (title, GUILayout.MaxWidth(50));
		string newValue = EditorGUILayout.TextField ("", value, GUILayout.MaxWidth(width), GUILayout.Height(20));

		if (needLayout) EditorGUILayout.EndHorizontal ();
		return newValue;
	}
	
	static public int DrawIntField(string title, int value, int width=30, bool needLayout = true)
	{
		if (needLayout) EditorGUILayout.BeginHorizontal ();

		GUILayout.Box (title);
		int newValue = EditorGUILayout.IntField ("", value, GUILayout.MaxWidth(width),GUILayout.Height(20));

		if (needLayout) EditorGUILayout.EndHorizontal ();
		return newValue;
	}

	static public float DrawFloatField(string title, float value, int width=30, bool needLayout = true)
	{
		if (needLayout) EditorGUILayout.BeginHorizontal ();

		GUILayout.Box (title);
		float newValue = EditorGUILayout.FloatField ("", value, GUILayout.MaxWidth(width),GUILayout.Height(20));

		if (needLayout) EditorGUILayout.EndHorizontal ();

		return newValue;
	}

	static public bool DrawToggle(string title, bool value, int width=30, bool needLayout = true)
	{
		if (needLayout) EditorGUILayout.BeginHorizontal ();

		GUILayout.Box (title);
		bool newValue = EditorGUILayout.Toggle ("", value, GUILayout.MaxWidth(width),GUILayout.Height(20));

		if (needLayout) EditorGUILayout.EndHorizontal ();
		return newValue;
	}
	#endregion

	/// <summary>
	/// Creates the scriptable object asset.
	/// </summary>
	public static void CreateScriptableObjectAsset(ScriptableObject asset,string path,string fileName)
	{
		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		AssetDatabase.CreateAsset (asset,path + fileName + ".asset");
		
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Selection.activeObject = asset;
    }

	/// <summary>
	/// Loads the scriptable object asset.
	/// </summary>
	public static T LoadScriptableObjectAsset<T>(string fileName) where T :ScriptableObject
	{
		return AssetDatabase.LoadAssetAtPath(fileName+".asset",typeof(T)) as T;
	}	

	/// <summary>
	/// 获取指定目录下， 所有指定扩展名的文件名
	/// </summary>
	/// <param name="path"></param>
	/// <param name="extensions"></param>
	/// <returns></returns>
	public static List<string> GetAssetsList(string path, params string[] extensions)
	{
		if (!Directory.Exists(path))
		{
			Debug.Log("Directory  : " + path + " is not Exist");
			return null;
		}
		
		List<string> nameList = new List<string>();
		DirectoryInfo info = new DirectoryInfo(path);
		ListFiles(info, nameList, "Assets", true, extensions);
		return nameList;
	}
	
	public static List<string> GetAssetsList(string path, string beignString, bool needContainExtension,  params string[] extensions)
	{
		if (!Directory.Exists(path))
		{
			Debug.Log("Directory  : " + path + " is not Exist");
			return null;
		}
		
		List<string> nameList = new List<string>();
		DirectoryInfo info = new DirectoryInfo(path);
		ListFiles(info, nameList, beignString, needContainExtension, extensions);
		return nameList;
	}
	
	
	private static void ListFiles(FileSystemInfo info, List<string> nameList, string beignString, bool needContainExtension, params string[] extensions)
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
					
					string[] fileNames = file.FullName.Split('.');
					string fileNameExtension = fileNames[fileNames.Length - 1];
					fileNameExtension = fileNameExtension.ToLower();
					
					foreach (string ext in extensions)
					{
						if (fileNameExtension == ext)
						{
							string fileReturnName = string.Empty;
							
							if( needContainExtension )
							{
								fileReturnName = file.FullName;
							}
							else
							{
								int index = file.FullName.LastIndexOf( "." );
								if( index != -1 )
								{
									fileReturnName = file.FullName.Substring( 0, index );
								}
								else
								{
									fileReturnName = file.FullName;
								}
							}
							
							fileReturnName = fileReturnName.Substring(fileReturnName.IndexOf(beignString));
							fileReturnName = fileReturnName.Replace("\\", "/");
							
							nameList.Add(fileReturnName);
							
							break;
						}
					}
				}
				// is Directory
				else
				{
					ListFiles(files[i], nameList, beignString, needContainExtension ,extensions);
				}
			}
		}
	}
	
	public static void Space(int num)
	{
		for (int i = 0; i < num; i++)
		{
			EditorGUILayout.Space();
		}
	}
	
	public static void LocalToObjbect(UnityEngine.Object obj)
	{
		EditorGUIUtility.PingObject(obj);
	}
	
	public static bool ShowToggle( string str, bool flag, int stringWith, Color color)
	{
		GUI.color = color;
		EditorGUILayout.LabelField(str, GUILayout.Width(stringWith));
		bool returnFlag = EditorGUILayout.Toggle(flag, GUILayout.Width(40));
		
		GUI.color = Color.white;
		return returnFlag;
	}
	
	/// <summary>
	/// 分割线
	/// </summary>
	public static void PartitionLine()
	{
		EditorGUILayout.LabelField("========================================");
		Space(2);
	}
	
	public static void RepeatString(string str, int num , GUIStyle style = null)
	{
		for (int i = 0; i < num; i++)
		{
			if (style != null)
			{
				EditorGUILayout.LabelField(str, style);
			}
			else
			{
				EditorGUILayout.LabelField(str);
			}
		}
	}
	
	public static void LabelAndButton(string label, string button,  Action buttonAction = null, GUIStyle style = null)
	{
		EditorGUILayout.BeginHorizontal();
		{
			if (style != null)
			{
				EditorGUILayout.LabelField(label, style, GUILayout.Width(400));
			}
			else
			{
				EditorGUILayout.LabelField(label, GUILayout.Width(400));
			}
			
			if (GUILayout.Button(button, GUILayout.Width(200)))
			{
				if (buttonAction != null) buttonAction();
			}
		}
		EditorGUILayout.EndHorizontal();
	}
	
	
	/// <summary>
	/// 显示数组
	/// </summary>
	/// <param name="labelArray"></param>
	public static void LabelArray(string[] labelArray, Color bColor, Color eColor)
	{
		if ( labelArray == null ) return;
		
		GUI.color = bColor;
		foreach (string str in labelArray)
		{
			EditorGUILayout.LabelField(str, GUILayout.Width(400));
		}
		Space(1);
		GUI.color = eColor;
	}
	
	public static void LabelArray(string[] labelArray,  Color bColor, Color eColor, GUIStyle style)
	{
		if (labelArray == null) return;
		
		GUI.color = bColor;
		foreach (string str in labelArray)
		{
			EditorGUILayout.LabelField(str, style, GUILayout.Width(400));
		}
		Space(1);
		GUI.color = eColor;
	}
	
	public static void ShowLabelList(List<string> list, ref Vector2 pos, int with, int height)
	{ 
		pos = EditorGUILayout.BeginScrollView(pos, GUILayout.Width(with), GUILayout.Height(height) );
		{
			if (list != null)
			{
				foreach (string str in list)
				{
					EditorGUILayout.LabelField(str, GUILayout.Width((int)(with*0.8)));
				}
			}
		}
		EditorGUILayout.EndScrollView();
		Space(1);
	}
	
	
	
	public delegate void CallBackLabelFunc<T>( T arg);
	public static void ShowList<T>(List<T> list, CallBackLabelFunc<T> fun, ref Vector2 pos, int with, int height)
	{
		pos = EditorGUILayout.BeginScrollView(pos, GUILayout.Width(with), GUILayout.Height(height));
		{
			if (list != null)
			{
				foreach (T arg in list)
				{
					fun(arg);
				}
			}
		}
		EditorGUILayout.EndScrollView();
	}
	
	public delegate void CallBackDicLabelFunc<T1, T2>( T1 arg, T2 arg2);
	public static void ShowList<T1, T2>(Dictionary<T1, T2> dic, CallBackDicLabelFunc<T1, T2> fun, ref Vector2 pos, int with, int height)
	{
		pos = EditorGUILayout.BeginScrollView(pos, GUILayout.Width(with), GUILayout.Height(height));
		{
			if (dic != null)
			{
				foreach (KeyValuePair<T1, T2> item in dic )
				{
					fun(item.Key, item.Value);
				}
			}
		}
		EditorGUILayout.EndScrollView(); 
	}
	
	
	public static byte[] GetFileBytes( string path, bool isUncompress = true )
	{
		FileStream file;
		try
		{
			if (File.Exists(path))
			{
				file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			else
			{
				return null;
			}
		}
		catch (FileLoadException ex)
		{
			Debug.Log("#Error : Open file which path of : " + path + " : " + ex.Message);
			return null;
		}
		
		
		BinaryReader binaryReader = new BinaryReader(file);
		byte[] buff = binaryReader.ReadBytes((int)file.Length);
		if (isUncompress && buff != null)
		{
			buff = ZipLibUtils.Uncompress(buff);
		}
		
		//TextAsset textAsset = AssetDatabase.LoadMainAssetAtPath( path ) as TextAsset;
		binaryReader.Close();
		file.Close();
		
		return buff;
	}
	
	
	/// <summary>
	/// 获取配置文件
	/// </summary>
	/// <returns></returns>
	public static Dictionary<string, object > GetJsonFile( string path, bool isUncompress = true )
	{
		byte[] buff = GetFileBytes(path, isUncompress );
		
		if (buff != null)
		{
			return MiniJSON.Json.Deserialize(System.Text.UTF8Encoding.UTF8.GetString(buff)) as Dictionary<string, object>;
		}
		return null;
	}
	
	public static Dictionary<string, object > GetJsonFile( byte[] buff,  bool isUncompress = true )
	{
		if (isUncompress && buff != null)
		{
			buff = ZipLibUtils.Uncompress(buff);
		}
		
		if (buff != null)
		{
			return MiniJSON.Json.Deserialize(System.Text.UTF8Encoding.UTF8.GetString(buff)) as Dictionary<string, object>;
		}
		return null;
	}
	
	
	/// StreamReader读取文件
	/// <param name="path"></param>
	/// <returns></returns>
	public static Dictionary<string, object> GetJsonFileWithStream(string path)
	{
		FileStream file;
		try
		{
			if (File.Exists(path))
			{
				file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			else
			{
				return null;
			}
		}
		catch (FileLoadException ex)
		{
			Debug.Log("#Error : Open file which path of : " + path + " : " + ex.Message);
			return null;
		}
		
		StreamReader streamReader = new StreamReader(file, System.Text.Encoding.UTF8);
		string jsonData = streamReader.ReadToEnd();
		streamReader.Close();
		file.Close();
		
		if (jsonData != null)
		{
			return MiniJSON.Json.Deserialize(jsonData) as Dictionary<string, object>;
		}
		return null;
	}
	
	/// <summary>
	/// 配置文件
	/// </summary>
	/// <param name="data"></param>
	public static void SaveJsonFile(Dictionary<string, object> obj, string savePath, bool isCompress = true)
	{
		string jsonData = MiniJSON.Json.Serialize(obj);
		byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes(jsonData);
		if (isCompress)
		{
			buff = ZipLibUtils.Compress(buff);
		}
		
		if (jsonData.Length > 0)
		{
			SaveFileByte(buff, savePath );
		}
	}
	
	public static void SaveFileByte( byte[] buff,  string savePath )
	{
		string dir = savePath.Substring(0, savePath.LastIndexOf("/"));
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}
		
		try
		{
			FileStream fs = new FileStream(savePath, FileMode.Create);
			BinaryWriter writer = new BinaryWriter(fs, System.Text.Encoding.UTF8);
			writer.Write(buff);
			writer.Close();
			fs.Close();
		}
		catch (IOException exception)
		{
			Debug.LogException(exception);
		} 		
	}
	
	
	/// <summary>
	/// 获取
	/// </summary>
	public static byte[] GetJsonFileByte( Dictionary<string, object> obj, bool isCompress = true )
	{
		string jsonData = MiniJSON.Json.Serialize(obj);
		byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes(jsonData);
		if (isCompress)
		{
			buff = ZipLibUtils.Compress(buff);
		}
		
		return buff;
	}
}
