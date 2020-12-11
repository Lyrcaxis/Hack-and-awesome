using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[DefaultExecutionOrder(-100)]
public partial class Player : MonoBehaviour, ICombatEntity {

	public PlayerProfile settings = default;
	[SerializeField] BoxCollider hitCollider = default;

	Camera cam = default;
	CharacterController cc;
	Animator anim = default;

	Quaternion targetLookRot;
	Vector3 currentVel;
	Vector3 moveVector;

	enum State { FreeToDoActions, Attacking, Rolling }
	State state = State.FreeToDoActions;

	void Awake() {
		cc = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		cam = FindObjectOfType<GameCamera>().GetComponent<Camera>();

		anim.SetFloat(CharacterAnimationHelper.AttackSpeed, 1); // locked atm
		instance = this;
		HP = settings.MaxHP;

		OnDamaged += GetDamaged;
		OnDeath += () => {
			OnDamaged = null;
			this.enabled = false;
		};
	}

	void Update() {
		HandleMovement();
		HandleRotation();
		HandleAttack();
		HandleActions();
		HandleAnimations();
	}

	void HandleMovement() {
		var targetVel = new Vector3();
		targetVel += cam.transform.right.WithY(0).normalized * Input.GetAxis("Horizontal");
		targetVel += cam.transform.forward.WithY(0).normalized * Input.GetAxis("Vertical");

		currentVel = Vector3.MoveTowards(currentVel, targetVel.normalized * settings.Speed, settings.Acceleration * Time.deltaTime);
		moveVector = currentVel.normalized;
	}

	void HandleRotation() {
		var mousePos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
		var screenMid = new Vector3(Screen.width / 2, 0, Screen.height / 2);

		var MousePosNMZ = mousePos - screenMid;
		targetLookRot = Quaternion.LookRotation(MousePosNMZ, Vector3.up) * Quaternion.LookRotation(cam.transform.forward.WithY(0));

		if (state == State.FreeToDoActions) {
			var sqrMag = currentVel.sqrMagnitude;
			if (sqrMag != 0) {
				var targetRotY = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetLookRot.eulerAngles.y, sqrMag * settings.RotationSpeed * Time.deltaTime);
				transform.eulerAngles = transform.eulerAngles.WithY(targetRotY);
			}
		}
	}

	void HandleAnimations() {
		anim.SetFloat(CharacterAnimationHelper.DirZ, Vector3.Dot(transform.forward, moveVector));
		anim.SetFloat(CharacterAnimationHelper.DirX, Vector3.Dot(transform.right, moveVector));
		anim.SetFloat(CharacterAnimationHelper.Speed, currentVel.magnitude * Mathf.Max(anim.GetFloat(CharacterAnimationHelper.DirZ), 0.75f));
	}

}
