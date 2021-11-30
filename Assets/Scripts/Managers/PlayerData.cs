using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    // Player information
    public string spawnLevel;
    public float[] spawnPos;
    public int spawnExp;
    public List<string> spawnWeapons;
    public List<string> spawnSkills;

    // Store player state
    public PlayerData(string level, Vector3 pos, string state) {
        spawnLevel = level;
        spawnPos = new float[3];
        spawnPos[0] = pos.x;
        spawnPos[1] = pos.y;
        spawnPos[2] = 0;
        if (state == "default") {  // For testing
            spawnExp = 100;
            spawnWeapons = new List<string>();
            spawnSkills = new List<string>();
            return;
        } else if (state == "new") {  // New game
            spawnExp = 0;
            spawnWeapons = new List<string>();
            spawnSkills = new List<string>();
            return;
        }
        Player player = Player.Instance;
        spawnExp = player.exp;
        spawnWeapons = player.weapons;
        spawnSkills = player.skills;
    }
}
