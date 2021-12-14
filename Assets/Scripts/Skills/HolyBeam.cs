using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holy Beam skill
public class HolyBeam : MonoBehaviour {

    // Setup
    float timeout = 5;
    int damage = 40;
    bool active = true;
    public List<string> targetTag;

    // Start skill
    void Start() {
        AudioManager.Instance.PlaySound("holyBeam");
        StartCoroutine(Cast(2.5f, 6, 0.5f));
    }

    // Assign to enemy
    public void AssignToEnemy() {
        targetTag = new List<string> {"Player"};
    }

    // Cast skill
    IEnumerator Cast(float xValue, float yValue, float sTime) {
        yield return new WaitForSeconds(1);
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / sTime) {
            transform.localScale = new Vector3(Mathf.Lerp(x, xValue, t), Mathf.Lerp(y, yValue, t), 1);
            yield return null;
        }
    }

    // Skill activity
    void Update() {
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            if (active) {
                StartCoroutine(Cast(2.5f, 0, 1));
                timeout = 2;
                active = false;
            } else {
                if (!targetTag.Contains("Player")) {
                    Player.Instance.holy = false;
                }
                Destroy(gameObject);
            }
        }
    }

    // Enemy is inside beam
    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;

        // Light damage
        // TODO: add other bosses/attacks
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
