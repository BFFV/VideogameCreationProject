using UnityEngine;
using UnityEngine.UI;

// GUI Manager
public class GUIManager : SceneSingleton<GUIManager> {

    // References
    public GameObject hpNumber;  // TODO: replace with hp bar
    public GameObject mpNumber;  // TODO: replace with mp bar
    public GameObject expNumber;
    public GameObject gunIcon;
    public GameObject actionMessage;
    public GameObject eventMessage;
    public GameObject tutorialMessage;

    // Event Messages
    float eventTimeout = 0;

    // Setup
    void Start() {
        // Maybe add stuff later
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
        actionMessage.GetComponent<Text>().text = text;
        actionMessage.SetActive(active);
    }

    // Show event message
    public void ShowEvent(string text) {
        eventMessage.GetComponent<Text>().text = text;
        eventTimeout = 4;
        eventMessage.SetActive(true);
    }

    // Show/hide tutorial message
    public void ShowTutorial(string text, bool active) {
        tutorialMessage.GetComponent<Text>().text = text;
        tutorialMessage.SetActive(active);
    }

    // Update player HP
    public void UpdatePlayerHealth(int hp) {
        hpNumber.GetComponent<Text>().text = "HP: " + hp + "/" + 100;
    }

    // Update player EXP
    public void UpdatePlayerExp(int exp) {
        expNumber.GetComponent<Text>().text = "EXP: " + exp;
    }

     // Update player MP
    public void UpdatePlayerMagic(float mp) {
        mpNumber.GetComponent<Text>().text = "MP: " + (int) mp + "/" + 100;
    }

    // Update player status
    public void UpdatePlayerStatus(int hp, int exp, float mp) {
        UpdatePlayerHealth(hp);
        UpdatePlayerMagic(mp);
        UpdatePlayerExp(exp);
    }

    // TODO: Update gun icon (will be removed later)
    public void ToggleGunIcon(bool state) {
        gunIcon.SetActive(state);
    }
}
