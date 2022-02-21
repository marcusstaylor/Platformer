using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    private Animator anim; // Allow for animation
    // Stats
    public int[] damage = {1, 2, 3, 4, 5};
    public float[] pushForce = {2.0f, 2.4f, 2.8f, 3.2f, 3.6f}; //2.0f just adds the f to signify the .0 is important. sig figs.
    private float cooldown = 0.5f;
    private float lastSwing;

    // Upgrade
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>(); // References the saved list of weapon sprites. Allows us to later upgrade weapon_0 to weapon_1 etc...
        anim = GetComponent<Animator>();
    }

    protected override void Update() {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space)) { // Maps swing key to space?
            if(Time.time - lastSwing > cooldown) { // Test to see if cooldown has passed since last swing.
                lastSwing = Time.time; // Timestamp most recent swing
                Swing(); // Swing again
            }
        }
    }

    protected override void OnCollide(Collider2D coll) {
        if (coll.tag == "Fighter") { // Marks fighters as something able to take and deal damage
            if (coll.name == "Player") { // Prevents holding sword counting as self hit
                return;
            }

            // Assign damage by creating a damage object and passing it to a fighter object
            Damage dmg = new Damage() { // Did not know you could do constructors like this tbh
                damageAmount = damage[weaponLevel], // Scales damage to level
                origin = transform.position,
                pushForce = pushForce[weaponLevel] // Scales knockback to level
            };

            coll.SendMessage("ReceiveDamage", dmg); // Sends damage to object hit
        }
    }

    private void Swing() {
        anim.SetTrigger("Swing");
    }

    public void UpgradeWeapon() {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void SetWeaponLevel(int level) {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
