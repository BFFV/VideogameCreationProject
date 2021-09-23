using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Set speed of character
    [SerializeField]
    private float speed;

    // Set direction of movement
    private Vector2 direction;

    // Player body
    public Rigidbody2D player;

    // Animation to attack
    private Animator animator;

    private bool attacking = false;

    // Stats
    //private int health;
    //private int experience;

    void Start() {
        //health = 3;
        //experience = 0;
        direction = Vector2.zero;
        animator = GetComponent<Animator>();
    }

    void Update() {
        Move();
        HandleAttack();
    }

    // Movement
    public void Move() {
        direction = Vector2.zero;

        // Check input
        if (Input.GetAxisRaw("Vertical") == 1) {
            direction += Vector2.up;
        }
        if (Input.GetAxisRaw("Vertical") == -1) {
            direction += Vector2.down;
        }
        if (Input.GetAxisRaw("Horizontal") == -1) {
            direction += Vector2.left;
        }
        if (Input.GetAxisRaw("Horizontal") == 1) {
            direction += Vector2.right;
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Start the attack
    public void HandleAttack() {
        if (Input.GetKey(KeyCode.O) && !attacking) {
            StartCoroutine(Attack());
        }
    }

    // Attack
    private IEnumerator Attack() {
            animator.SetBool("Attacking", true);
            attacking = true;
            yield return new WaitForSeconds(1);
            animator.SetBool("Attacking", false);
            attacking = false;
    }
}
