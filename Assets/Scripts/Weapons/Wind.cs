using System.Collections;
using UnityEngine;

// Tornado weapon
public class Wind : MonoBehaviour {

    // Setup
    public Vector2 direction;
    float speed = 8;
    int distance = 900;
    int damage = 15;
    float impulse = 100f;

    // Setup
    void Start() {
        StartCoroutine(Cast(8, 0.5f));
        AudioManager.Instance.StartLoop("wind");
    }

    // Cast tornado
    IEnumerator Cast(float sValue, float sTime) {
        float x = transform.localScale.x;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / sTime) {
            transform.localScale = new Vector3(Mathf.Lerp(x, sValue, t), Mathf.Lerp(x, sValue, t), 1);
            yield return null;
        }
    }

    // Check distance for the wind
    void Update() {
        if (Time.timeScale == 0) {  // Game is paused
            return;
        }
        distance--;
        if (distance <= 0) {
            AudioManager.Instance.StopLoop("wind");
            Player.Instance.windAttacking = false;
            Destroy(gameObject);
        }
    }

    // Wind movement
    void FixedUpdate() {
        System.Random random = new System.Random();
        Vector2 vr;
        int sum_or_subs_x = random.Next(0, 2);
        int sum_or_subs_y = random.Next(0, 2);
        vr = new Vector2((float) random.NextDouble(), (float) random.NextDouble());
        if (sum_or_subs_x == 1) {
            direction.x = direction.x + vr.x;
        } else {
            direction.x = direction.x - vr.x;
        }
        if (sum_or_subs_y == 1) {
            direction.y = direction.y + vr.y;
        } else {
            direction.y = direction.y - vr.y;
        }
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    // Hit an enemy
    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        // TODO: add other bosses
        string tag = other.gameObject.tag;
        if (tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.body.AddForce(direction * impulse, ForceMode2D.Impulse);
        } else if (tag == "Boss1") {
            SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
            boss.TakeDamage(damage);
            boss.body.AddForce(direction * impulse, ForceMode2D.Impulse);
        } else if (tag == "Boss2") {
            AngelBoss boss = other.gameObject.GetComponent<AngelBoss>();
            boss.TakeDamage(damage);
            boss.body.AddForce(direction * impulse, ForceMode2D.Impulse);
        }
    }
}
