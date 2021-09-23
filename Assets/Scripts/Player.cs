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
    public Rigidbody2D body;

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
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        HandleAttack();
    }

    void FixedUpdate() {
        Move();
    }

    // Player Movement
    public void Move() {
        // Get input
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction.Normalize();

        // Move body
        body.MovePosition(new Vector2(transform.position.x + direction.x * speed * Time.deltaTime,
                transform.position.y + direction.y * speed * Time.deltaTime));
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
