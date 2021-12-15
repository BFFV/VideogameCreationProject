using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fire weapon
public class Fire : MonoBehaviour
{

    public Rigidbody2D body;
    public int duration = 60;
    public int damage = 25;
    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
        AudioManager.Instance.StartLoop("fire");
    }

    // Update is called once per frame
    void Update() {
        duration--;
        if (duration == 0) {
            Player.Instance.fire_attacking = false;
            AudioManager.Instance.StopLoop();
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;
        if (tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);

        } else if (tag == "Boss1") {
            SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
            boss.TakeDamage(damage);

        }
    }
}
