using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Fireball
public class Fireball : MonoBehaviour {

    // References
    Rigidbody2D body;
    public Vector2 direction;
    public float speed;
    public int distance;
    public int damage;

    // Setup
    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    // Check distance for the bullet
    void Update() {
        distance--;
        if (distance <= 0) {
            Destroy(gameObject);
        }
    }

    // Bullet movement
    private void FixedUpdate() {
        body.velocity = direction.normalized * speed;
    }

    // Hit the player
    void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Player") {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
