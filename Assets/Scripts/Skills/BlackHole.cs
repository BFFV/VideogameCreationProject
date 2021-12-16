using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Black Hole skill
public class BlackHole : MonoBehaviour {

    // Setup
    float timeout = 5;
    int damage = 20;
    bool active = true;
    public List<string> targetTag;
    float force = 10000f;
    public Collider2D singularity;

    // Start skill
    void Start() {
        AudioManager.Instance.PlaySound("blackHole", 3f);
        StartCoroutine(Cast(2.5f, 2));
    }

    // Assign to enemy
    public void AssignToEnemy() {
        targetTag = new List<string> {"Player"};
    }

    // Cast skill
    IEnumerator Cast(float sValue, float sTime) {
        float x = transform.localScale.x;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / sTime) {
            transform.localScale = new Vector3(Mathf.Lerp(x, sValue, t), Mathf.Lerp(x, sValue, t), 1);
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
                if (!targetTag.Contains("Player")) {
                    Player.Instance.vortex = false;
                }
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
        } else if (tag == "Player" && targetTag.Contains("Player")) {
            Player.Instance.body.AddForce(direction * force, ForceMode2D.Force);
        } else if (tag == "Fireball" && targetTag.Contains("Fireball")) {
            Fireball fireball = other.gameObject.GetComponent<Fireball>();
            fireball.body.AddForce(direction * force, ForceMode2D.Force);
        } else if (tag == "Boss2" && targetTag.Contains("Boss2")) {
            AngelBoss boss = other.gameObject.GetComponent<AngelBoss>();
            boss.body.AddForce(direction * force, ForceMode2D.Force);
        }

        // Singularity
        // TODO: add other bosses/attacks
        if (singularity.IsTouching(other)) {
            if (tag == "Enemy" && targetTag.Contains("Enemy")) {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
            } else if (tag == "Player" && targetTag.Contains("Player")) {
                Player.Instance.TakeDamage(damage);
            } else if (tag == "Fireball" && targetTag.Contains("Fireball")) {
                Destroy(other.gameObject);
            } else if (tag == "Boss2" && targetTag.Contains("Boss2")) {
                AngelBoss boss = other.gameObject.GetComponent<AngelBoss>();
                boss.TakeDamage(damage);
            }
        }
    }
}
