using UnityEngine;
using UnityEngine.UI;

// Information panel for weapons
public class WeaponInfo : MonoBehaviour {

    // References
    public Image icon;
    public Text weaponName;
    public Text location;
    public Text description;

    // Show info of weapon
    public void UpdateInfo(Sprite newIcon, string newName, string newLocation, string newDesc) {
        icon.sprite = newIcon;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
        weaponName.text = newName;
        location.text = newLocation;
        if (newLocation != "Weapon Acquired!") {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0.47f);
        }
        description.text = newDesc;
    }
}
