using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player player = Player.Instance;
            GameManager.Instance.SaveCheckpoint(transform.position, player.exp, player.weapons, player.skills);
        }
    }
}
