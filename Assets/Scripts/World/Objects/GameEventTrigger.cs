using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class GameEventTrigger : MonoBehaviour {

	[SerializeField] EventType type = default;
	[SerializeField] UnityEvent Event = default;
	[SerializeField] LayerMask layerMask = default;

	void Awake() {
		if (type.HasFlag(EventType.FireOnce)) {
			Event.AddListener(() => this.enabled = false);
		}
	}

	void OnTriggerEnter(Collider other) {
		if ((type & EventType.OnEnter) == 0) { return; }
		if ((layerMask.value & (1 << other.gameObject.layer)) == 0) { return; }
		Event.Invoke();
	}

	void OnTriggerExit(Collider other) {
		if ((type & EventType.OnExit) == 0) { return; }
		if ((layerMask & (1 << other.gameObject.layer)) == 0) { return; }
		Event.Invoke();
	}

	[System.Flags]
	public enum EventType {
		None = 0,
		OnEnter = 1 << 0,
		OnExit = 1 << 1,
		FireOnce = 1 << 2
	}
}
