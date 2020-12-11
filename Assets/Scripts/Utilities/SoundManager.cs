using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class SoundManager : ScriptableObject {

	[Header("General Settings")]
	public float gameVolume = 1;
	[SerializeField] AudioSource sourcePrefab = default;

	static GameObject root;
	static List<AudioSource> pool;

	const int audioPoolAmount = 100;

	public static SoundManager instance { get; private set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void Initialize() {
		instance = Resources.Load<SoundManager>("Sound Manager");

		root = new GameObject("Sound Manager");
		pool = new List<AudioSource>(audioPoolAmount);

		DontDestroyOnLoad(root);

		var rootTR = root.transform;
		for (int i = 0; i < audioPoolAmount; i++) {
			var newObj = Instantiate(instance.sourcePrefab);
			newObj.transform.SetParent(rootTR);
			pool.Add(newObj);
		}
	}

	static AudioSource GetNext() {
		var src = pool.FirstOrDefault(x => !x.isPlaying);

		if (!src) {
			src = Instantiate(instance.sourcePrefab);
			pool.Add(src);
		}

		return src;
	}

	public static void Play(AudioClip clip, float volume = 1, float pitch = 1) {
		var src = GetNext();
		src.clip = clip;
		src.volume = instance.gameVolume * volume;
		src.pitch = pitch;
		src.Play();
	}

}
