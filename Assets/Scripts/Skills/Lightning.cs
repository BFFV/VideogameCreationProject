using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lightning skill
public class Lightning : MonoBehaviour {

    // Setup
    List<GameObject> targets = new List<GameObject>();
    public float minTime;
    public float maxTime;
    float timeout = 5;
    public List<GameObject> strikes;
    int damage = 15;
    bool active = true;
    public List<string> targetTag;

    // Start skill
    void Start() {
        AudioManager.Instance.PlaySound("storm", 2f);
        StartCoroutine(Cast(3.5f, 1));
        StartCoroutine(Storm());
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

    // Randomized strikes
    IEnumerator Storm() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime + 1));
            Strike();
        }
    }

    // Skill activity
    void Update() {
        transform.Rotate(0, 0, 60 * Time.deltaTime, Space.World);
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            if (active) {
                StartCoroutine(Cast(0, 1));
                timeout = 2;
                active = false;
            } else {
                StopCoroutine(Storm());
                if (!targetTag.Contains("Player")) {
                    Player.Instance.storm = false;
                }
                Destroy(gameObject);
            }
        }
    }

    // Enemy enters strike zone
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (!other.isTrigger && targetTag.Contains(tag)) {
            targets.Add(other.gameObject);
        }
    }

    // Enemy exits strike zone
    void OnTriggerExit2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (!other.isTrigger && targetTag.Contains(tag)) {
            targets.Remove(other.gameObject);
        }
    }

    // Lightning strike
    void Strike() {
        for (int i = targets.Count - 1; i >= 0; i--) {
            GameObject t = targets[i];
            Vector3 pos = t.transform.position;
            Instantiate(strikes[Random.Range(0, 3)], pos, Quaternion.identity);
            // TODO: add other bosses
            if (t.CompareTag("Enemy") && targetTag.Contains("Enemy")) {
                Enemy enemy = t.GetComponent<Enemy>();
                if (!enemy.isRecovering && enemy.hp <= damage) {
                    targets.RemoveAt(i);
                }
                enemy.TakeDamage(damage);
            } else if (t.CompareTag("Boss1") && targetTag.Contains("Boss1")) {
                SkeletonBoss boss = t.GetComponent<SkeletonBoss>();
                if (!boss.isRecovering && boss.hp <= damage) {
                    targets.RemoveAt(i);
                }
                boss.TakeDamage(damage);
            } else if (t.CompareTag("Boss2") && targetTag.Contains("Boss2")) {
                AngelBoss boss = t.GetComponent<AngelBoss>();
                if ((!boss.isRecovering && !boss.invincible) && boss.hp <= damage) {
                    targets.RemoveAt(i);
                }
                boss.TakeDamage(damage);
            } else if (t.CompareTag("Player") && targetTag.Contains("Player")) {
                if (!Player.Instance.isRecovering && Player.Instance.hp <= damage) {
                    targets.RemoveAt(i);
                }
                Player.Instance.TakeDamage(damage);
            }
        }
    }
}
