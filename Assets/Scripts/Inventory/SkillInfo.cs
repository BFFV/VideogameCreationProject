using UnityEngine;
using UnityEngine.UI;

// Information panel for skills
public class SkillInfo : MonoBehaviour {

    // References
    public Image icon;
    public Text skillName;
    public Text cost;
    public Text description;
    public Text mpCost;

    // Show info of skill
    public void UpdateInfo(Sprite newIcon, string newName, int newCost, string newDesc, string mp) {
        icon.sprite = newIcon;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
        skillName.text = newName;
        cost.text = "Skill Unlocked!";
        if (newCost > 0) {
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0.47f);
            cost.text = "Unlock: " + newCost + " EXP";
            if (newName == "Black Hole") {
                cost.text = "Unlock: Defeat the Dark Lord";
            } else if (newName == "Holy Beam") {
                cost.text = "Unlock: Defeat the Holy Lord";
            }
        }
        description.text = newDesc;
        mpCost.text = "MP Cost: " + mp;
    }
}
