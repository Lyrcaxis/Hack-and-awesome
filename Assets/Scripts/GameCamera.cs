using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour {
	[SerializeField] Transform target = default;
	[SerializeField] float upDistance = default;
	[SerializeField] Vector3 angle = default;

	private void LateUpdate() {
		transform.eulerAngles = angle;
		transform.position = target.position - transform.forward * upDistance;
	}

}
