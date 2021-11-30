using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    // Level
    public string level;

    // Player enters checkpoint
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player.Instance.currentCheckpoint = gameObject.GetComponent<Checkpoint>();
            GUIManager.Instance.ToggleSave(true);
        }
    }

    // Player exits checkpoint
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player.Instance.currentCheckpoint = null;
            GUIManager.Instance.ToggleSave(false);
        }
    }

    // Save game state
    public void SaveGame() {
        GameManager.Instance.SaveCheckpoint(level, transform.position);
        GUIManager.Instance.ShowEvent("Progress Saved!");
    }
}
