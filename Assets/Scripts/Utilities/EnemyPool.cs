using System;

using UnityEngine;

public class EnemyPool : Pool<Enemy> {

	MaterialPropertyBlock[] properties;
	Renderer[] renderers;

	static int colorPID = Shader.PropertyToID("_Color");

	public override void Initialize(int maxAmount, Enemy prefab) {
		// Shader restriction. Could work around that
		if (maxAmount > 1023) { throw new UnityException("Max supported pool amount is 1023"); }

		properties = new MaterialPropertyBlock[maxAmount];
		renderers = new Renderer[maxAmount];

		for (int i = 0; i < maxAmount; i++) { properties[i] = new MaterialPropertyBlock(); }
		foreach (var prop in properties) { prop.SetColor(colorPID, Color.white); }
		base.Initialize(maxAmount, prefab);
	}


	protected override void OnDespawn(Enemy poolObj) => base.OnDespawn(poolObj);
	protected override void OnEnqueue(Enemy poolObj, int i) {
		base.OnEnqueue(poolObj, i);
		poolObj.instanceIndex = i;
		renderers[i] = poolObj.GetComponentInChildren<Renderer>();
	}

	protected override void OnSpawn(Enemy poolObj) {
		base.OnSpawn(poolObj);
		UpdateColor(poolObj.instanceIndex, Color.white);
	}

	public void UpdateColor(int i, Color clr) {
		properties[i].SetColor(colorPID, clr);
		renderers[i].SetPropertyBlock(properties[i]);
	}
}