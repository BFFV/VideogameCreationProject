using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// Inventory UI
public class InventoryUI : SceneSingleton<InventoryUI> {

    // References
    public List<GameObject> skillSlots;
    public List<GameObject> weaponSlots;
    public GameObject inventoryUI;
    public GameObject[] windows;
    int windowIdx = 1;

    // Inventory navigation
    void Update() {
        // Toggle
        if (Input.GetButtonDown("Inventory")) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf) {  // Pause
                Time.timeScale = 0;
                windows[1].SetActive(true);
                windowIdx = 1;
                AudioManager.Instance.PlaySound("pause", 0.5f);
            } else {  // Unpause
                Time.timeScale = 1;
                AudioManager.Instance.PlaySound("unpause");
                foreach (GameObject w in windows) {
                    w.SetActive(false);
                }
            }
        }

        // Navigate TODO: add visual indicator to use <A> and <D> for navigation
        if (inventoryUI.activeSelf) {
            if (Input.GetKeyDown(KeyCode.A) && windowIdx > 0) {
                AudioManager.Instance.PlaySound("switchWindow");
                windows[windowIdx].SetActive(false);
                windowIdx -= 1;
                windows[windowIdx].SetActive(true);
            } else if (Input.GetKeyDown(KeyCode.D) && windowIdx < 3) {
                AudioManager.Instance.PlaySound("switchWindow");
                windows[windowIdx].SetActive(false);
                windowIdx += 1;
                windows[windowIdx].SetActive(true);
            }
        }
    }

    // Update skill slot
    public void UpdateSkill(int skillID) {
        SkillSlot skill = skillSlots[skillID].GetComponent<SkillSlot>();
        skill.ActivateSkill();
    }

    // Update weapon slot
    public void UpdateWeapon(int weaponID) {
        WeaponSlot weapon = weaponSlots[weaponID].GetComponent<WeaponSlot>();
        weapon.ActivateWeapon();
    }
}
