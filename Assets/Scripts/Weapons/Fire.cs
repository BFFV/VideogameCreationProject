using System.Collections;
using UnityEngine;

// Fire weapon
public class Fire : MonoBehaviour {

    // Setup
    int duration = 300;
    int damage = 20;

    // Setup
    void Start() {
        StartCoroutine(Cast(0.5f, 0.5f));
        AudioManager.Instance.PlaySound("fire");
        AudioManager.Instance.PlaySound("burning");
    }

    // Cast fire
    IEnumerator Cast(float sValue, float sTime) {
        float x = transform.localScale.x;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / sTime) {
            transform.localScale = new Vector3(Mathf.Lerp(x, sValue, t), Mathf.Lerp(x, sValue, t), 1);
            yield return null;
        }
    }

    // Weapon activity
    void Update() {
        if (Time.timeScale == 0) {  // Game is paused
            return;
        }
        duration--;
        if (duration <= 0) {
            Player.Instance.fireAttacking = false;
            Destroy(gameObject);
        }
    }

    // Burn enemies
    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        // TODO: add other bosses
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
    }
}
