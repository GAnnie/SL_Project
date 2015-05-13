// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  BaiduEncoding.cs
// Author   : willson
// Created  : 2014/11/5 
// Porpuse  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// \u50FA\u49AD 转换成 "中国"
using UnityEngine;

public class BaiduEncoding
{ 
	/// \u50FA\u49AD 转换成 "中国"
	public static string NormalU2C(string input)
	{
		string str = "";
		char[] chArray = input.ToCharArray();
		Encoding bigEndianUnicode = Encoding.BigEndianUnicode;
		for (int i = 0; i < chArray.Length; i++)
		{
			char ch = chArray[i];
			if (ch.Equals('\\'))
			{
				i++;
				i++;
				char[] chArray2 = new char[4];
				int index = 0;
				index = 0;
				while ((index < 4) && (i < chArray.Length))
				{
					chArray2[index] = chArray[i];
					index++;
					i++;
				}
				if (index == 4)
				{
					try
					{
						str = str + UnicodeCode2Str(chArray2);
					}
					catch (Exception)
					{
						str = str + @"\u";
						for (int j = 0; j < index; j++)
						{
							str = str + chArray2[j];
						}
					}
					i--;
				}
				else
				{
					str = str + @"\u";
					for (int k = 0; k < index; k++)
					{
						str = str + chArray2[k];
					}
				}
			}
			else
			{
				str = str + ch.ToString();
			}
		}
		return str;
	}
	/// UnicodeCode字节 转换成 "中国"
	public static string UnicodeCode2Str(char[] u4)
	{
		if (u4.Length < 4)
		{
			throw new Exception("It's not a unicode code array");
		}
		string str = "0123456789ABCDEF";
		char ch = char.ToUpper(u4[0]);
		char ch2 = char.ToUpper(u4[1]);
		char ch3 = char.ToUpper(u4[2]);
		char ch4 = char.ToUpper(u4[3]);
		int index = str.IndexOf(ch);
		int num2 = str.IndexOf(ch2);
		int num3 = str.IndexOf(ch3);
		int num4 = str.IndexOf(ch4);
		if (((index == -1) || (num2 == -1)) || ((num3 == -1) || (num4 == -1)))
		{
			throw new Exception("It's not a unicode code array");
		}
		byte num5 = (byte)(((index * 0x10) + num2) & 0xff);
		byte num6 = (byte)(((num3 * 0x10) + num4) & 0xff);
		byte[] bytes = new byte[] { num5, num6 };
		//Debug.Log(Encoding.BigEndianUnicode.GetString(bytes));
		return Encoding.BigEndianUnicode.GetString(bytes);
		//return System.Text.UTF8Encoding.UTF8.GetString(bytes);
	}
}