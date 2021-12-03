using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    // Body
    private Rigidbody2D body;

    // Direction of the shot
    public Vector2 direction;

    [SerializeField]
    private float speed;

    // Distance of the bullet
    [SerializeField]
    private int distance;

    // Damage
    public int damage;

    // Player
    private Player player;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        string tag = other.gameObject.tag;
        if (tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
