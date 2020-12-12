
using UnityEngine;

public partial class TutorialManager {
	[System.Serializable]
	class TutorialStep {

		public TMPro.TextMeshPro[] FadingTextList = default;

		enum TutorialStepType { EndsManually, EndsAfterTime }
		[SerializeField] TutorialStepType stepType = default;

		[SerializeField] float SecondsUntilEnd = default;

		public bool isStepDone {
			get {
				switch (stepType) {
					case TutorialStepType.EndsManually: { return isDone; }
					case TutorialStepType.EndsAfterTime: { return isDone || Time.time > StartTime + SecondsUntilEnd; }
				}

				throw new UnityException($"Unsupported step type {stepType}");
			}
		}


		float StartTime;
		bool isDone;

		public void Begin() => StartTime = Time.time;
		public void End() => isDone = true;

	}
}
