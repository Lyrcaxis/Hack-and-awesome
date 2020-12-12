using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	[SerializeField] float closedYPos = default;
	[SerializeField] float openYPos = default;
	[SerializeField] float speed = default;

	bool isOpen;
	public void Open() => isOpen = true;
	public void Close() => isOpen = false;

	void Update() {
		var pos = transform.position;
		var tarPos = pos.WithY(isOpen ? openYPos : closedYPos);
		transform.position = Vector3.MoveTowards(pos, tarPos, speed * Time.deltaTime);
	}

}
