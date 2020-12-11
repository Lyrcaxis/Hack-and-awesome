
using UnityEngine;

public partial class HealthBar {
	public struct AnimationHelper {
		public float startPercentage;
		public float endPercentage;

		float startTime;

		public AnimationHelper(float t1, float t2) {
			startPercentage = t1;
			endPercentage = t2;

			startTime = Time.time;
		}

		public float GetCurrentPercentage(AnimationCurve curve) {
			float T = Time.time - startTime;
			return startPercentage - (startPercentage - endPercentage) * curve.Evaluate(T);
		}
	}
}
