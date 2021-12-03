using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// Inventory UI
public class InventoryUI : SceneSingleton<InventoryUI> {

    // References
    public List<GameObject> skillSlots;
    public GameObject inventoryUI;

    // Toggle inventory
    void Update() {
        if (Input.GetButtonDown("Inventory")) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    // Update skill slot
    public void UpdateUI(int skillID) {
        SkillSlot skill = skillSlots[skillID].GetComponent<SkillSlot>();
        skill.ActivateSkill();
    }
}
