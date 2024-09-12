using UnityEngine;

public class DummyDamagable : MonoBehaviour, IPlayerDamagable
{
    public Transform GetTransform()
    {
        return transform;
    }

    public virtual void OnTakeDamage(int damage)
    {

    }
}