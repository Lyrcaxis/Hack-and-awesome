using System.Collections.Generic;

using UnityEngine;

public partial class HealthBar : MonoBehaviour {

	[Range(0f, 1f)] 
	public float opacity = 0.75f;
	public Color color = Color.green;
	public float yOffset = 5f;
	public float width = 60;
	public AnimationCurve easingCurve;

	public float DrawPercentage => animHelper.GetCurrentPercentage(easingCurve);


	float percentage = 1f;
	AnimationHelper animHelper;

	public static List<HealthBar> Active = new List<HealthBar>();

	void Start() {
		var entity = GetComponent<ICombatEntity>();

		entity.OnDamaged += (_) => {
			var prevPercentage = percentage;
			percentage = (entity.HP / (float) entity.profile.MaxHP);
			animHelper = new AnimationHelper(prevPercentage, percentage);
		};
	}
	void OnEnable() {
		percentage = 1f;
		animHelper = new AnimationHelper(1f, 1f);

		Active.Add(this);
	}
	void OnDisable() => Active.Remove(this);
}
