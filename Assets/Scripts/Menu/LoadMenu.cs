using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : MonoBehaviour {

    // Current save slot
    public int slot;

    // Load current game slot
    public void LoadGame() {
        GameManager game = GameManager.Instance;
        game.slot = slot;
        game.playerData = SaveSystem.LoadData(slot);
        game.StartGame();
    }
}
