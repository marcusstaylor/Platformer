using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectible // Inherit functions from Collidable script
{

    public Sprite emptyChest;
    public int amt = 5;

    protected override void OnCollect() {
        
        if (!collected) { // only colleciable once
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest; // Change sprite after open
            GameManager.instance.gold += amt; // Adds gold to current save state
            GameManager.instance.ShowText("You found " + amt + " gold!", 25, Color.yellow, transform.position, Vector3.up * 25, 1.5f);
            // Show x text at size 25 in yellow, on chest's position, that moves up at 25 pixels/second for 1.5 seconds.
        }

        base.OnCollect();
        
    }
}
