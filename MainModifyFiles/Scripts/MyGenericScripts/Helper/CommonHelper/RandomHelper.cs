// --------------------------------------
//  Unity Foundation
//  RandomHelper.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 
using UnityEngine;


public static class RandomHelper
{
		/// <summary>
		/// return Random.Range(0f, 1f);
		/// </summary>
		/// <returns></returns>
		public static float Range01 ()
		{
				return Random.Range (0f, 1f);
		}

		/// <summary>
		/// Converted to 3d space
		/// 
		/// var p1 = Random.insideUnitCircle * radius;
		/// var p2 = new Vector3(p1.x, 0, p1.y);
		/// </summary>
		/// <param name="radius"></param>
		/// <returns>new Vector3(p1.x, 0, p1.y</returns>
		public static Vector3 Inside2DCircle (float radius)
		{
				//get random vector 2
				var p1 = Random.insideUnitCircle * radius;

				//convert v2 to v3
				var p2 = new Vector3 (p1.x, 0, p1.y);

				return p2;
		}
    
}
