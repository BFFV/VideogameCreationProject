using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    // Settings
    bool playing = true;
    bool win = false;

    // Finish game
    public void EndGame(bool won) {
        if (playing) {
            win = won;
            playing = false;
            SceneManager.LoadScene("EndScene");
        }
    }

    // Start the level
    public void StartGame() {
        playing = true;
        SceneManager.LoadScene("DemoLevel");
    }
}
