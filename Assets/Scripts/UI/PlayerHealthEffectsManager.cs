using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerHealthEffectsManager : MonoBehaviour {

	[Header("HP Bar settings")]
	[SerializeField] float totalDrainTime = 1f;
	[SerializeField] AnimationCurve easingCurve = default;
	[SerializeField] Image frontImg = default;

	// These values animate based on Time, always the same
	[Space, Header("Post-Processing Animation Stuff - (On Player Damaged)")]
	[SerializeField] PostProcessVolume ppVolume = default;
	[SerializeField] AnimationCurve vignetteAnimCurve = default;
	[SerializeField] AnimationCurve cAberrationAnimCurve = default;
	[SerializeField] float totalVignetteAnimTime = 1f;
	[SerializeField] float totalAberrationAnimTime = 1f;
	[SerializeField] Color vignetteColor = Color.red;

	// These values animate on HP, only once
	[Space, Header("Post-Processing Static Stuff - (Based on HP)")]
	[SerializeField] float timeToBringBackToStatic = 1f;
	[SerializeField] AnimationCurve vignetteStaticCurve = default;
	[SerializeField] AnimationCurve cAberrationStaticCurve = default;

	float currentHPPercentage = 1;
	MEC.CoroutineHandle drainHandle;
	MEC.CoroutineHandle ppAnimHandle;
	int fillPID;

	void Start() {
		var player = FindObjectOfType<Player>();
		fillPID = Shader.PropertyToID("_FillAmount");

		// Re-instantiate the material so we won't have any unwanted 'changes' shown on git
		var mat = new Material(frontImg.material);
		frontImg.material = mat;

		player.OnDamaged += (_) => {
			var prevHPPercentage = currentHPPercentage;
			currentHPPercentage = player.HP / (float) player.settings.MaxHP;

			MEC.Timing.KillCoroutines(drainHandle);
			MEC.Timing.KillCoroutines(ppAnimHandle);

			drainHandle = MEC.Timing.RunCoroutine(DrainHPBar(mat, prevHPPercentage, currentHPPercentage));
			ppAnimHandle = MEC.Timing.RunCoroutine(AnimatePostProcessing(currentHPPercentage));
		};

		player.OnDeath += () => {
			MEC.Timing.KillCoroutines(drainHandle);
			MEC.Timing.KillCoroutines(ppAnimHandle);
		};
	}

	IEnumerator<float> DrainHPBar(Material mat, float startPercentage, float endPercentage) {
		float t = 0;

		while (t < totalDrainTime) {
			t += Time.deltaTime;

			float T = t / totalDrainTime;
			mat.SetFloat(fillPID, startPercentage - (startPercentage - endPercentage) * easingCurve.Evaluate(T));

			yield return 0f;
		}
	}

	IEnumerator<float> AnimatePostProcessing(float newPercentage) {

		var targetVVal = vignetteStaticCurve.Evaluate(1 - newPercentage);
		var targetCVal = cAberrationStaticCurve.Evaluate(1 - newPercentage);


		var vAnim = ppVolume.profile.GetSetting<Vignette>();
		var cAnim = ppVolume.profile.GetSetting<ChromaticAberration>();

		float t = 0;
		float totalT = Mathf.Max(totalVignetteAnimTime, totalAberrationAnimTime);

		while (t < totalT) {
			t += Time.deltaTime;

			if (t < totalVignetteAnimTime) {
				var tarValue = vignetteAnimCurve.Evaluate(t / totalVignetteAnimTime);
				vAnim.color.value = Color.Lerp(Color.black, vignetteColor, t / totalVignetteAnimTime);
				vAnim.intensity.value = tarValue + targetVVal;
			}
			if (t < totalAberrationAnimTime) {
				var tarValue = cAberrationAnimCurve.Evaluate(t / totalAberrationAnimTime);
				cAnim.intensity.value = tarValue + targetCVal;
			}

			yield return 0f;
		}

		var originalVVal = vAnim.intensity.value;
		var originalCVal = cAnim.intensity.value;

		t = 0;
		
		while (t < timeToBringBackToStatic) {
			t += Time.deltaTime;

			float T = t / timeToBringBackToStatic;
			vAnim.color.value = Color.Lerp(vignetteColor, Color.black + Color.red * 0.01f, T);
			vAnim.intensity.value = Mathf.Lerp(originalVVal, targetVVal, T);
			cAnim.intensity.value = Mathf.Lerp(originalCVal, targetCVal, T);

			yield return 0f;
		}
	}

}
