using UnityEngine;

// Fireball
public class Fireball : MonoBehaviour {

    // Setup
    public Rigidbody2D body;
    public Vector2 direction;
    float speed = 4;
    int distance = 300;
    int damage = 30;

    // Setup
    void Start() {
        body = GetComponent<Rigidbody2D>();
        AudioManager.Instance.PlaySound("fireball", 0.5f);
    }

    // Check distance for the bullet
    void Update() {
        distance--;
        if (distance <= 0) {
            Destroy(gameObject);
        }
    }

    // Fireball movement
    private void FixedUpdate() {
        body.velocity = direction.normalized * speed;
    }

    // Hit the player
    void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Player") {
            Player.Instance.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
