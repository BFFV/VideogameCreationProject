using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Exit button logic
public class Exit : MonoBehaviour {

    // Go back to menu
    public void exitLevel() {
        Time.timeScale = 1;
        GameManager.Instance.MainMenu();
    }
}
