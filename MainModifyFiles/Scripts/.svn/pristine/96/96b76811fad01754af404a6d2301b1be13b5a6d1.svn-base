// --------------------------------------
//  Unity Foundation
//  ColorExt.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 
using UnityEngine;

/// <summary>
/// Static helper for Colors
/// </summary>
public static class ColorExt
{
	/// <summary>
	/// returns the same color with a new alpha value
	/// </summary>
	/// <param name="c"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static Color SetAlpha (this Color c, float a)
	{
		return new Color (c.r, c.g, c.b, a);
	}

	/// <summary>
	/// Color To String
	/// </summary>
	/// <param name="c"></param>
	/// <returns></returns>
	public static string Serialize (this Color c)
	{
		return string.Format ("{0},{1},{2},{3}", c.r, c.g, c.b, c.a);
	}

	/// <summary>
	/// String to Color
	/// </summary>
	/// <param name="color"></param>
	/// <returns></returns>
	public static Color Deserialize (string color)
	{
		var s = color.Split (',');

		return new Color (float.Parse (s [0]), float.Parse (s [1]), float.Parse (s [2]), float.Parse (s [3]));
	}

	// Note that Color32 and Color implicitly convert to each other. You may pass a Color object to this method without first casting it.
	public static string ColorToHex (Color32 color)
	{
		string hex = color.r.ToString ("X2") + color.g.ToString ("X2") + color.b.ToString ("X2");
		return hex;
	}

	public static Color HexToColor (string hex)
	{
		byte r = byte.Parse (hex.Substring (0, 2), global::System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse (hex.Substring (2, 2), global::System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse (hex.Substring (4, 2), global::System.Globalization.NumberStyles.HexNumber);
		return new Color32 (r, g, b, 255);
	}
}