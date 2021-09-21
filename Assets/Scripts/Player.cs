using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Rigidbody2D player;

    public float speed;

    void Start() {
    }

    void Update() {
        player.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
        player.velocity.Normalize();
    }
}
