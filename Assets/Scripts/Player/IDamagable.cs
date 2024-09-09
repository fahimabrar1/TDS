using UnityEngine;

public interface IDamagable
{
    void OnTakeDamage(int damage);  // Method to handle taking damage
    Transform GetTransform();  // Method to handle the transform of the object
}


public interface IPlayerDamagable : IDamagable { }
public interface IEnemyDamagable : IDamagable { }
