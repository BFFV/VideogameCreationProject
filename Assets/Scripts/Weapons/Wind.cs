using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wind weapon
public class Wind : MonoBehaviour {

    // Body
    Rigidbody2D body;
    public Vector2 direction;
    public float speed;
    public int distance;
    public int damage;

    // Setup
    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    // Check distance for the wind
    void Update() {
        distance--;
        if (distance <= 0) {
            Destroy(gameObject);
        }
    }

    // Wind movement
    private void FixedUpdate() {
        body.velocity = direction.normalized * speed;
    }

    // Hit an enemy
    void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        } else if (tag == "Boss1") {
            SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
            boss.TakeDamage(damage);
        } else if (tag == "Boss2") {
            AngelBoss boss = other.gameObject.GetComponent<AngelBoss>();
            boss.TakeDamage(damage);
        }
        // TODO: add other bosses
        Destroy(gameObject);
    }
}
