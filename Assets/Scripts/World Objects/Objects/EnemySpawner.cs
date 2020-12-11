using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour {
	[Header("Spawn Settings")]
	[SerializeField] MinMax spawnTime = default;
	[SerializeField] List<Vector3> EnemySpawnPositions = default;
	[SerializeField] int totalSpawns = default;
	[SerializeField] EnemyProfile profile = default;

	[SerializeField] UnityEvent onAllSpawnsKilled = default;

	public bool isActive;

	float spawnT = 0;
	float nextSpawnT;
	EnemyPool pool;
	int totalSpawned;
	int spawnsKilled;

	void Start() {
		nextSpawnT = Mathf.Lerp(spawnTime.min, spawnTime.max, 0.5f);
		pool = GameManager.instance.enemyPool;
		onAllSpawnsKilled.AddListener(() => Debug.Log("ALL SPAWNS KILLED"));
	}

	void Update() {
		if (!isActive) { return; }

		spawnT += Time.deltaTime;
		if (spawnT > nextSpawnT) { SpawnNew(); }
	}


	void SpawnNew() {
		var enemy = pool.GetNew();
		if (!enemy) { return; }

		var spawnPos = transform.TransformPoint(EnemySpawnPositions.GetRandom());

		NavMesh.SamplePosition(spawnPos, out var hit, 5f, NavMesh.AllAreas);
		enemy.GetComponent<NavMeshAgent>().Warp(hit.position);
		enemy.OnDeath += SpawnKilled;
		enemy.settings = profile;

		spawnT = 0;
		nextSpawnT = spawnTime.RandomFloat;
		totalSpawned++;

		if (totalSpawned >= totalSpawns) { isActive = false; }


		void SpawnKilled() {
			spawnsKilled++;
			enemy.OnDeath -= SpawnKilled;
			if (spawnsKilled >= totalSpawns) { onAllSpawnsKilled.Invoke(); }
		}
	}




	public void Activate() => isActive = true;

	void OnDrawGizmosSelected() {
		foreach (var pos in EnemySpawnPositions) {
			Gizmos.DrawWireSphere(transform.TransformPoint(pos).WithY(0), 1.5f);
		}
	}
}
