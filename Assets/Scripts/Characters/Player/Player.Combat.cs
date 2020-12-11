using System.Collections.Generic;

using UnityEngine;

public partial class Player {
	public int HP { get; set; }

	public static Player instance { get; private set; }
	public System.Action OnDeath { get; set; }
	public System.Action<int> OnDamaged { get; set; }
	ICombatProfile ICombatEntity.profile => settings;

	int currentCombo = -1;
	float timeSincePrevAttack;
	int enemyLayer;

	HashSet<Collider> enemiesHit = new HashSet<Collider>();
	Collider[] results = new Collider[100];
	MEC.CoroutineHandle attackCommand = default;

	void GetDamaged(int dmg) {
		HP -= dmg;

		if (HP > 0) { }
		else {
			HP = 0;
			anim.CrossFadeInFixedTime(CharacterAnimationHelper.Die, 0.25f, 0, 0.2f);
			OnDeath?.Invoke();
		}
	}

	void HandleAttack() {
		if ((state == State.FreeToDoActions || state == State.Attacking) && currentCombo < 2 && Input.GetMouseButtonDown(0)) {
			var percentage = timeSincePrevAttack / (float) anim.GetCurrentAnimatorStateInfo(0).length;
			bool canAttack = percentage >= settings.minComboSkipTime;
			if (canAttack) { PlayNextCombo(); }
		}

		timeSincePrevAttack += Time.deltaTime;
		if (timeSincePrevAttack >= settings.maxSecondsToKeepCombo && currentCombo != -1) { ResetCombo(); }



		void PlayNextCombo() {
			currentCombo++;
			timeSincePrevAttack = 0;

			state = State.Attacking;

			MEC.Timing.KillCoroutines(attackCommand);
			attackCommand = MEC.Timing.RunCoroutine(DoAttackStuff());

			var t = currentCombo == -1 ? 0 : settings.comboSkipTimeCurve.Evaluate(timeSincePrevAttack / settings.maxSecondsToKeepCombo);
			anim.CrossFadeInFixedTime(CharacterAnimationHelper.GetHashForCombo(currentCombo), 0.2f, 0, t);
		}
		void ResetCombo() {
			currentCombo = -1;
			if (state == State.Attacking) { state = State.FreeToDoActions; }
		}
		IEnumerator<float> DoAttackStuff() {
			const float totalTimeToRotate = 0.25f;
			const float totalTimeToAnimMove = 0.25f;
			float t = 0;

			var extents = hitCollider.bounds.extents;
			enemiesHit.Clear();

			while (t < totalTimeToRotate) {
				var targetRotY = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetLookRot.eulerAngles.y, settings.AttackRotationSpeed * Time.deltaTime);
				transform.eulerAngles = transform.eulerAngles.WithY(targetRotY);

				t += Time.deltaTime;
				yield return 0f;
			}

			var curve = CharacterAnimationHelper.GetSettingForCombo(currentCombo, settings);
			t = 0;

			while (t < totalTimeToAnimMove) {
				t += Time.deltaTime;
				cc.Move(curve.Evaluate(t / totalTimeToAnimMove) * transform.forward * settings.moveForcePerAttack * Time.deltaTime);

				ScanForHitsAndAttack();

				yield return 0f;
			}
		}

		void ScanForHitsAndAttack() {
			int hitAmount = Physics.OverlapBoxNonAlloc(hitCollider.bounds.center, hitCollider.bounds.extents, results, Quaternion.identity, LayerHelper.enemyLayer, QueryTriggerInteraction.Collide);

			for (int i = 0; i < hitAmount; i++) {
				var col = results[i];
				if (!col || enemiesHit.Contains(col)) { return; }
				var enemy = col.GetComponent<Enemy>();
				enemy.OnDamaged?.Invoke(settings.Damage);
				enemiesHit.Add(col);
			}
		}
	}

}
