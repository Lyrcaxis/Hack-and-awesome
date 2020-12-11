using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIAgent : MonoBehaviour {

	public NavMeshAgent agent { get; private set; }
	public float Speed => profile.Speed;
	public float RotateSpeed => profile.RotationSpeed;

	[HideInInspector] public bool CanMove = true;

	EnemyProfile profile;
	NavMeshPath path;
	Vector3[] Path = new Vector3[100];

	void Awake() {
		agent = GetComponent<NavMeshAgent>();
		profile = GetComponent<Enemy>().settings;
		path = new NavMeshPath();
	}

	public void GetPathTowards(Vector3 Destination) {
		path = new NavMeshPath();
		path.ClearCorners();
		agent.CalculatePath(GetClosestPointToNavMesh(Destination), path);

		if (path == null || path.status == NavMeshPathStatus.PathInvalid) {
			Debug.LogError("No path found.");
			return;
		}

		path.GetCornersNonAlloc(Path);
	}

	public Vector3 GetDirToNextPoint() => GetDistToNextPoint().normalized;
	public Vector3 GetDistToNextPoint() {
		var nextPoint = Vector3.MoveTowards(transform.position, Path[1], Speed * Time.deltaTime);
		var dist = nextPoint - transform.position;
		return dist;
	}

	public void LookTowardsNextPoint(float customRotateSpeed = -1) {
		Vector3 lookDir = Path[1] - transform.position;
		if (lookDir != Vector3.zero) { LooksTowardsDirection(lookDir, customRotateSpeed); }
	}

	Vector3 GetClosestPointToNavMesh(Vector3 pos) {
		NavMesh.SamplePosition(pos, out var hit, 5f, NavMesh.AllAreas);
		return hit.position;
	}

	void LooksTowardsDirection(Vector3 direction, float customRotateSpeed = -1) {
		if (customRotateSpeed < 0) { customRotateSpeed = RotateSpeed; }

		var lookQuaternion = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuaternion, customRotateSpeed * Time.deltaTime);
	}

}
