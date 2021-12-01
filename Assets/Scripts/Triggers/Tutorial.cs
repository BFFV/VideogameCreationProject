using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    // Tutorial text
    public string text;

    // Player enters tutorial trigger
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GUIManager.Instance.ShowTutorial(text, true);
        }
    }

    // Player exits tutorial trigger
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GUIManager.Instance.ShowTutorial("", false);
        }
    }
}
