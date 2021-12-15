using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wind weapon
public class Wind : MonoBehaviour {

    // Body
    Rigidbody2D body;
    public Vector2 direction;
    public float speed;
    public int distance;
    public int damage;

    float impulse = 20f;


    // Setup
    void Start() {
        body = GetComponent<Rigidbody2D>();
        AudioManager.Instance.StartLoop("wind");

    }

    // Check distance for the wind
    void Update() {
        distance--;
        if (distance <= 0) {
            AudioManager.Instance.StopLoop("wind");
            Destroy(gameObject);
            Player.Instance.wind_attacking = false;
        }
    }

    // Wind movement
    private void FixedUpdate() {
        System.Random random = new System.Random();
        Vector2 vr;
        int sum_or_subs_x = random.Next(0,2);
        int sum_or_subs_y = random.Next(0,2);
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

        //body.velocity = direction.normalized * speed;
    }

    // Hit an enemy
    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;
        if (tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.body.AddForce(direction * impulse, ForceMode2D.Impulse);

        } else if (tag == "Boss1") {
            SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
            boss.TakeDamage(damage);
            boss.body.AddForce(direction * impulse, ForceMode2D.Impulse);

        }
    }
}
