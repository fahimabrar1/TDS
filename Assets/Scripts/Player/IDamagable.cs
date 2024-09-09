public interface IDamagable
{
    void OnTakeDamage(int damage);  // Method to handle taking damage
}


public interface IPlayerDamagable : IDamagable { }
public interface IEnemyDamagable : IDamagable { }
