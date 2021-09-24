using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // Body
    private Rigidbody2D body;

    // Direction of the shot
    public Vector2 direction;

    [SerializeField]
    private float speed;

    // Distance of the bullet
    [SerializeField]
    private int distance;

    void Start() {
        body = GetComponent<Rigidbody2D>();

    }

    void Update() {
        distance--;
        if (distance <= 0) {
            Destroy(gameObject);
        }
    }


    private void FixedUpdate() {
        body.velocity = direction.normalized * speed;
    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Flying_enemy" || other.gameObject.tag == "Enemy") {
            Enemy enemy = other.collider.GetComponentInParent<Enemy>();
            // Bala impacto en un enemigo
            Debug.Log("Colision con enemigo");
            
        }
        Destroy(gameObject);
    }
}
