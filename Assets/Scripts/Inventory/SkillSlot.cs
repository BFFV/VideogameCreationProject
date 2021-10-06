using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour {

    public Button unlockButton;

    public Image icon;

    // Unlock fast run ability
    public void OnUnlockButton() {
        Player player = Player.Instance;
        if (player.exp >= 100) {
            player.exp -= 100;
            Inventory.Instance.SetSkill(0);
            unlockButton.interactable = false;
        }
    }

    // Update skill icon
    public void ActivateSkill(int skillID) {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 255f);
    }
}
