
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Settings")]
public class EnemyProfile : CharacterProfile, ICombatProfile {
	[Space, Header("Combat Settings")]
	public int MaxHP;
	public int Damage;
	public float SecondsAfterAttack;
	public float SecondsBeforeAttack;
	public float AttackSpeedMulti;
	public float AttackRange;
	public float AfterDeathTime;

	int ICombatProfile.MaxHP => MaxHP;
	int ICombatProfile.Damage => Damage;

	//public float AttackRotationSpeed; // locked atm
}
