using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed for floating text

public class FloatingText
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show() {
        active = true;
        lastShown = Time.time; // Now
        go.SetActive(active);
    }

    public void Hide() { // Used to remove text window rather than destroying it
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText() {
        if (!active) {
            return; // Does not execute subsequent code if not active
        }

        // If now - when it was opened > show duration, close text
        if (Time.time - lastShown > duration) {
            Hide();
        }

        go.transform.position += motion * Time.deltaTime; // Move text
    }
}
