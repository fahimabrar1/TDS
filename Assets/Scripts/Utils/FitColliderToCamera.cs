using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FitColliderToCamera : MonoBehaviour
{
    public Camera cam;
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        UpdateColliderSize();
    }



    void UpdateColliderSize()
    {
        // Orthographic camera size is half of the vertical size
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        boxCollider.size = new Vector2(cameraWidth, cameraHeight);
        boxCollider.offset = Vector2.zero; // Ensure the collider is centered
    }
}
