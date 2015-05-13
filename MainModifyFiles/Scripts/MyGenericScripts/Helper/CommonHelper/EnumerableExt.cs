// --------------------------------------
//  Unity Foundation
//  EnumerableExt.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 
using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExt
{
		public static T Random<T> (this IEnumerable<T> list)
		{
				var count = list.Count ();

				if (count == 0)
						return default(T);

				return list.ElementAt (UnityEngine.Random.Range (0, count));
		}

		public static T Next<T> (this T[] list, T current)
		{
				if (list == null || list.Length == 0)
						return current;

				if (current == null)
						return list [0];

				var index = Array.IndexOf (list, current);

				index++;

				if (index >= list.Length) {
						index = 0;
				}

				return list [index];
		}

		public static T Back<T> (this T[] list, T current)
		{
				if (list == null || list.Length == 0)
						return current;

				if (current == null)
						return list [0];

				var index = Array.IndexOf (list, current);

				index--;

				if (index < 0) {
						index = list.Length - 1;
				}

				return list [index];
		}

		/// <summary>
		///   Perform the <paramref name="action" /> on each item in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static IEnumerable<T> ForEach<T> (this IEnumerable<T> collection, Action<T> action)
		{
				if (action == null) {
						throw new ArgumentNullException ("action");
				}

				foreach (var e in collection) {
						action.Invoke (e);
				}

				return collection;
		}


}