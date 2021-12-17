using UnityEngine;

// Gun weapon
public class Bullet : MonoBehaviour {

    // References
    Rigidbody2D body;
    public Vector2 direction;
    float speed = 15;
    int distance = 100;
    int damage = 5;

    // Setup
    void Start() {
        body = GetComponent<Rigidbody2D>();
        AudioManager.Instance.PlaySound("gun");
    }

    // Check distance for the bullet
    void Update() {
        if (Time.timeScale == 0) {  // Game is paused
            return;
        }
        distance--;
        if (distance <= 0) {
            Destroy(gameObject);
        }
    }

    // Bullet movement
    void FixedUpdate() {
        body.velocity = direction.normalized * speed;
    }

    // Hit an enemy
    void OnCollisionEnter2D(Collision2D other) {
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
        // TODO: add other bosses
        Destroy(gameObject);
    }
}
