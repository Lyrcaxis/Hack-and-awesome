
using UnityEngine;

public abstract class CharacterProfile : ScriptableObject {
	[Header("Movement Settings")]
	public float Speed;
	public float Acceleration;
	public float RotationSpeed;
}