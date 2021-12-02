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
    int damage = 10;
    bool active = true;

    // Start skill
    void Start() {
        StartCoroutine(Cast(15, 1));
        StartCoroutine(Storm());
    }

    // Cast skill
    IEnumerator Cast(float sValue, float sTime) {
        float x = transform.localScale.x;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / sTime) {
            transform.localScale = new Vector3(Mathf.Lerp(x, sValue, t), Mathf.Lerp(x, sValue, t), 0);
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
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            if (active) {
                StartCoroutine(Cast(0, 1));
                timeout = 2;
                active = false;
            } else {
                StopCoroutine(Storm());
                Destroy(gameObject);
            }
        }
    }

    // Enemy enters strike zone
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (!other.isTrigger && tag == "Enemy") {
            targets.Add(other.gameObject);
        }
    }

    // Enemy exits strike zone
    void OnTriggerExit2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (!other.isTrigger && tag == "Enemy") {
            targets.Remove(other.gameObject);
        }
    }

    // Lightning strike
    void Strike() {
        for (int i = targets.Count - 1; i >= 0; i--) {
            GameObject t = targets[i];
            Vector3 pos = t.transform.position;
            Instantiate(strikes[Random.Range(0, 3)], pos, Quaternion.identity);
            Enemy enemy = t.GetComponent<Enemy>();
            if (enemy.hp <= damage) {
                targets.RemoveAt(i);
            }
            enemy.TakeDamage(damage, forced: true);
        }
    }
}
