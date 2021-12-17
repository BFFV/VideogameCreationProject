using UnityEngine;
using UnityEngine.SceneManagement;

// Game Manager
public class GameManager : Singleton<GameManager> {

    // Warping
    public bool warping = false;
    public PlayerData warpData = new PlayerData("Shrine", new Vector3(0, 0, 0), "default");
    public int warpHp = 0;
    public float warpMp = 0;

    // Checkpoints & save data
    public int slot = 0;
    public PlayerData playerData = new PlayerData("Shrine", new Vector3(0, 0, 0), "default");

    // Settings
    void Start() {
        Application.targetFrameRate = 60;
    }

    // Start level or respawn
    public void StartGame() {
        warping = false;
        SceneManager.LoadScene(playerData.spawnLevel);
    }

    // Warp to new level
    public void Warp(string level, Vector3 pos) {
        warping = true;
        warpData = new PlayerData(level, pos, "saved");
        warpHp = Player.Instance.hp;
        warpMp = Player.Instance.mp;
        SceneManager.LoadScene(level);
    }

    // Save checkpoint data
    public void SaveCheckpoint(string level, Vector3 pos) {
        playerData = new PlayerData(level, pos, "saved");
        SaveSystem.SaveData(slot, playerData);
    }

    // Back to main menu
    public void MainMenu() {
        SceneManager.LoadScene("Menu");
    }

    // Show opening
    public void Opening() {
        SceneManager.LoadScene("Opening");
    }

    // Show ending
    public void Ending() {
        SceneManager.LoadScene("Ending");
    }
}
