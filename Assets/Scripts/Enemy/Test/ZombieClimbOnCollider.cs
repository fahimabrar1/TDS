using UnityEngine;
using DG.Tweening;

public class ZombieClimbOnCollider : MonoBehaviour
{
    public float dirX;

    public float moveSpeed = 3f;
    public float jumpForce = 3f;

    public Rigidbody2D rb;



    public MyCollider2D frontCollier;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        frontCollier.OnTriggerEnter2DEvent.AddListener((col) => OnTriggerEnterFront2D(col));

    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }



    #region Colliders

    /// <summary>
    /// Detects when a player enters the attack zone and initiates the attack routine on the closest one.
    /// </summary>
    public virtual void OnTriggerEnterFront2D(Collider2D other)
    {
        switch (other.tag)
        {

            case "Enemy":
                rb.AddForce(Vector2.up * jumpForce);
                break;
        }
    }


    #endregion
}
