using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T> where T : MonoBehaviour, IPoolEntity  {
	protected Queue<T> pool = new Queue<T>();
	protected Transform poolParent;
	
	public virtual void Initialize(int maxAmount, T prefab) {
		poolParent = new GameObject($"{typeof(T).Name} pool").transform;

		for (int i = 0; i < maxAmount; i++) {
			var newObj = UnityEngine.Object.Instantiate(prefab, poolParent);

			newObj.OnSpawn += () => OnSpawn(newObj);
			newObj.OnDespawn += () => OnDespawn(newObj);

			OnEnqueue(newObj, i);
			pool.Enqueue(newObj);
		}
	}

	public virtual T GetNew() {
		if (pool.Count == 0) { return null; }
		var obj = pool.Dequeue();
		obj.OnSpawn?.Invoke();
		return obj;
	}

	protected virtual void OnEnqueue(T poolObj, int i) => poolObj.gameObject.SetActive(false);
	protected virtual void OnSpawn(T poolObj) => poolObj.gameObject.SetActive(true);
	protected virtual void OnDespawn(T poolObj) {
		poolObj.gameObject.SetActive(false);
		pool.Enqueue(poolObj);
	}
}
