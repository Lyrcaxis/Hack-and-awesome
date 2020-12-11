using UnityEngine;

public static class CharacterAnimationHelper {
	public static int Combo1 = Animator.StringToHash("Attack 1");
	public static int Combo2 = Animator.StringToHash("Attack 2");
	public static int Combo3 = Animator.StringToHash("Attack 3");

	public static int AttackSpeed = Animator.StringToHash("AttackSpeed");
	public static int Cooldown = Animator.StringToHash("Cooldown");

	public static int DirZ = Animator.StringToHash("DirZ");
	public static int DirX = Animator.StringToHash("DirX");
	public static int Speed = Animator.StringToHash("Speed");

	public static int Roll = Animator.StringToHash("Roll");
	public static int Die = Animator.StringToHash("Die");

	public static int GetHashForCombo(int currentCombo) {
		switch (currentCombo) {
			case 0: { return Combo1; }
			case 1: { return Combo2; }
			case 2: { return Combo3; }
		}
		throw new UnityException("Unsupported combo number");
	}
	public static AnimationCurve GetSettingForCombo(int currentCombo, PlayerProfile settings) {
		switch (currentCombo) {
			case 0: { return settings.combo1MoveCurve; }
			case 1: { return settings.combo2MoveCurve; }
			case 2: { return settings.combo3MoveCurve; }
		}
		throw new UnityException("Unsupported combo number");
	}
}
