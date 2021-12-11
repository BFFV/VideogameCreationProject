using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Black Hole skill
public class BlackHole : MonoBehaviour {

    // Setup
    float timeout = 5;
    int damage = 10;
    bool active = true;
    public List<string> targetTag;
    float force = 800f;
    public Collider2D singularity;

    // Start skill
    void Start() {
        StartCoroutine(Cast(2.5f, 2));
    }

    // Cast skill
    IEnumerator Cast(float sValue, float sTime) {
        float x = transform.localScale.x;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / sTime) {
            transform.localScale = new Vector3(Mathf.Lerp(x, sValue, t), Mathf.Lerp(x, sValue, t), 0);
            yield return null;
        }
    }

    // Skill activity
    void Update() {
        transform.Rotate(0, 0, 45 * Time.deltaTime, Space.World);
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            if (active) {
                StartCoroutine(Cast(0, 1));
                timeout = 2;
                active = false;
            } else {
                Player.Instance.vortex = false;
                Destroy(gameObject);
            }
        }
    }

    // Enemy enters black hole
    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;

        // Gravity
        Vector2 direction = (transform.position - other.gameObject.transform.position).normalized;
        // TODO: add other bosses/attacks
        if (tag == "Enemy" && targetTag.Contains("Enemy")) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.body.AddForce(direction * force, ForceMode2D.Force);
        } else if (tag == "Boss1" && targetTag.Contains("Boss1")) {
            SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
            boss.body.AddForce(direction * force, ForceMode2D.Force);
        } else if (tag == "Player" && targetTag.Contains("Player")) {
            Player.Instance.body.AddForce(direction * force, ForceMode2D.Force);
        } else if (tag == "Fireball" && targetTag.Contains("Fireball")) {
            Fireball fireball = other.gameObject.GetComponent<Fireball>();
            fireball.body.AddForce(direction * force, ForceMode2D.Force);
        }

        // Singularity
        // TODO: add other bosses/attacks
        if (singularity.IsTouching(other)) {
            if (tag == "Enemy" && targetTag.Contains("Enemy")) {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
            } else if (tag == "Boss1" && targetTag.Contains("Boss1")) {
                SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
                boss.TakeDamage(damage);
            } else if (tag == "Player" && targetTag.Contains("Player")) {
                Player.Instance.TakeDamage(damage);
            } else if (tag == "Fireball" && targetTag.Contains("Fireball")) {
                Destroy(other.gameObject);
            }
        }
    }
}