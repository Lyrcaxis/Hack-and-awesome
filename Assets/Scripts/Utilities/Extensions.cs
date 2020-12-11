using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
	#region Vector2 Extensions
	public static Vector2 WithX(this Vector2 vec, float newValue) => new Vector2(newValue, vec.y);
	public static Vector2 WithY(this Vector2 vec, float newValue) => new Vector2(vec.x, newValue);
	#endregion

	#region Vector3 Extensions
	public static Vector3 WithX(this Vector3 vec, float newValue) => new Vector3(newValue, vec.y, vec.z);
	public static Vector3 WithY(this Vector3 vec, float newValue) => new Vector3(vec.x, newValue, vec.z);
	public static Vector3 WithZ(this Vector3 vec, float newValue) => new Vector3(vec.x, vec.y, newValue);
	#endregion

	#region Color Extensions
	public static Color WithR(this Color clr, float newValue) => new Color(newValue, clr.g, clr.b, clr.a);
	public static Color WithG(this Color clr, float newValue) => new Color(clr.r, newValue, clr.b, clr.a);
	public static Color WithB(this Color clr, float newValue) => new Color(clr.r, clr.g, newValue, clr.a);
	public static Color WithA(this Color clr, float newValue) => new Color(clr.r, clr.g, clr.b, newValue);
	#endregion

	#region List Extensions
	public static T GetRandom<T>(this IList<T> list) => list[UnityEngine.Random.Range(0, list.Count)];
	#endregion
}
