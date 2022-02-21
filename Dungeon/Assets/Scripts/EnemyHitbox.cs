using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    public int damage;
    public float pushForce;

    protected override void OnCollide(Collider2D coll) {
        if (coll.tag == "Fighter" && coll.name == "Player") {
            // Create a new damage object and pass it to the player.
            Damage dmg = new Damage() { // Did not know you could do constructors like this tbh
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg); // Sends damage to object hit
        }
    }
}
