using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;


public interface ICrateSpawner
{
    void SpawnCrate(Transform playerFeet);
}



public class CrateSpawner : MonoBehaviour, ICrateSpawner
{
    [Tooltip("The prefab for the crate to be spawned.")]
    public GameObject cratePrefab;

    [Tooltip("The final scale to which the crate will grow.")]
    public Vector3 crateFinalScale = new Vector3(0.2f, 0.2f, 0.2f);

    [Tooltip("The speed at which the crate scales up.")]
    public float crateScaleDuration = 1f;

    [Tooltip("The duration for the player to move on top of the crate.")]
    public float playerMoveDuration = 0.5f;
    public Rigidbody2D playerRb;

    public readonly List<GameObject> spawnedCrates = new();



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get the Rigidbody component of the player
        playerRb = GetComponent<Rigidbody2D>();
    }


    /// <summary>
    /// Spawns a crate at the player's feet or on top of existing crates, scaling it up smoothly.
    /// </summary>
    /// <param name="playerFeet">The transform of the player's feet, used to determine where to spawn the crate.</param>
    public void SpawnCrate(Transform playerFeet)
    {
        // Get the spawn position (either below the player or on top of the last crate)
        Vector3 spawnPosition = GetTopPosition(playerFeet);

        // Instantiate the crate at the calculated position with scale 0
        GameObject newCrate = Instantiate(cratePrefab, spawnPosition, Quaternion.identity);
        newCrate.transform.localScale = Vector3.zero;

        // Add the new crate to the spawned crates list
        spawnedCrates.Add(newCrate);

        LevelManager.instance.crates.Add(newCrate.transform);

        // Animate the crate's scale using DOTween
        newCrate.transform.DOScale(crateFinalScale, crateScaleDuration).OnComplete(() =>
        {
            // Move the player on top of the crate after the crate has finished scaling
            MovePlayerToTop(playerFeet, newCrate.transform);
        });
    }

    private Vector3 GetTopPosition(Transform playerFeet)
    {

        // If no crates exist, spawn the first one directly under the player
        return new Vector3(playerFeet.position.x, playerFeet.position.y, playerFeet.position.z);

    }


    /// <summary>
    /// Moves the player smoothly to the top of the newly spawned crate.
    /// </summary>
    /// <param name="playerFeet">The player's feet transform, which will be moved to the top of the crate.</param>
    /// <param name="crateTransform">The transform of the crate to move the player on top of.</param>
    private void MovePlayerToTop(Transform playerFeet, Transform crateTransform)
    {

        // If the player has a Rigidbody, move them using physics-safe movement
        if (playerRb != null)
        {
            // Temporarily disable player collisions while moving
            Collider playerCollider = playerFeet.GetComponent<Collider>();
            playerCollider.enabled = false;  // Disable collider

            // Calculate the new position for the player on top of the crate
            Vector3 topPosition = new Vector3(crateTransform.position.x, crateTransform.position.y + crateFinalScale.y, crateTransform.position.z);

            // Use DOTween to move the Rigidbody smoothly (it calls Rigidbody.MovePosition under the hood)
            playerRb.DOMove(topPosition, playerMoveDuration).OnComplete(() =>
            {
                playerCollider.enabled = true;  // Re-enable collider after movement
            });
        }
        else
        {
            // If the player doesn't have a Rigidbody, fallback to DOMove
            Vector3 topPosition = new Vector3(crateTransform.position.x, crateTransform.position.y + crateFinalScale.y, crateTransform.position.z);
            playerFeet.DOMove(topPosition, playerMoveDuration);
        }
    }

    /// <summary>
    /// Handles crate destruction. If a crate is destroyed, the player will fall onto the crate below.
    /// </summary>
    /// <param name="destroyedCrate">The crate that has been destroyed.</param>
    public void OnCrateDestroyed(GameObject destroyedCrate, Transform playerFeet)
    {
        spawnedCrates.Remove(destroyedCrate);

        LevelManager.instance.crates.Add(destroyedCrate.transform);


        if (spawnedCrates.Count > 0)
        {
            // Move the player to the top of the next crate
            GameObject newTopCrate = spawnedCrates[^1];
            MovePlayerToTop(playerFeet, newTopCrate.transform);
        }
        else
        {
            // No crates left, move the player to the ground (y = 0)
            Vector3 groundPosition = new Vector3(playerFeet.position.x, 0, playerFeet.position.z);
            playerFeet.DOMove(groundPosition, playerMoveDuration);
        }
    }
}
