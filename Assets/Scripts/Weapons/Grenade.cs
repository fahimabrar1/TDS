using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionDelay = 1f;  // Time after stopping before explosion
    public float explosionRadius = 1; // Radius of the explosion
    public int explosionDamage = 50;   // Damage dealt by the explosion
    public LayerMask enemyLayer;       // Layer mask to detect enemies
    public GameObject explosionEffectPrefab; // Optional explosion effect prefab

    private Rigidbody2D rb2d;

    private bool hasExploded = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the grenade has stopped moving (i.e., velocity is near zero)
        if (rb2d.velocity.magnitude < 0.1f && !hasExploded)
        {
            // Start the explosion sequence
            StartCoroutine(ExplodeAfterDelay());
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);

        // Trigger explosion logic
        Explode();
    }

    private void Explode()
    {
        if (hasExploded) return; // Prevent multiple explosions

        hasExploded = true;

        // Optionally, instantiate an explosion effect at the grenade's position
        if (explosionEffectPrefab)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Detect all enemies within the explosion radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        // Damage all enemies within the radius
        foreach (Collider2D enemyCollider in enemies)
        {
            if (enemyCollider.TryGetComponent(out Enemy enemy))
            {
                enemy.OnTakeDamage(explosionDamage);  // Deal damage to each enemy
            }
        }

        // Destroy the grenade after the explosion
        Destroy(gameObject);
    }

    // Optional: To visualize the explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
