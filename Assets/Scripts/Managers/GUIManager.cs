using UnityEngine;
using UnityEngine.UI;

// GUI Manager
public class GUIManager : SceneSingleton<GUIManager> {

    // References
    public GameObject saveMessage;
    public GameObject eventMessage;

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

    // Toggle save game message
    public void ToggleSave(bool active) {
        saveMessage.SetActive(active);
    }

    // Show event message
    public void ShowEvent(string text) {
        eventMessage.GetComponent<Text>().text = text;
        eventTimeout = 4;
        eventMessage.SetActive(true);
    }
}
