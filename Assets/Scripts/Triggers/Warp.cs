using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

    // Destination
    public string destination;
    public Vector3 position;

    // Player enters warp
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameManager.Instance.Warp(destination, position);
        }
    }
}
