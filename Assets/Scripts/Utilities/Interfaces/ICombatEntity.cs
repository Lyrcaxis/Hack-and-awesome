public interface ICombatEntity {
    System.Action OnDeath { get; set; }
    System.Action<int> OnDamaged { get; set; }

    int HP { get; set; }

    ICombatProfile profile { get; }
}
