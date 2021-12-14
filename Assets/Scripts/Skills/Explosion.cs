using System.Collections.Generic;
using UnityEngine;

// Explosion
public class Explosion : MonoBehaviour {

    // Setup
    int damage = 30;
    public List<string> targetTag;
    float timeout = 0.5f;
    float impulse = 100f;

    // Explode
    void Start() {
        AudioManager.Instance.PlaySound("explosion");
    }

    // Assign to enemy
    public void AssignToEnemy() {
        targetTag = new List<string> {"Player"};
    }

    // Skill activity
    void Update() {
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            if (!targetTag.Contains("Player")) {
                Player.Instance.blast = false;
            }
            Destroy(gameObject);
        }
    }

    // Blast enemies
    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;
        Vector2 direction = (other.gameObject.transform.position - transform.position).normalized;
        // TODO: add other bosses
        if (tag == "Enemy" && targetTag.Contains("Enemy")) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.body.AddForce(direction * impulse, ForceMode2D.Impulse);
        } else if (tag == "Boss1" && targetTag.Contains("Boss1")) {
            SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
            boss.TakeDamage(damage);
            boss.body.AddForce(direction * impulse, ForceMode2D.Impulse);
        } else if (tag == "Player" && targetTag.Contains("Player")) {
            Player.Instance.TakeDamage(damage);
            Player.Instance.body.AddForce(direction * impulse, ForceMode2D.Impulse);
        }
    }
}
