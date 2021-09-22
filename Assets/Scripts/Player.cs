using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Set speed of character
    [SerializeField]
    private float speed;

    // Set direction of move
    private Vector2 direction;

    public Rigidbody2D player;

    // Animation to attack
    private Animator animator;

    private bool attacking = false;

    // Stats
    //private int health;
    //private int experience;

    // Start is called before the first frame update
    void Start() {
    //   health = 3;
    //   experience = 0;
        direction = Vector2.zero;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update() {
        Move();
        HandleAttack();
    }

    public void Move() {
        // First get the key of move that the player press
        direction = Vector2.zero;
        // Kyeboard of move: WASD
        if (Input.GetKey(KeyCode.W)) {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S)) {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A)) {
            direction += Vector2.left;
        }
       if (Input.GetKey(KeyCode.D)) {
            direction += Vector2.right;
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void HandleAttack() {
        if (Input.GetKey(KeyCode.O) && !attacking) {
            StartCoroutine(Attack());
        } 
    }

    private IEnumerator Attack() {
        
            animator.SetBool("Attacking", true);
            attacking = true;

            yield return new WaitForSeconds(1);
            animator.SetBool("Attacking", false);
            attacking = false;

    }


}
