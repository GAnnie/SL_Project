// --------------------------------------
//  Unity Foundation
//  PlayerPrefsExt.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 
using UnityEngine;


/// <summary>
/// Extension methods for PlayerPrefs
/// </summary>
public static class PlayerPrefsExt
{
	/// <summary>
	/// Returns the PlayerPref serialized as a bool.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static bool GetBool (string key)
	{
		return PlayerPrefs.GetInt (key, 0) == 1;
	}

	/// <summary> 
	/// Returns the PlayerPref serialized as a bool.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static bool GetBool (string key, bool defaultValue)
	{
		return PlayerPrefs.GetInt (key, defaultValue ? 1 : 0) == 1;
	}

	/// <summary>
	/// Sets the PlayerPref with a boolean value.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	public static void SetBool (string key, bool value)
	{
		PlayerPrefs.SetInt (key, value ? 1 : 0);
	}

	#region Operate PlayerPrefs with PlayerID
	public static void SetLocalString (string name, string value)
	{
		name = PlayerModel.Instance.GetPlayerId () + "_" + name;
		PlayerPrefs.SetString (name, value);
	}
		
	public static string GetLocalString (string name)
	{
		name = PlayerModel.Instance.GetPlayerId () + "_" + name;
		return PlayerPrefs.GetString (name);
	}
		
	public static bool HasLocalKey (string key)
	{
		key = PlayerModel.Instance.GetPlayerId () + "_" + key;
		return PlayerPrefs.HasKey (key);
	}
		
	public static void SetLocalInt (string name, int value)
	{
		name = PlayerModel.Instance.GetPlayerId () + "_" + name;
		PlayerPrefs.SetInt (name, value);
	}
		
	public static int GetLocalInt (string name, int defaultValue)
	{
		name = PlayerModel.Instance.GetPlayerId () + "_" + name;
		int contentId = PlayerPrefs.GetInt (name, defaultValue);
		return contentId;
	}
		
	public static void DeleteLocalKey (string name)
	{
		name = PlayerModel.Instance.GetPlayerId () + "_" + name;
		PlayerPrefs.DeleteKey (name);
	}
	#endregion
}