using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    public string message; // Change this to array later for more than one message?
    
    private float cooldown = 4.0f; // Change to public later to make it customizable in UI?
    private float lastShout = -4.0f;

    protected override void OnCollide(Collider2D coll) {
        if (Time.time - lastShout > cooldown) {
            lastShout = Time.time;
            GameManager.instance.ShowText(message, 25, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.up * 5, 4.0f);
        }
    }
}
