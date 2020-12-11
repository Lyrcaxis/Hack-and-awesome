public interface IPoolEntity {
    System.Action OnSpawn { get; set; }
    System.Action OnDespawn { get; set; }
}