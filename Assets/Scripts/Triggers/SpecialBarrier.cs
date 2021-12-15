using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Special barrier blocking the final boss
public class SpecialBarrier : MonoBehaviour {

    // References
    public GameObject barrier;
    string text;

    // Setup
    void Start() {
        text = "You over there!\nThat barrier up ahead will only open for those enveloped in both light and darkness...";
        PlayerData state;
        if (GameManager.Instance.warping) {
            state = GameManager.Instance.warpData;
        } else {
            state = GameManager.Instance.playerData;
        }
        List<string> skills = state.spawnSkills;
        if (!skills.Contains("BlackHole") && skills.Contains("HolyBeam")) {
            text = "I see that you are blessed with holy powers, but you still lack darkness...";
        } else if (skills.Contains("BlackHole") && !skills.Contains("HolyBeam")) {
            text = "I see that you are one with the dark, but you still need to see the light...";
        } else if (skills.Contains("BlackHole") && skills.Contains("HolyBeam")) {
            Destroy(gameObject);
            Destroy(barrier);
        }
    }

    // Player talks with final boss
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GUIManager.Instance.ShowTutorial(text, true);
        }
    }

    // Player stops talking
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GUIManager.Instance.ShowTutorial("", false);
        }
    }
}
