using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory> {

    public delegate void OnSkillActivated();
    public OnSkillActivated onSkillActivatedCallback;

    public List<int> skills = new List<int>();

    public bool SetSkill(int skillID) {
        skills.Add(skillID);

        // Update UI
        if (onSkillActivatedCallback != null) {
            onSkillActivatedCallback.Invoke();
        }
        return true;
    }
}
