using UnityEngine;

// Sword weapon
public class Sword : MonoBehaviour {

    // References
    Rigidbody2D body;
    int damage = 10;
    float impulse = 200f;

    // Setup
    void Start() {
        body = GetComponent<Rigidbody2D>();
        AudioManager.Instance.PlaySound("sword");
    }

    // Hit an enemy
    void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;
        Vector2 direction = (other.gameObject.transform.position - Player.Instance.transform.position).normalized;
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
        } else if (tag == "Boss3") {
            FinalBoss boss = other.gameObject.GetComponent<FinalBoss>();
            boss.TakeDamage(damage);
            boss.body.AddForce(direction * impulse, ForceMode2D.Impulse);
        }
    }
}
