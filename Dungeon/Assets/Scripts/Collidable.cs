using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>(); // Requires collidable object to have hitbox
    }

    protected virtual void Update() {
        // Collision logic
        boxCollider.OverlapCollider(filter, hits); // Finds all things colliding and adds them to the hits array
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i] == null) {
                continue;
            }
            
            OnCollide(hits[i]);
            // Debug.Log(hits[i].name); // Print collided object to console

            hits[i] = null; // Clean array
        }
    }

    protected virtual void OnCollide(Collider2D coll) {
        Debug.Log("OnCollide not yet implemented for" + this.name);
    }
}
