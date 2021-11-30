using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour {

    void Start() {
        string weaponType = gameObject.tag;
        if (GameManager.Instance.playerData.spawnWeapons.Contains(weaponType)) {
            Destroy(gameObject);
        }
    }
}
