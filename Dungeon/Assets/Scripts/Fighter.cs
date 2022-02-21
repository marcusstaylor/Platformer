using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitPoint = 10;
    public int maxHitPoint = 10;
    public float pushRecoverySpeed = 0.2f; // Knockback time
    protected Vector3 pushDirection;
    protected float immuneTime = 1.0f; // I-frame Time
    protected float lastImmune; // Time between last i-frame // Not sure this is working.

    protected virtual void ReceiveDamage(Damage dmg) {
        if (Time.time - lastImmune > immuneTime) { // Checks to see if i-frames are over
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce; // Pushes hit object in direction away from damage source, scaled by knockback.
            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 15, Color.red, transform.position, Vector3.zero, 0.5f);
            // Show damage in red on character position which does not move for 0.5 seconds.
        }

        if (hitPoint <= 0) {
            hitPoint = 0;
            Death();
        }
    }

    protected virtual void Death() {

    }
}
