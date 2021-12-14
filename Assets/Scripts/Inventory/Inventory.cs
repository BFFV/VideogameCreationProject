using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory logic
public class Inventory : SceneSingleton<Inventory> {

    // Skills
    Dictionary<string, int> skills = new Dictionary<string, int>();
    Dictionary<string, int> skillsMp = new Dictionary<string, int>();

    // Weapons
    Dictionary<string, int> weapons = new Dictionary<string, int>();

    // Setup
    void Start() {
        // Skills
        skills.Add("Sprint", 0);
        skills.Add("Lightning", 1);
        skills.Add("Barrier", 2);
        skills.Add("Teleport", 3);
        skills.Add("Explosion", 4);
        skills.Add("Ice", 5);
        skills.Add("BlackHole", 6);
        skills.Add("HolyBeam", 7);

        //Skills MP
        skillsMp.Add("Sprint", 1);
        skillsMp.Add("Lightning", 10);
        skillsMp.Add("Barrier", 1);
        skillsMp.Add("Teleport", 10);
        skillsMp.Add("Explosion", 10);
        skillsMp.Add("Ice", 10);
        skillsMp.Add("BlackHole", 100);
        skillsMp.Add("HolyBeam", 100);

        // Weapons
        skills.Add("Sword", 0);
        skills.Add("Gun", 1);
        skills.Add("Wind", 2);
        skills.Add("Fire", 3);
    }

    // Activate new skill
    public void SetSkill(string skill) {
        AudioManager.Instance.PlaySound("unlock");
        Player.Instance.skills.Add(skill);
        InventoryUI.Instance.UpdateSkill(skills[skill]);
    }

    // Activate new weapon
    public void SetWeapon(string weapon) {
        AudioManager.Instance.PlaySound("unlock");
        Player.Instance.weapons.Add(weapon);
        InventoryUI.Instance.UpdateWeapon(weapons[weapon]);
    }

    // Get skill MP cost
    public int SkillCost(string skill) {
        return skillsMp[skill];
    }
}
