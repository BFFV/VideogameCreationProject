using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        string[] enemies = {"Enemy", "Flying_enemy", "Boss"};
        string tag = other.gameObject.tag;
        if (enemies.Contains(tag)) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            int expGained = enemy.TakeDamage(damage);
            player.GainExperience(expGained);
        }
        Destroy(gameObject);
    }
}
