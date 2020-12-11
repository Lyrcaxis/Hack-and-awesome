using System.Collections.Generic;

using UnityEngine;

public static class GraphicsHelper {
	public static IEnumerator<float> ChangeGraphicsAlpha(float targetAlpha, float totalTime, System.Action Callback = null, AnimationCurve curve = null, params UnityEngine.UI.Graphic[] graphics) {
		if (graphics.Length == 0) {
			Debug.LogWarning("Empty graphics list. Potential mistake?");
			yield break;
		}

		float t = 0;
		var startAlpha = Mathf.Abs(1 - targetAlpha);

		foreach (var txt in graphics) { txt.gameObject.SetActive(true); }

		while (t < totalTime) {
			t += Time.deltaTime;

			foreach (var txt in graphics) {
				var T = t / totalTime;

				if (curve != null) { T = curve.Evaluate(T); }

				txt.color = txt.color.WithA(Mathf.Lerp(startAlpha, targetAlpha, T));
			}

			yield return 0f;
		}

		if (targetAlpha == 0) {
			foreach (var txt in graphics) { txt.gameObject.SetActive(false); }
		}

		Callback?.Invoke();
	}
}