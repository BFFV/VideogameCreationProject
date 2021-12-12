using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ice Shot skill
public class IceShot: MonoBehaviour {

    // Setup
    public Vector2 direction;
    public float speed;
    public int distance;
    public List<string> targetTag;

    // Spawn
    void Start() {
        AudioManager.Instance.PlaySound("ice");
    }

    // Assign to enemy
    public void AssignToEnemy() {
        targetTag = new List<string> {"Player"};
    }

    // Advance
    void Update() {
        distance--;
        if (distance <= 0) {
            Destroy(gameObject);
        }
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    // Freeze enemies
    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;
        // TODO: add other bosses
        if (tag == "Enemy" && targetTag.Contains("Enemy")) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.Freeze();
        } else if (tag == "Boss1" && targetTag.Contains("Boss1")) {
            SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
            boss.Freeze();
        } else if (tag == "Player" && targetTag.Contains("Player")) {
            Player.Instance.Freeze();
        }
    }
}
