using System.Collections;
using UnityEngine;

// Checkpoints
public class Checkpoint : MonoBehaviour {

    // Level
    public string level;

    // Player enters checkpoint
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player.Instance.currentCheckpoint = gameObject.GetComponent<Checkpoint>();
            GUIManager.Instance.ShowAction("Press <G> to save your progress", true);
        }
    }

    // Player exits checkpoint
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player.Instance.currentCheckpoint = null;
            GUIManager.Instance.ShowAction("", false);
        }
    }

    // Save game state
    public void SaveGame() {
        AudioManager.Instance.PlaySound("save", 2f);
        GameManager.Instance.SaveCheckpoint(level, transform.position);
    }
}
