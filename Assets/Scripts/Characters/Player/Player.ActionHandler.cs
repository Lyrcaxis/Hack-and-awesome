using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public partial class Player {

	void HandleActions() {
		HandleRoll();
		// <insert any additional misc actions here>
	}

	void HandleRoll() {
		const float rollWaitT = 0.5f;

		bool shouldRoll = state != State.Rolling && Input.GetMouseButtonDown(1);
		if (shouldRoll) { MEC.Timing.RunCoroutine(StartRolling()); }

		IEnumerator<float> StartRolling() {
			state = State.Rolling;

			anim.CrossFadeInFixedTime(CharacterAnimationHelper.Roll, 0.2f, 0, 0f);
			yield return MEC.Timing.WaitForSeconds(rollWaitT);

			if (state == State.Rolling) { state = State.FreeToDoActions; }
		}
	}
}
