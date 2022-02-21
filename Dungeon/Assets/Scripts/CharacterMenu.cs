using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for text/image manipulation

public class CharacterMenu : MonoBehaviour
{
    // Text field reference
    public Text levelText, hitpointText, goldText, upgradeCostText, xpText;

    // Logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar; // Changes xp bar size by progress

    // Character select logic (not visual)
    public void OnArrowClick(bool right) {
        if (right) {
            currentCharacterSelection++;

            // Loops back around if reaches end
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count) {
                currentCharacterSelection = 0;
            }

            OnSelectionChanged();
        } else {
            currentCharacterSelection--;

            // Loops back around if reaches end
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count) {
                currentCharacterSelection = 0;
            }

            OnSelectionChanged();
        }
    }

    // Character select logic (visual)
    private void OnSelectionChanged() {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection); // New Player function to change in game sprite
    }

    // Weapon upgrade
    public void OnUpgradeClick() {
        if (GameManager.instance.TryUpgradeWeapon()) {
            UpdateMenu();
        }
    }

    // Update Character Info
    public void UpdateMenu() {
        // Update weapon upgrade sprite/cost
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count) {
            upgradeCostText.text = "Max";
        } else {
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        }

        // Update HP
        hitpointText.text = GameManager.instance.player.hitPoint.ToString();
        // Update gold
        goldText.text = GameManager.instance.gold.ToString();
        // Update level
        int currLevel = GameManager.instance.GetCurrentLevel();
        levelText.text = currLevel.ToString();
        // Update xp bar+
        
        if (currLevel == GameManager.instance.xpTable.Count) {
            xpText.text = GameManager.instance.xp.ToString() + " total XP"; // Display total xp if max
            xpBar.localScale = Vector3.one; // Completely fill xp bar
        } else {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1); // find how much xp it took to get to current level
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp; // Find xp needed to get to next level
            int currXpIntoLevel = GameManager.instance.xp - prevLevelXp;

            // calculated percent of current level completed, scale xp bar to reflect
            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }
    }
}
