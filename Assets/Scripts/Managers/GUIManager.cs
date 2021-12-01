using UnityEngine;
using UnityEngine.UI;

// GUI Manager
public class GUIManager : SceneSingleton<GUIManager> {

    // References
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
}
