using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sword : MonoBehaviour {
    // Start is called before the first frame update
    private Player player;
    private Rigidbody2D body;

    public int damage;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
