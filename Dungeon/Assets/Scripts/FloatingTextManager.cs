using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed for floating text

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    // Creates a pool of test strings to pull from
    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Start() {
    }

    private void Update() {
        foreach (FloatingText txt in floatingTexts) {
            txt.UpdateFloatingText(); // Calls the FloatingText update function every frame
        }
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) // Uses same parameters from FloatingText class
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg; // Sets actual text in game
        floatingText.txt.fontSize = fontSize; // Sets font size in game
        floatingText.txt.color = color; // Sets color of text in game
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(position); // Sets relative to camera so you can zoom in/out without affecting text+
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();
    }
    // Pulls a text string from pool
    private FloatingText GetFloatingText() {
        FloatingText txt = floatingTexts.Find(t => !t.active); // "Look for a text in floatingTexts that is not active"

        if (txt  == null) {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab); // Text template
            txt.go.transform.SetParent(textContainer.transform); // Grants functions to new text
            txt.txt = txt.go.GetComponent<Text>(); // txt.txt = the txt value of the txt object


            floatingTexts.Add(txt); // Add to our previous array
        }

        return(txt);
    }
}
