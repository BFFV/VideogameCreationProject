using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem {

    // WebGL file sync
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    // Save game state
    public static void SaveData(int slot, PlayerData data) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/slot" + slot + ".dat";
        if (File.Exists(path)) {
            File.Delete(path);
        }
        using (FileStream stream = new FileStream(path, FileMode.Create)) {
            formatter.Serialize(stream, data);
        }
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            SyncFiles();
        }
    }

    // Load game state
    public static PlayerData LoadData(int slot) {
        string path = Application.persistentDataPath + "/slot" + slot + ".dat";
        if (File.Exists(path)) {  // Saved state
            BinaryFormatter formatter = new BinaryFormatter();
            PlayerData data;
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                data = formatter.Deserialize(stream) as PlayerData;
            }
            return data;
        } else {  // New game state
            PlayerData newGame = new PlayerData("Tutorial", new Vector3(0, 0, 0), "new");
            SaveData(slot, newGame);
            return newGame;
        }
    }

    // Delete game state
    public static void DeleteData(int slot) {
        string path = Application.persistentDataPath + "/slot" + slot + ".dat";
        if (File.Exists(path)) {
            File.Delete(path);
        }
    }
}
