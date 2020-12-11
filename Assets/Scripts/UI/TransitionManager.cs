using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour {
	[SerializeField] Image transitionPanel = default;
	[SerializeField] float fadeInT = default;
	[SerializeField] float fadeOutT = default;
	[SerializeField] Color startColor = default;

	[Space, Header("Death Panel Settings")]
	[SerializeField] GameObject deathPanel = default;
	[SerializeField] float deathPanelShowT = default;
	[SerializeField] float deathPanelHideT = default;
	[SerializeField] AnimationCurve deathPanelShowCurve = default;
	[SerializeField] AnimationCurve deathPanelHideCurve = default;

	void Awake() {
		transitionPanel.gameObject.SetActive(true);
		transitionPanel.color = startColor;

		Time.timeScale = 1;
		FadeFromBlack();
	}

	void Start() => Player.instance.OnDeath += InitGameOver;

	void FadeToBlack(System.Action Callback = null) => MEC.Timing.RunCoroutine(GraphicsHelper.ChangeGraphicsAlpha(1, fadeInT, Callback, null, transitionPanel));
	void FadeFromBlack(System.Action Callback = null) => MEC.Timing.RunCoroutine(GraphicsHelper.ChangeGraphicsAlpha(0, fadeOutT, Callback, null, transitionPanel));

	void InitGameOver() {
		FadeToBlack(() => {
			deathPanel.SetActive(true);

			var graphics = deathPanel.GetComponentsInChildren<Graphic>();
			foreach (var g in graphics) {
				g.color = g.color.WithA(0);
				g.raycastTarget = false;
			}

			MEC.Timing.RunCoroutine(GraphicsHelper.ChangeGraphicsAlpha(1, deathPanelShowT, EnableRaycastTargets, deathPanelShowCurve,graphics));
			deathPanel.GetComponentInChildren<Button>().onClick.AddListener(FadeDeathPanel);


			void EnableRaycastTargets() { foreach (var g in graphics) { g.raycastTarget = true; } }
			void FadeDeathPanel() => MEC.Timing.RunCoroutine(GraphicsHelper.ChangeGraphicsAlpha(0, deathPanelHideT, RestartGame, deathPanelHideCurve, graphics));
			void RestartGame() {
				var currentSceneID = SceneManager.GetActiveScene().buildIndex;
				SceneManager.LoadScene(currentSceneID);
			}
		});

	}
}
