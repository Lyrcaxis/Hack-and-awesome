using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public partial class TutorialManager : MonoBehaviour {
	[SerializeField] List<TutorialStep> tutorialSteps = default;
		
	[SerializeField] float initialWaitT = default;
	[SerializeField] float textFadeInT = default;
	[SerializeField] float textFadeOutT = default;
	[Space]
	[SerializeField] UnityEvent OnTutorialComplete = default;


	void Start() => MEC.Timing.RunCoroutine(BeginTutorial());

	IEnumerator<float> BeginTutorial() {

		yield return MEC.Timing.WaitForSeconds(initialWaitT);

		foreach (var step in tutorialSteps) {
			step.Begin();
			bool canContinue = false;

			MEC.Timing.RunCoroutine(GraphicsHelper.ChangeGraphicsAlpha(1, textFadeInT, () => canContinue = true, null, step.FadingTextList));
			while (!step.isStepDone) { yield return 0f; }

			canContinue = false;

			MEC.Timing.RunCoroutine(GraphicsHelper.ChangeGraphicsAlpha(0, textFadeOutT, () => canContinue = true, null, step.FadingTextList));
			while (!canContinue) { yield return 0f; }
		}

		OnTutorialComplete.Invoke();
	}

	// To be called from UnityEvents
	public void EndStep(int i) => tutorialSteps[i].End();
}
