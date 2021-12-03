using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Skill slot
public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    // References
    public Button unlockButton;
    public Image icon;
    public GameObject infoUI;
    public SkillInfo info;

    // Skill info
    public string skill;
    public string skillName;
    public int cost;
    public string description;

    // Skill already unlocked
    void Start() {
        info = infoUI.GetComponent<SkillInfo>();
        PlayerData state;
        if (GameManager.Instance.warping) {
            state = GameManager.Instance.warpData;
        } else {
            state = GameManager.Instance.playerData;
        }
        if (state.spawnSkills.Contains(skill)) {
            unlockButton.interactable = false;
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
        }
    }

    // Unlock skill
    public void OnUnlockButton() {
        Player player = Player.Instance;
        if (player.exp >= cost) {
            player.exp -= cost;
            GUIManager.Instance.UpdatePlayerExp(player.exp);
            Inventory.Instance.SetSkill(skill);
            unlockButton.interactable = false;
            info.UpdateInfo(icon.sprite, skillName, 0, description);
            infoUI.SetActive(true);
        }
    }

    // Show info
    public void OnPointerEnter(PointerEventData data) {
        int newCost = cost;
        if (Player.Instance.skills.Contains(skill)) {
            newCost = 0;
        }
        info.UpdateInfo(icon.sprite, skillName, newCost, description);
        infoUI.SetActive(true);
    }

    // Hide info
    public void OnPointerExit(PointerEventData data) {
        infoUI.SetActive(false);
    }

    // Update skill icon
    public void ActivateSkill() {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 255f);
    }
}
