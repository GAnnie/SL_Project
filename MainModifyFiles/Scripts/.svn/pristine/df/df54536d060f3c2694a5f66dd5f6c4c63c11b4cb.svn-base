using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using com.nucleus.commons.data;
using LITJson;

public static class DataHelper
{

	#region Json Data Control
	public static byte[] GetFileBytes (string path)
	{
		try {
			FileStream fs = new FileStream (path, FileMode.Open);
			BinaryReader reader = new BinaryReader (fs);
			byte[] bytes = reader.ReadBytes ((int)fs.Length);
			
			reader.Close ();
			fs.Close ();
			
			return bytes;
		} catch (System.Exception e) {
			GameDebuger.Log (string.Format ("Utility < {0} >", e.Message));
			return null;
		} 		
	}
	
	public static Dictionary<string, object> GetJsonFile (string path, string extension = "txt", bool isUncompress = true)
	{
		return GetJsonFile(GetFileBytes(path), isUncompress);

//		TextAsset textAsset = ResourceLoader.Load (path, extension) as TextAsset;
//		if (textAsset != null) {
//			if (isUncompress) {
//				byte[] buff = ZipLibUtils.Uncompress (textAsset.bytes);
//				return MiniJSON.Json.Deserialize (System.Text.UTF8Encoding.UTF8.GetString (buff)) as Dictionary<string, object>;
//			} else {
//				return MiniJSON.Json.Deserialize (textAsset.text) as Dictionary<string, object>;
//			}
//		}
//		return null;
	}
	
	public static Dictionary<string, object> GetJsonFile (byte[] bytes, bool isUncompress = true)
	{
		if (bytes != null)
		{
			if (isUncompress) {
				byte[] buff = ZipLibUtils.Uncompress (bytes);
				return MiniJSON.Json.Deserialize (System.Text.UTF8Encoding.UTF8.GetString (buff)) as Dictionary<string, object>;
			} else {
				return MiniJSON.Json.Deserialize (System.Text.UTF8Encoding.UTF8.GetString (bytes)) as Dictionary<string, object>;
			} 
		}
		else
		{
			return null;
		}
	}

	public static byte[] GetJsonData (Dictionary<string, object> obj, bool isCompress = true)
	{
		string jsonData = MiniJSON.Json.Serialize (obj);
		byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes (jsonData);
		if (isCompress) {
			buff = ZipLibUtils.Compress (buff);
		}
		
		return buff;
	}
	
	public static void SaveJsonFile (Dictionary<string, object> obj, string savePath, bool isCompress = true)
	{
		string jsonData = MiniJSON.Json.Serialize (obj);
		byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes (jsonData);
		if (isCompress) {
			buff = ZipLibUtils.Compress (buff);
		}
		
		if (jsonData.Length > 0) {
			SaveFile (savePath, buff);
		}
	}

	public static void SaveJsonFile (Object obj, string savePath, bool isCompress = true)
	{
		string json = JsonMapper.ToJson (obj);
		byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes (json);
		if (isCompress) {
			buff = ZipLibUtils.Compress (buff);
		}
		
		if (json.Length > 0) {
			SaveFile (savePath, buff);
		}
	}

	public static T GetJsonFile<T> (string path, string extension = "txt", bool isUncompress = true)
	{
		TextAsset textAsset = ResourceLoader.Load (path, extension) as TextAsset;
		if (textAsset != null) {
			string json = null;
			if (isUncompress) {
				byte[] buff = ZipLibUtils.Uncompress (textAsset.bytes);
				json = System.Text.UTF8Encoding.UTF8.GetString (buff);
			} else {
				json = textAsset.text;
			}
			return JsonMapper.ToObject<T>(json);
		}
		return default(T);
	}

	public static void SaveFile (string savePath, byte[] buff)
	{
		string dir = savePath.Substring (0, savePath.LastIndexOf ("/"));
		if (!Directory.Exists (dir)) {
			Directory.CreateDirectory (dir);
		}
		
		try {
			FileStream fs = new FileStream (savePath, FileMode.Create);
			BinaryWriter writer = new BinaryWriter (fs, System.Text.Encoding.UTF8);
			writer.Write (buff);
			writer.Close ();
			fs.Close ();
		} catch (IOException exception) {
			Debug.LogException (exception);
		}		
	}
	#endregion

	#region 获取配置格式的信息
	public static string AnalyConfigString (string configStr, int level)
	{
		if (string.IsNullOrEmpty (configStr)) {
			return string.Empty;
		} else {
			Regex regex = new Regex ("\\[[^\\[^\\]]*\\]");
			
			MatchCollection mc = regex.Matches (configStr);
			
			if (mc.Count > 0) {
				Dictionary< string , string > strDict = new Dictionary<string, string> ();
				
				foreach (Match match in mc) {
					string matchStr = match.ToString ();
					
					
					if (!strDict.ContainsKey (matchStr)) {
						if (matchStr.Length > 3) {
							string str = Regex.Replace (matchStr, @"\s", ""); 
							str = str.Substring (1, str.Length - 2);
							str = str.Replace ("lv", level.ToString ());
							
							string[] split1 = str.Split ('+');
							string[] split2 = split1 [1].Split ('*');
							
							float number = float.Parse (split2 [0]) * float.Parse (split2 [1]);
							number += float.Parse (split1 [0]);
							
							strDict.Add (matchStr, number.ToString ());
						}
					}
				}
				
				foreach (KeyValuePair< string, string > item in strDict) {
					configStr = configStr.Replace (item.Key, item.Value);
				}
				
				
			}
			
			return configStr;
		}
	}
	#endregion

	#region 获取静态数据配置
	public static int GetStaticConfigValue (int key, int defaultValue = 0)
	{
		StaticConfig data = DataCache.getDtoByCls<StaticConfig> (key);
		return (data != null && !string.IsNullOrEmpty(data.value)) ? int.Parse (data.value) : defaultValue;
	}
	
	public static float GetStaticConfigValuef (int key, float defaultValue = 0.0f)
	{
		StaticConfig data = DataCache.getDtoByCls<StaticConfig> (key);
		return (data != null) ? float.Parse (data.value) : defaultValue;
	}
	#endregion

	/// <summary>
	/// 将Unix时间戳转换为DateTime类型时间
	/// </summary>
	/// <param name="d">double 型数字</param>
	/// <returns>DateTime</returns>
	public static System.DateTime ConvertIntDateTime(double milliseconds)
	{
		System.DateTime time = System.DateTime.MinValue;
		System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
		time = startTime.AddMilliseconds(milliseconds);
		return time;
	}
}
