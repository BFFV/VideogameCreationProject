using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    // Settings (not in use)
    bool playing = true;
    bool win = false;

    // Checkpoints & save data
    public int slot = 0;
    public PlayerData playerData = new PlayerData("DemoLevel", new Vector3(0, 0, 0), "default");

    // Finish game (not in use)
    public void EndGame(bool won) {
        if (playing) {
            win = won;
            playing = false;
            SceneManager.LoadScene("EndScene");
        }
    }

    // Start level or respawn
    public void StartGame() {
        SceneManager.LoadScene(playerData.spawnLevel);
    }

    // Save checkpoint data
    public void SaveCheckpoint(string level, Vector3 pos) {
        playerData = new PlayerData(level, pos, "saved");
        SaveSystem.SaveData(slot, playerData);
    }
}
