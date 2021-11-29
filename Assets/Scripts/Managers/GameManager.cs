using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    // Settings
    bool playing = true;
    bool win = false;

    // Checkpoints
    public Vector3 spawnPos = new Vector3(60, 60, 0);
    public int spawnExp = 100;
    public bool spawnGun = false;
    public List<string> spawnWeapons = new List<string>();
    public List<string> spawnSkills = new List<string>();

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

    // Player respawn after death
    public void Respawn() {
        SceneManager.LoadScene("DemoLevel");
    }

    // Checkpoints
    public void SaveCheckpoint(Vector3 pos, int exp, List<string> weapons, List<string> skills) {
        spawnPos = pos;
        spawnExp = exp;
        spawnWeapons = weapons;
        spawnSkills = skills;
    }
}
