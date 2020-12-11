using UnityEngine;

[System.Serializable]
public struct MinMax {
	public float min;
	public float max;

	public MinMax(float min, float max) {
		this.min = min;
		this.max = max;
	}

	public int RandomInt => Random.Range((int) min, (int) max);
	public float RandomFloat => Random.Range(min, max);

	public float Lerp(float t) => Mathf.Lerp(min, max, t);
	public float InverseLerp(float value) => Mathf.InverseLerp(min, max, value);
}
