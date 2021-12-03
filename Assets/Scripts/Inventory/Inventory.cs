using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory logic
public class Inventory : SceneSingleton<Inventory> {

    // Skills
    Dictionary<string, int> skills = new Dictionary<string, int>();

    // Setup
    void Start() {
        skills.Add("Sprint", 0);
        skills.Add("Lightning", 1);
        skills.Add("Barrier", 2);
        skills.Add("Teleport", 3);
        skills.Add("Explosion", 4);
        skills.Add("Ice", 5);
        skills.Add("BlackHole", 6);
        skills.Add("HolyBeam", 7);
    }

    // Activate new skill
    public void SetSkill(string skill) {
        Player.Instance.skills.Add(skill);
        InventoryUI.Instance.UpdateUI(skills[skill]);
    }
}
