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
            spawnExp = 3000;
            spawnWeapons = new List<string>() {"Sword"};
            spawnSkills = new List<string>() {};
        } else if (state == "new") {  // New game
            spawnExp = 1;
            spawnWeapons = new List<string> {"Sword"};
            spawnSkills = new List<string>();
        } else if (state == "saved") {  // Checkpoint
            Player player = Player.Instance;
            spawnExp = player.exp;
            spawnWeapons = new List<string>(player.weapons);
            spawnSkills = new List<string>(player.skills);
        }
    }
}
