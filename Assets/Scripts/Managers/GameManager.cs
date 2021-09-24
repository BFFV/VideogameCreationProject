using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    // Settings
    bool playing = true;

    // Finish game
    public void EndGame() {
        if (playing) {
            playing = false;
            SceneManager.LoadScene("Demo2");
        }
    }

    // Start the level
    public void StartGame() {
        playing = true;
        SceneManager.LoadScene("DemoLevel");
    }
}
