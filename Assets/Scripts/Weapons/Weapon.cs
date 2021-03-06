using UnityEngine;

// Weapon Item
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
            Inventory.Instance.SetWeapon(gameObject.tag);
            AudioManager.Instance.PlaySound("item", 1.5f);
            if (gameObject.tag == "Gun") {
                GUIManager.Instance.ShowEvent("You have acquired a " + gameObject.tag + "!");
            }
            if (gameObject.tag == "Wind") {
                GUIManager.Instance.ShowEvent("You have acquired the Typhoon!");
            }
            if (gameObject.tag == "Fire") {
                GUIManager.Instance.ShowEvent("You have acquired the Flame!");
            }
            Destroy(gameObject);
        }
    }
}
