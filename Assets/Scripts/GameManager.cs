
using UnityEngine;

public class GameManager : MonoBehaviour {
	[SerializeField] Enemy enemyPrefab = default;

	public EnemyPool enemyPool { get; private set; }

	public static GameManager instance { get; private set; }

	void Awake() {
		instance = this;

		enemyPool = new EnemyPool();
		enemyPool.Initialize(100, enemyPrefab);
	}
}
