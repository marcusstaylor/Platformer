using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() {
        // Prevents more than one GameManager from being created when swapping between scenes
        if (GameManager.instance != null) {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return; // Prevents next block from running
        }
        instance = this; // Set game instance
        SceneManager.sceneLoaded += LoadState; // Needs a state to call from LoadState() function
        SceneManager.sceneLoaded += OnSceneLoaded; // Calls every time new scene is entered
    }

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform healthBar;
    public Animator deathMenuAnim;
    public GameObject hud;
    public GameObject menu;

    // Logic
    public int gold;
    public int xp;

    // Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) { // Same params from Text scripts
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration); // Pass in here so we dont have to reference FloatingTextManager directly. not sure why.
    }

    public bool TryUpgradeWeapon() {
        // Check if weapon is already max level
        if (weaponPrices.Count <= weapon.weaponLevel) {
            return false;
        }

        // Check if player can afford current upgrade
        if (gold >= weaponPrices[weapon.weaponLevel]) {
            gold -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Update health bar
    public void OnHitpointChange() {
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;
        healthBar.localScale = new Vector3(1, ratio, 1); // Scale health bar to current health
    }

    // Save game
    // Loads preferredSkin, gold, xp, weaponLevel
    public void SaveState() {
        string s = "";

        s += "0" + "|"; // Use 0 for skin because we dont have skins yet
        s += gold.ToString() + "|";
        s += xp.ToString() + "|";
        s += weapon.weaponLevel.ToString(); 

        PlayerPrefs.SetString("SaveState", s);
    }

    // Called every time a new scene is loaded
    public void OnSceneLoaded(Scene sc, LoadSceneMode mode) {
        // Teleport player to scene spawnpoint on load.
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Load game
    public void LoadState(Scene sc, LoadSceneMode mode) { // Creates save to be called in the Awake()
        SceneManager.sceneLoaded -= LoadState; // Prevents stats from reinitalizing every time scene changes

        // Prevents loading an empty save state
        if(!PlayerPrefs.HasKey("SaveState")) {
            return;
        }
        
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // skin = data[0];
        gold = int.Parse(data[1]);
        // Get current xp
        xp = int.Parse(data[2]);
        // Set level to current level
        player.SetLevel(GetCurrentLevel());
        // Set current weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));
    }

    // Determines how much XP needed to advance to next level
    public int GetXpToLevel(int level) {
        int r = 0;
        int xpMissing = 0;

        while (r < level) {
            xpMissing += xpTable[r];
            r++;
        }

        return xpMissing;
    }

    public int GetCurrentLevel() {
        int r = 0; // return value
        int add = 0; // determines if ready to level

        while (xp >= add) { // Checks if > level 1
            add += xpTable[r]; // Checks bound for next level
            r++;

            if (r == xpTable.Count) { // Checks for max level
                return r;
            } 
        }

        return r; // Returns level values calculated by loop
    }

    public void GrantXp(int exper) {
        int currLevel = GetCurrentLevel();
        xp += exper;
        if (currLevel < GetCurrentLevel()) {
            LevelUp();
        }
    }

    public void LevelUp() {
        Debug.Log("Level Up!");
        player.LevelUp(); // Level up player as well, not just game state
    }

    // Respawn
    public void Respawn() {
        deathMenuAnim.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main"); // Reload main scene on respawn
        player.Respawn();
    }
}
