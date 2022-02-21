using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter // Forces inheritence, cannot be instantiated
{
    private Vector3 originalSize;
    
    private BoxCollider2D boxCollider; // Collision hitboxes
    private Vector3 moveDelta = new Vector3(0,0,0); // Movement
    private RaycastHit2D hit; // Hit detection
    public float ySpeed = 0.75f; // Movement speed
    public float xSpeed = 1.0f; // Movement speed
    public Vector3 currSpeed;
    public float xAcc = 0.2f;

    protected virtual void Start()
    {
        originalSize = transform.localScale; // Allows for resizing of sprites and have them not reset on play
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Player movement
    protected virtual void UpdateMotor(Vector3 input) { // Protected virtual means it can be overridden 
        // Reset moveDelta
        moveDelta = new Vector3((input.x * xSpeed), (input.y * ySpeed), 0); // Scales movement to movespeed

        // Flip sprite when changing directions
        if (moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);

        // Include pushing on hit
        moveDelta += pushDirection;
        // Then reduce push velocity every frame
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed); // Goes from pushDirection to 0 at a rate of pushRecoverySpeed per frame

        // Vertical hit detection
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        // Vertical movement
        if (hit.collider == null) {
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        // Horizontal hit detection
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        // Horizontal movement
        if (hit.collider == null) {
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
