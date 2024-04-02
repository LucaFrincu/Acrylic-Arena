using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float stoppingDistance = 1f; 
    public float detectDistance = 6f; 
    public string playerTag = "Player";
    public bool hasCollided = false;

    private Transform player; // Reference to the player's transform
    private Vector3 targetPosition; // Position the enemy is moving towards
    private bool playerSeen = false;

    public float squareSpawnOffset = 2f; // Offset for spawning the square

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate direction to the player
            Vector3 direction = (targetPosition - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // Check if the player is within detect distance
            if (Vector3.Distance(transform.position, player.position) <= detectDistance)
            {
                targetPosition = player.position;
                playerSeen = true;
                
                //Debug.Log("Saw you!");
            }

            if (playerSeen && distanceToTarget > stoppingDistance)
            {
                // Move enemy towards the target position
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.name == "TemporaryCollider" && hasCollided == false)
        {
            hasCollided = true;
            
            SpawnColoredSquare(other.transform.position);
            Destroy(gameObject); // Destroy the enemy or handle accordingly
            FindObjectOfType<EnemySpawner>().OnEnemyDestroyed(); // Update the spawner's enemy count
        }
    }

    void SpawnColoredSquare(Vector3 collisionPosition)
    {
        // Calculate spawn position for the square
        Vector3 direction = (collisionPosition - transform.position).normalized;
        Vector3 spawnPosition = transform.position - direction * squareSpawnOffset;

        // Create the square
        GameObject square = GameObject.CreatePrimitive(PrimitiveType.Cube);
        square.tag = "square";
        square.transform.position = spawnPosition;
        square.transform.localScale = new Vector3(1f, 0.1f, 1f); // Make it a flat square
        Renderer renderer = square.GetComponent<Renderer>();
        renderer.material.color = new Color(1f, 0f, 1f); // Set color to magenta

        Destroy(square.GetComponent<Collider>());
        BoxCollider collider = square.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        // Optional: Destroy the square after some time
        Destroy(square, 5f);
    }
}

