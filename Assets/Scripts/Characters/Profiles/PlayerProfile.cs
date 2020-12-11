
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Settings")]
public class PlayerProfile : CharacterProfile, ICombatProfile {
	[Space, Header("Combat Settings")]
	public int MaxHP;
	public int Damage;
	public float AttackRotationSpeed;
	[Space,Header("Combo Settings")]
	public float moveForcePerAttack;
	public float maxSecondsToKeepCombo = default;
	public AnimationCurve combo1MoveCurve;
	public AnimationCurve combo2MoveCurve;
	public AnimationCurve combo3MoveCurve;
	[Range(0, 1)] public float minComboSkipTime = default;
	public AnimationCurve comboSkipTimeCurve = default;

	int ICombatProfile.MaxHP => MaxHP;
	int ICombatProfile.Damage => Damage;
}
