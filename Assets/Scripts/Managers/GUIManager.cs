using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// GUI Manager
public class GUIManager : SceneSingleton<GUIManager> {

    // References
    public Slider hpBar;
    public Slider mpBar;
    public GameObject expNumber;
    public GameObject eventMessage;
    public GameObject tutorialMessage;
    public Transform damagePopup;
    public Image weaponIcon;
    public List<Sprite> weaponIcons;

    // Weapon mapping to image
    Dictionary<string, Sprite> weapons = new Dictionary<string, Sprite>();

    // Event Messages
    float eventTimeout = 0;

    // Setup
    void Start() {
        weapons.Add("Sword", weaponIcons[0]);
        weapons.Add("Gun", weaponIcons[1]);
        weapons.Add("Wind", weaponIcons[2]);
        weapons.Add("Fire", weaponIcons[3]);
        ShowWeapon("Sword");
    }

    // Update
    void Update() {
        // Event messages
        if (eventTimeout > 0) {
            eventTimeout -= Time.deltaTime;
            if (eventTimeout <= 0) {
                eventTimeout = 0;
                eventMessage.SetActive(false);
            }
        }
    }

    // Show/hide action message
    public void ShowAction(string text, bool active) {
        eventMessage.GetComponent<Text>().text = text;
        eventMessage.SetActive(active);
    }

    // Show event message
    public void ShowEvent(string text) {
        eventMessage.GetComponent<Text>().text = text;
        eventTimeout = 5;
        eventMessage.SetActive(true);
    }

    // Show/hide tutorial message
    public void ShowTutorial(string text, bool active) {
        tutorialMessage.GetComponent<Text>().text = text;
        tutorialMessage.SetActive(active);
    }

    // Update player HP
    public void UpdatePlayerHealth(int hp) {
        hpBar.value = hp;
    }

    // Update player EXP
    public void UpdatePlayerExp(int exp) {
        expNumber.GetComponent<Text>().text = "EXP: " + exp;
    }

     // Update player MP
    public void UpdatePlayerMagic(float mp) {
        mpBar.value = mp;
    }

    // Update player status
    public void UpdatePlayerStatus(int hp, int exp, float mp) {
        UpdatePlayerHealth(hp);
        UpdatePlayerMagic(mp);
        UpdatePlayerExp(exp);
    }

    // Show current weapon
    public void ShowWeapon(string weapon) {
        weaponIcon.sprite = weapons[weapon];
    }
}
