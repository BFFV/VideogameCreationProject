using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    // Start is called before the first frame update
    private Rigidbody2D myRigidbody;

    // Allow direction of the bullet
    public Vector2 direction;

    [SerializeField]
    private float speed;

    // Distance of the bullet
    [SerializeField]
    private int distance;
    void Start() {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        distance--;
        if (distance <= 0) {
            Destroy(gameObject);
        }
    }


    private void FixedUpdate() {
        myRigidbody.velocity = direction.normalized * speed;
    }
}
