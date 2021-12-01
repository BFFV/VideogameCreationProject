using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Spawner
public class EnemySpawner : MonoBehaviour {

    public GameObject[] enemies;
    int currentEnemies = 0;
    public int maxEnemies;
    public int minTime;
    public int maxTime;

    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Player") {
            InvokeRepeating("SpawnEnemy", 1, Random.Range(minTime, maxTime));
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Player") {
            CancelInvoke("SpawnEnemy");
        }
    }

    void SpawnEnemy() {
        if (currentEnemies >= maxEnemies) {
            return;
        }
        Vector3 spawnPosition = gameObject.transform.position;
        float radius = gameObject.GetComponent<CircleCollider2D>().radius;
        Vector3 position = Random.insideUnitSphere * radius;
        position.x += spawnPosition.x;
        position.y += spawnPosition.y;
        position.z = 0.0f;
        int enemyType = Random.Range(0, enemies.Length);
        GameObject enemy = Instantiate(enemies[enemyType], position, Quaternion.identity);
        enemy.GetComponent<Enemy>().spawner = gameObject.GetComponent<EnemySpawner>();
        currentEnemies++;
    }

    public void EnemyDestroyed() {
        if (currentEnemies > 0) {
            currentEnemies--;
        }
    }
}
