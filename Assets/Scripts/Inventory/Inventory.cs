using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory logic
public class Inventory : SceneSingleton<Inventory> {
    // Events
    public delegate void OnSkillActivated();
    public OnSkillActivated onSkillActivatedCallback;

    // Skill list (not in use)
    public List<int> skills = new List<int>();

    // Set new skill (not in use)
    public bool SetSkill(int skillID) {
        skills.Add(skillID);

        // Update UI
        if (onSkillActivatedCallback != null) {
            onSkillActivatedCallback.Invoke();
        }
        return true;
    }
}
