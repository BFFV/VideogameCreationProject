using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour {

    // Only spawn if player doesn't already have this weapon
    void Start() {
        PlayerData state;
        if (GameManager.Instance.warping) {
            state = GameManager.Instance.warpData;
        } else {
            state = GameManager.Instance.playerData;
        }
        string weaponType = gameObject.tag;
        if (state.spawnWeapons.Contains(weaponType)) {
            Destroy(gameObject);
        }
    }

    // Player obtains weapon
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player.Instance.weapons.Add(gameObject.tag);
            GUIManager.Instance.ToggleGunIcon(true);  // will be changed later
            GUIManager.Instance.ShowEvent("You have acquired a " + gameObject.tag + "!");
            Destroy(gameObject);
        }
    }
}
