using UnityEngine;

public class Zombie : Enemy
{
    [Tooltip("The speed at which the zombie moves towards the player.")]
    public float moveSpeed = 2f;

    private Transform target;

    private void Start()
    {
        Health = 100;
        // target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (target != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}