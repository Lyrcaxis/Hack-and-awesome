using System.Collections.Generic;

using UnityEngine;

public partial class Enemy {

	public System.Action<int> OnDamaged { get; set; }
	public System.Action OnDeath { get; set; }

	MEC.CoroutineHandle attackCommand;

	IEnumerator<float> Attack() {
		var attackRangeSqrd = settings.AttackRange * settings.AttackRange;
		float t = 0;

		state = State.Attack;
		anim.SetFloat(CharacterAnimationHelper.DirZ, 0);
		anim.SetFloat(CharacterAnimationHelper.Speed, 0);

		// Make the enemy rotate towards the target while waiting for SecondsBeforeAttack
		while (t < settings.SecondsBeforeAttack) {
			t += Time.deltaTime;

			var dist = (target.position - transform.position).WithY(0);

			var targetLookRot = Quaternion.LookRotation(dist, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetLookRot, t / settings.SecondsBeforeAttack);
			
			var sqrMagn = dist.sqrMagnitude;

			if (sqrMagn > attackRangeSqrd) {
				yield return MEC.Timing.WaitForSeconds(0.15f);
				Reset();
				yield break;
			}
			yield return 0f;
		}

		// Attack using one of the three attack moves
		var attackHash = CharacterAnimationHelper.GetHashForCombo(Random.Range(0, 3));
		anim.CrossFadeInFixedTime(attackHash, 0.1f);

		// Hit scanning will be registered via animation events

		t = 0;
		while (t < settings.SecondsAfterAttack) {
			t += Time.deltaTime;
			yield return 0f;
		}

		Reset();

		void Reset() {
			state = State.Chase;
			anim.SetFloat(CharacterAnimationHelper.DirZ, 1);
			anim.SetFloat(CharacterAnimationHelper.Speed, 1);
		}
	}

	static Collider[] hitResults = new Collider[1];

	void ScanForHitsAndAttack() {
		var pos = transform.position + transform.forward * settings.AttackRange/ 2f;
		var halfExtents = new Vector3(0.4f, 0.25f, settings.AttackRange / 2f);
		var hits = Physics.OverlapBoxNonAlloc(pos, halfExtents, hitResults, Quaternion.identity, LayerHelper.playerLayer, QueryTriggerInteraction.Collide);

		bool didHitPlayer = hits == 1;
		if (didHitPlayer) { hitResults[0].GetComponent<Player>().OnDamaged?.Invoke(settings.Damage); }
	}


	void GetDamaged(int damage) {
		if (HP == 0) { return; }
		HP -= damage;

		MEC.Timing.KillCoroutines(attackCommand);

		state = State.Chase;

		if (HP > 0) { MEC.Timing.RunCoroutine(FlashRed()); }
		else { OnDeath?.Invoke(); }


		IEnumerator<float> FlashRed() {
			const float totalFlashT = 0.5f;
			float t = 0;

			while (t < totalFlashT) {
				t += Time.deltaTime;

				float T = (1 + Mathf.Sin(25 * t)) / 2;

				var clr = Color.Lerp(Color.white, Color.red, T);
				GameManager.instance.enemyPool.UpdateColor(instanceIndex, clr);

				yield return 0f;
			}

			GameManager.instance.enemyPool.UpdateColor(instanceIndex, Color.white);
		}
	}

	IEnumerator<float> Die() {
		state = State.Idle;
		HP = 0;

		anim.CrossFadeInFixedTime(CharacterAnimationHelper.Die, 0.3f, 0, 0.6f);
		yield return MEC.Timing.WaitForSeconds(settings.AfterDeathTime);

		OnDespawn?.Invoke();
		yield return 0f;
	}

}
