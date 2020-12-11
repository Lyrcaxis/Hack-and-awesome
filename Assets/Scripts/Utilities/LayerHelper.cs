
using UnityEngine;

public static class LayerHelper {
	public static int playerLayer = LayerMask.GetMask("Player");
	public static int enemyLayer = LayerMask.GetMask("Enemies");
}