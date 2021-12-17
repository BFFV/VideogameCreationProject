using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ending
public class EndGame : MonoBehaviour {

    // Player finishes the game
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameManager.Instance.Ending();
        }
    }
}
