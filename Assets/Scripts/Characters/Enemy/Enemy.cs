using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(AIAgent))]
public partial class Enemy : MonoBehaviour, ICombatEntity, IPoolEntity {
	public EnemyProfile settings = default;

	public int HP { get; set; }
	Animator anim;
	AIAgent agent;
	Transform target;

	enum State { Idle, Chase, Attack }
	State state = State.Idle;

	public System.Action OnSpawn { get; set; }
	public System.Action OnDespawn { get; set; }

	public int instanceIndex { get; set; }

	ICombatProfile ICombatEntity.profile => settings;
	new Transform transform; // for a slight performance gain

	void Start() {
		anim = GetComponent<Animator>();
		agent = GetComponent<AIAgent>();
		transform = base.transform;

		OnDamaged += GetDamaged;
		OnDeath += () => MEC.Timing.RunCoroutine(Die());

		OnSpawn += () => SetActiveState(true);
		OnDespawn += () => SetActiveState(false);

		GetComponent<AnimationEventsListener>().OnAttack += ScanForHitsAndAttack;

		SetActiveState(true);

		void SetActiveState(bool active) {
			state = State.Chase;
			StopAllCoroutines();

			HP = settings.MaxHP;

			anim.enabled = active;


			anim.SetFloat(CharacterAnimationHelper.DirZ, active ? 1 : 0);
			anim.SetFloat(CharacterAnimationHelper.DirX, 0);
			anim.SetFloat(CharacterAnimationHelper.AttackSpeed, 1);
			anim.SetFloat(CharacterAnimationHelper.Speed, active ? settings.Speed : 0);

			if (active) { anim.Play("Idle"); }
			if (active) { target = Player.instance.transform; }
		}
	}

	void Update() {
		switch (state) {
			case State.Idle: { break; } // it doesn't really exist in this project
			case State.Chase: {
				float sqrDist2player = (target.position - transform.position).WithY(0).sqrMagnitude;
				bool shouldAttack = sqrDist2player < settings.AttackRange * settings.AttackRange;

				if (shouldAttack) { attackCommand = MEC.Timing.RunCoroutine(Attack()); }
				else { RotateTowardsNextPoint(); }
				// Movement is handled via the animator's Root Motion

				break;
			}
			case State.Attack: { break; } // the Attack() coroutine handles that
			default: { throw new UnityException("Unsupported state"); }
		}
	}

	void RotateTowardsNextPoint() {
		agent.GetPathTowards(target.position.WithY(0));
		agent.LookTowardsNextPoint();
	}

}
