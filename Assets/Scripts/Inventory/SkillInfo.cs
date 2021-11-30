using UnityEngine;
using UnityEngine.UI;

// Information panel for skills
public class SkillInfo : MonoBehaviour {
    // References
    Inventory inventory;
    public Image icon;
    public Text cost;

    // Setup
    void Start() {
        inventory = Inventory.Instance;
        inventory.onSkillActivatedCallback += Unlocked;

        // Skill already unlocked
        PlayerData state;
        if (GameManager.Instance.warping) {
            state = GameManager.Instance.warpData;
        } else {
            state = GameManager.Instance.playerData;
        }
        if (state.spawnSkills.Contains("Sprint")) {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 255f);
            cost.text = "Skill Unlocked!";
        }
    }

    // Show info of unlocked skill
    void Unlocked() {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 255f);
        cost.text = "Skill Unlocked!";
    }
}
