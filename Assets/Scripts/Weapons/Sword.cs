using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Sword weapon
public class Sword : MonoBehaviour {
    // References
    Rigidbody2D body;
    public int damage;

    // Setup
    void Start() {
        body = GetComponent<Rigidbody2D>();
        AudioManager.Instance.PlaySound("sword");
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
        }
        // TODO: add other bosses
    }
}
