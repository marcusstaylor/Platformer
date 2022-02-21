using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    // Will be linked to the object for camera to lock on to.
    private Transform lookAt;

    // Boundaries before camera snap
    public float boundX = 0.15F;
    public float boundY = 0.05f;

    private void Start() {
        lookAt = GameObject.Find("Player").transform; // Locks camera to player after changing scenes
    }

    private void LateUpdate() {
        Vector3 delta = Vector3.zero;

        // Check if player has left camera bounds on X axis
        float deltaX = lookAt.position.x - transform.position.x;
        if(deltaX > boundX || deltaX < -boundX) {
            if (transform.position.x < lookAt.position.x) { // Right side
                delta.x = deltaX - boundX;
            } else { // Left side
                delta.x = deltaX + boundX;
            }
        }

        // Check if player has left camera bounds on Y axis
        float deltaY = lookAt.position.y - transform.position.y;
        if(deltaY > boundY || deltaY < -boundY) {
            if (transform.position.y < lookAt.position.y) { // Top side
                delta.y = deltaY - boundY;
            } else { // Bottom side
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0); // Move camera
    }
}
