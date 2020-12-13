using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class AnimationEventsListener : MonoBehaviour {

	public bool active;

	[Header("Sounds")]
	[SerializeField] AudioTrack footWalk = default;
	[SerializeField] AudioTrack roll = default;
	[Space]
	[SerializeField] AudioTrack attack1 = default;
	[SerializeField] AudioTrack attack2 = default;
	[SerializeField] AudioTrack attack3 = default;

	public System.Action OnAttack;

	public void FootR() {
		if (!active) { return; }
		var pitch = Random.Range(0.6f, 0.9f); // magic numbers
		SoundManager.Play(footWalk.clip, footWalk.volume, pitch);
	}
	public void FootL() {
		if (!active) { return; }
		var pitch = Random.Range(0.6f, 1.3f); // more magic numbers
		SoundManager.Play(footWalk.clip, footWalk.volume, pitch);
	}

	public void Roll() {
		if (!active) { return; }
		var pitch = 1.5f; // man do I love magic numbers
		SoundManager.Play(roll.clip, roll.volume, pitch);
	}

	public void Attack(int currentCombo) {
		var audioTrack = GetTrackForCombo();
		SoundManager.Play(audioTrack.clip, audioTrack.volume);

		OnAttack?.Invoke();

		AudioTrack GetTrackForCombo() {
			switch (currentCombo) {
				case 0: { return attack1; }
				case 1: { return attack2; }
				case 2: { return attack3; }
			}
			throw new UnityException("Unsupported combo number");
		}
	}
}
