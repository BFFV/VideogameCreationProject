using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Spawner
public class EnemySpawner : MonoBehaviour {

    public GameObject[] enemies;

    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Player") {
            InvokeRepeating("SpawnEnemy", Random.Range(3, 10), Random.Range(3, 10));
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        CancelInvoke("SpawnEnemy");
    }

    void SpawnEnemy() {
        Vector3 spawnPosition = gameObject.transform.position;
        float radius = gameObject.GetComponent<CircleCollider2D>().radius;
        Vector3 position = Random.insideUnitSphere * radius;
        position.x += spawnPosition.x;
        position.y += spawnPosition.y;
        position.z = 0.0f;
        int enemyType = Random.Range(0, enemies.Length);
        Instantiate(enemies[enemyType], position, Quaternion.identity);
    }
}
