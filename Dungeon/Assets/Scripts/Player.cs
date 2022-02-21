using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover // Inherits
{    
    // Initialize current sprite ID upon awake
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Death() {
        isAlive = false;
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
    }

    protected override void ReceiveDamage(Damage dmg) {
        if (!isAlive) {
            return;
        }
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }

    private void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal"); // Detect if movement keys are pressed
        float y = Input.GetAxisRaw("Vertical"); 

        if (isAlive) {
            UpdateMotor(new Vector3(x, y, 0));
        }
    }

    // Manually change sprite ID
    public void SwapSprite(int skinID) {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinID];
    }

    public void LevelUp() {
        maxHitPoint++;
        hitPoint = maxHitPoint;
        GameManager.instance.OnHitpointChange();
    }

    public void SetLevel(int level) {
        for (int i = 0; i < level; i++) {
            LevelUp();
        }
    }

    public void Heal(int healAmount) {
        if (hitPoint == maxHitPoint) {
            return;
        }

        hitPoint += healAmount;
        if (hitPoint > maxHitPoint) {
            hitPoint = maxHitPoint;
        }
        GameManager.instance.ShowText("+" + healAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitpointChange();
    }

    public void Respawn() {
        Heal(maxHitPoint);
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }
}
