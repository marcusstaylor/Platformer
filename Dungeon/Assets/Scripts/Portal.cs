using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // This one

public class Portal : Collidable
{
    public string[] sceneNames;

    protected override void OnCollide(Collider2D coll) {
        if (coll.name == "Player") {
            GameManager.instance.SaveState(); // Save game after loading new scene
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)]; // Teleport player to random dungeon
            SceneManager.LoadScene(sceneName); // Requires SceneManagement call above
        }
    }
}
