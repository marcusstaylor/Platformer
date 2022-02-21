using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    public int xpDropped = 1;

    // Chase logic
    public float triggerLength = 1; // Distance that will aggro enemy
    public float chaseLength = 5; // How far the mob will chase the player
    private bool isChasing;
    private bool isTouchingPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start() {
        base.Start();
        playerTransform = GameManager.instance.player.transform; // Searches for player position
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>(); // Gets the child of the enemy, which in index 0 should be the hitbox
    }

    private void FixedUpdate() {
        // Check if player is still in chase range
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLength) {
            // Check if player is in range to aggro
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength) {
                isChasing = true;
            }

            if (isChasing) {
                if (!isTouchingPlayer) {
                    UpdateMotor((playerTransform.position - transform.position).normalized); // Attempts to minimize distance between enemy and player
                }
            } else {
                UpdateMotor(startingPosition - transform.position); // Attempts to return from current position to starting position
            }
        } else {
            UpdateMotor(startingPosition - transform.position); // Attempts to return from current position to starting position
            isChasing = false;
        }

        // Check if colliding with player
        isTouchingPlayer = false;
        hitbox.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null) {
                continue;
            }

            if(hits[i].tag == "Fighter" && hits[i].name == "Player") {
                isTouchingPlayer = true;
            }

            hits[i] = null; // Clean array
        }
    }

    protected override void Death() {
        Destroy(gameObject); // Delete enemny
        GameManager.instance.GrantXp(xpDropped); // Grant player xp
        GameManager.instance.ShowText("+" + xpDropped + "xp", 30, Color.green, transform.position, Vector3.up * 40, 0.5f); // Show text
    }
}
