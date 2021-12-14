using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Inventory UI
public class InventoryUI : SceneSingleton<InventoryUI> {

    // References
    public List<GameObject> skillSlots;
    public List<GameObject> weaponSlots;
    public GameObject inventoryUI;
    public GameObject[] windows;
    int windowIdx = 1;
    List<string> windowNames;
    public GameObject navA;
    public GameObject navB;
    public GameObject navC;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Setup
    void Start() {
        windowNames = new List<string>();
        windowNames.Add("Weapons");
        windowNames.Add("Skills");
        windowNames.Add("Controls");
        windowNames.Add("Options");
    }

    // Inventory navigation
    void Update() {
        // Toggle
        if (Input.GetButtonDown("Inventory")) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (inventoryUI.activeSelf) {  // Pause
                Time.timeScale = 0;
                Player.Instance.ResetState();
                AudioManager.Instance.PlaySound("pause", 0.5f);
                windowIdx = 1;
                UpdateNavigation();
                windows[1].SetActive(true);
                musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 1);
                sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 1);
            } else {  // Unpause
                Time.timeScale = 1;
                AudioManager.Instance.PlaySound("unpause");
                foreach (GameObject w in windows) {
                    w.SetActive(false);
                }
            }
        }

        // Navigate
        if (inventoryUI.activeSelf) {
            if (Input.GetKeyDown(KeyCode.A) && windowIdx > 0) {
                AudioManager.Instance.PlaySound("switchWindow");
                windows[windowIdx].SetActive(false);
                windowIdx -= 1;
                UpdateNavigation();
                windows[windowIdx].SetActive(true);
            } else if (Input.GetKeyDown(KeyCode.D) && windowIdx < 3) {
                AudioManager.Instance.PlaySound("switchWindow");
                windows[windowIdx].SetActive(false);
                windowIdx += 1;
                UpdateNavigation();
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

    // Update navigation text
    void UpdateNavigation() {
        navB.GetComponent<Text>().text = windowNames[windowIdx];
        if (windowIdx > 0) {
            navA.GetComponent<Text>().text = "<A>  " + windowNames[windowIdx - 1];
        } else {
            navA.GetComponent<Text>().text = "";
        }
        if (windowIdx < 3) {
            navC.GetComponent<Text>().text = windowNames[windowIdx + 1] + "  <D>";
        } else {
            navC.GetComponent<Text>().text = "";
        }
    }
}
