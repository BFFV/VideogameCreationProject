using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Explosion
public class Explosion : MonoBehaviour {

    // Setup
    public int damage;
    public List<string> targetTag;
    float timeout = 0.5f;
    float impulse = 1000f;

    // Explode
    void Start() {
        AudioManager.Instance.PlaySound("explosion");
    }

    // Skill activity
    void Update() {
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            Destroy(gameObject);
            }
    }

    // Blast enemies
    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;
        // TODO: add other bosses
        if (tag == "Enemy" && targetTag.Contains("Enemy")) {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                Vector2 direction = (other.gameObject.transform.position - transform.position).normalized;
                enemy.body.AddForce(direction * impulse, ForceMode2D.Impulse);
                enemy.TakeDamage(damage);
            } else if (tag == "Boss1" && targetTag.Contains("Boss1")) {
                SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
                boss.TakeDamage(damage);
            } else if (tag == "Player" && targetTag.Contains("Player")) {
                Player.Instance.TakeDamage(damage);
            }
    }
}
