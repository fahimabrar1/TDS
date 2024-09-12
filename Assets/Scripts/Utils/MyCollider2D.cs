using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class UnityColliderTrigger2DEvent : UnityEvent<Collider2D> { }

[Serializable]
public class UnityColliderCollision2DEvent : UnityEvent<Collision2D> { }

public class MyCollider2D : MonoBehaviour
{
    public UnityColliderTrigger2DEvent OnTriggerEnter2DEvent;
    public UnityColliderTrigger2DEvent OnTriggerStay2DEvent;
    public UnityColliderTrigger2DEvent OnTriggerExit2DEvent;
    public UnityColliderCollision2DEvent OnCollisionEnter2DEvent;
    public UnityColliderCollision2DEvent OnCollisionStay2DEvent;
    public UnityColliderCollision2DEvent OnCollisionExit2DEvent;


    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        MyDebug.Log("Trigger Enter");
        OnTriggerEnter2DEvent?.Invoke(other);
    }


    /// <summary>
    /// Sent each frame where another object is within a trigger collider
    /// attached to this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerStay2D(Collider2D other)
    {

        OnTriggerStay2DEvent?.Invoke(other);
    }



    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExit2DEvent?.Invoke(other);
    }


    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        OnCollisionEnter2DEvent?.Invoke(other);
    }



    /// <summary>
    /// Sent each frame where a collider on another object is touching
    /// this object's collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionStay2D(Collision2D other)
    {
        OnCollisionStay2DEvent?.Invoke(other);

    }



    /// <summary>
    /// Sent when a collider on another object stops touching this
    /// object's collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionExit2D(Collision2D other)
    {
        OnCollisionEnter2DEvent?.Invoke(other);

    }
}
