using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Weapon slot
public class WeaponSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    // References
    public Image icon;
    public GameObject infoUI;
    public WeaponInfo info;

    // Weapon info
    public string weapon;
    public string weaponName;
    public string location;
    public string description;

    // Weapon already unlocked
    void Start() {
        info = infoUI.GetComponent<WeaponInfo>();
        PlayerData state;
        if (GameManager.Instance.warping) {
            state = GameManager.Instance.warpData;
        } else {
            state = GameManager.Instance.playerData;
        }
        if (state.spawnWeapons.Contains(weapon)) {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
        }
    }

    // Show info
    public void OnPointerEnter(PointerEventData data) {
        string newLocation = location;
        if (Player.Instance.weapons.Contains(weapon)) {
            newLocation = "Weapon Acquired!";
        }
        info.UpdateInfo(icon.sprite, weaponName, newLocation, description);
        infoUI.SetActive(true);
    }

    // Hide info
    public void OnPointerExit(PointerEventData data) {
        infoUI.SetActive(false);
    }

    // Update weapon icon
    public void ActivateWeapon() {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 255f);
    }
}
