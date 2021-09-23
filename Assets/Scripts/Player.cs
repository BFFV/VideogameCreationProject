using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Rigidbody2D player;

    public float speed;

    public int health;

    void Start() {
    }

    void Update() {
        player.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
        player.velocity.Normalize();
    }

    void OnCollisionEnter2D(Collision2D other) { // other es el otro objeto con el que colisiona
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Flying_enemy") {
            Destroy(other.gameObject);
            health -= 1;
            if (health == 0) {
                Destroy(gameObject);
            }
        }
    }
}
