using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Skill slot
public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // References
    public Button unlockButton;
    public Image icon;
    public GameObject infoUI;

    // Unlock fast run ability (later on add more skills)
    public void OnUnlockButton() {
        Player player = Player.Instance;
        if (player.exp >= 100) {
            player.exp -= 100;
            Inventory.Instance.SetSkill(0);
            unlockButton.interactable = false;
        }
    }

    // Show info
    public void OnPointerEnter(PointerEventData data) {
        infoUI.SetActive(true);
    }

    // Hide info
    public void OnPointerExit(PointerEventData data) {
        infoUI.SetActive(false);
    }

    // Update skill icon
    public void ActivateSkill(int skillID) {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 255f);
    }
}
