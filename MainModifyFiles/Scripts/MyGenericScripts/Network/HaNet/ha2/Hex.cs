///**
// * Hex
// * 
// * Utility class to convert Hex strings to ProtoByteArray or String types.
// * Copyright (c) 2007 Henri Torgemane
// * 
// * See LICENSE.txt for full license information.
// */
//
//
//using System.Text.RegularExpressions;
//
//public class Hex
//{
//	/**
//	 * Support straight hex, or colon-laced hex.
//	 * (that means 23:03:0e:f0, but *NOT* 23:3:e:f0)
//	 * Whitespace Charactors are ignored.
//	 */
//	public static ProtoByteArray toArray( string hex )
//	{
//		string pattern = "/\\s|:/gm";
//		Regex rgx = new Regex(pattern);
//		hex = rgx.Replace( hex, "");
//		
//		
//		//hex = hex.Replace(/\s|:/gm,'');
//		ProtoByteArray a = new ProtoByteArray();
//		if (hex.Length&1==1)
//			hex="0"+hex;
//		
//		for (uint i = 0; i < hex.Length; i += 2)
//		{
//			a[i/2] = int.Parse( hex.Substring( i, 2 ), 16 );//  hex.substr(i,2),16);
//		}
//		return a;
//	}
//	
//	public static string fromArray( ProtoByteArray array, bool colons = false)
//	{
//		string s = "";
//		for ( uint i = 0; i < array.Length; i++) {
//			s+=("0"+array[i].toString(16)).substr(-2,2)+" ";
//			if (colons) {
//				if (i<array.Length-1) s+=":";
//			}
//		}
//		return "["+array.Length.ToString()+"] "+s;
//	}
//	
//	/**
//	 * 
//	 * @param hex
//	 * @return a UTF-8 string decoded from hex
//	 * 
//	 */
//	public static string toString( string hex )
//	{
//		ProtoByteArray a = toArray( hex );
//		return a.ReadUTFBytes(a.Length);
//	}
//	
//	
//	/**
//	 * 
//	 * @param str
//	 * @return a hex string encoded from the UTF-8 string str
//	 * 
//	 */
//	public static string fromString( string str, bool colons=false) {
//		ProtoByteArray a = new ProtoByteArray();
//		a.WriteUTFBytes(str);
//		return fromArray(a, colons);
//	}
//}
