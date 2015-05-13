// --------------------------------------
//  Unity Foundation
//  StringHelper.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 字符串工具类
/// </summary>
public static class StringHelper
{
	/// <summary>
	/// writes byte[] from String
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static byte[] GetBytes (this string str)
	{
		var bytes = new byte[str.Length * sizeof(char)];
		Buffer.BlockCopy (str.ToCharArray (), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	/// <summary>
	/// Reads a string from byte[]
	/// </summary>
	/// <param name="bytes"></param>
	/// <returns></returns>
	public static string GetString (this byte[] bytes)
	{
		var chars = new char[bytes.Length / sizeof(char)];
		Buffer.BlockCopy (bytes, 0, chars, 0, bytes.Length);
		return new string (chars);
	}

	/// <summary>
	/// splits by newline
	/// </summary>
	/// <param name="s"></param>
	/// <returns></returns>
	public static string[] SplitByNewline (this string s)
	{
		return s.Split (new[] { "\r\n", "\n" }, StringSplitOptions.None);
	}

	/// <summary>
	/// Removes any newline
	/// </summary>
	/// <param name="s"></param>
	/// <returns></returns>
	public static string RemoveNewline (this string s)
	{
		return s.Replace ("\r\n", "").Replace ("\r", "").Replace ("\n", "");
	}

	/// <summary>
	/// Randoms the number.
	/// </summary>
	/// <param name="min">The min.</param>
	/// <param name="max">The max.</param>
	/// <returns></returns>
	public static int RandomNumber (int min, int max)
	{
		return UnityEngine.Random.Range (min, max);
	}

	/// <summary>
	/// Randoms the string.
	/// </summary>
	/// <param name="size">The size.</param>
	/// <returns></returns>
	public static string RandomString (int size)
	{
		StringBuilder builder = new StringBuilder ();

		char ch;

		for (int i = 0; i < size; i++) {
			ch = Convert.ToChar (Convert.ToInt32 (Math.Floor (26 * UnityEngine.Random.value + 65)));
			builder.Append (ch);
		}

		return builder.ToString ();
	}

	/// <summary>
	/// Generates an id.
	/// </summary>
	/// <returns></returns>
	public static string GenerateId (int size)
	{
		string g = Guid.NewGuid ().ToString ().Replace ("-", "");

		return g.Substring (0, size);
	}

	#region Color Text
	/// <summary>
	/// Creates the color symbol text.
	/// </summary>
	public static string WrapColor (this string txt, string colorSymbol)
	{
		return string.Format ("[{0}]{1}[-]", colorSymbol, txt);
	}

	public static string WrapColorWithLog (this string txt, string color ="orange"){
		return string.Format("<color={0}>{1}</color>",color,txt);
	}

	public static string WrapColor (this string txt, Color color)
	{
		return WrapColor (txt, ColorExt.ColorToHex (color));
	}

	public static string WrapColor (this int txt, Color color)
	{
		return WrapColor (txt.ToString(), ColorExt.ColorToHex (color));
	}
	#endregion

	#region Symbol Text 
	/// <summary>
	/// <para>Wraps the symbol.</para>
	/// <para>[b] 加粗</para>
	/// <para>[i] 斜体</para>
	/// <para>[u] 下划线</para>
	/// <para>[s] 删除线</para>
	/// </summary>
	public static string WrapSymbol (this string txt, string symbol)
	{
		return string.Format ("[{0}]{1}[/{0}]", symbol, txt);
	}
	#endregion

	#region URL Text
	public static string WrapURL (this string txt, string link)
	{
		return string.Format ("[url={0}]{1}[/url]", link, txt);
	}
	#endregion

	#region 字符串匹配
	public static bool IsEmail (this string email)
	{
		const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
			+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
			+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
		
		var regex = new Regex (pattern, RegexOptions.IgnoreCase);
		
		return regex.IsMatch (email);
	}
	
	public static bool ValidateAccount (string s)
	{
		string pattern = @"^[a-zA-Z0-9]+$";
		return Regex.IsMatch (s, pattern);
	}
	#endregion
}
