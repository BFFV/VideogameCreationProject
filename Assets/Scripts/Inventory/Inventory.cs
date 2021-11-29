using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory logic
public class Inventory : SceneSingleton<Inventory> {
    // Events
    public delegate void OnSkillActivated();
    public OnSkillActivated onSkillActivatedCallback;

    // Activate new skill
    public bool SetSkill(string skill) {
        Player.Instance.skills.Add(skill);

        // Update UI
        if (onSkillActivatedCallback != null) {
            onSkillActivatedCallback.Invoke();
        }
        return true;
    }
}
