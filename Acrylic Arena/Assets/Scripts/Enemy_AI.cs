using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float stoppingDistance = 1f; 
    public float detectDistance = 6f; 
    public string playerTag = "Player"; 

    private Transform player; // Reference to the player's transform
    private Vector3 targetPosition; // Position the enemy is moving towards
    private bool playerSeen = false; 

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
                
                Debug.Log("Saw you!");
            }

            if (playerSeen && distanceToTarget > stoppingDistance)
            {
                // Move enemy towards the target position
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }
}