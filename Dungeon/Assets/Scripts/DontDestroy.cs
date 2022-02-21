using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Place this on objects that should retain state between scenes
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
