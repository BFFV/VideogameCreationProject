using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Teleport skill
public class Teleport : MonoBehaviour {

    // Setup
    public Vector2 direction;
    public float speed;
    public int distance;

    // Spawn
    void Start() {
        AudioManager.Instance.PlaySound("portal");
    }

    // Advance & rotate
    void Update() {
        distance--;
        if (distance <= 0) {
            Player.Instance.gateActive = false;
            Destroy(gameObject);
        }
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, - 270 * Time.deltaTime, Space.World);
    }

    // Warp player to portal
    public void Warp() {
        AudioManager.Instance.PlaySound("teleport");
        Player player = Player.Instance;
        SpriteRenderer sprite = player.gameObject.GetComponent<SpriteRenderer>();
        sprite.material.color = new Color(0, 2, 2.2f, 1);
        player.gameObject.transform.position = transform.position;
        player.OnWarp();
        Destroy(gameObject);
    }

    // Collide with invalid warp zone
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Solid" || tag == "Obstacle") {
            Player.Instance.gateActive = false;
            Destroy(gameObject);
        };
    }
}
