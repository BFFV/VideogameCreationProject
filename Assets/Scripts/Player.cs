using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Set speed of character
    [SerializeField]
    private float speed;

    // Set direction of movement
    private Vector2 direction;
    private Vector2 last_direction;

    // Player body
    public Rigidbody2D body;

    // Animation to attack
    private Animator animator;

    // Bullets
    [SerializeField]
    private GameObject[] projectiles;

    private bool attacking = false;

    // Stats
    //private int health;
    //private int experience;

    // Shoot settings
    private bool hasGun;
    private int nBullets;

    void Start() {
        //health = 3;
        //experience = 0;
        direction = Vector2.zero;
        last_direction = new Vector2(1,0);
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // TODO: Get gun
        hasGun = true;
        nBullets = 10;
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
        if (direction.x != 0 || direction.y != 0){
            last_direction = direction;
        }

        // Move body
        body.MovePosition(new Vector2(transform.position.x + direction.x * speed * Time.deltaTime,
                transform.position.y + direction.y * speed * Time.deltaTime));
    }

    // Start the attack
    public void HandleAttack() {
        if (!attacking) {
            if (Input.GetKey(KeyCode.O)) {
                    StartCoroutine(Attack());  
            } else if (hasGun && Input.GetKeyUp(KeyCode.L)) { // KeyUp event to avoid spam projectiles
                StartCoroutine(Shoot());
            }
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

    public IEnumerator Shoot() {
        attacking = true;
        GameObject newProjectile = Instantiate(projectiles[0], transform.position, transform.rotation);
        // Set direction of the bullet
        newProjectile.GetComponent<Bullet>().direction = last_direction;
        yield return new WaitForSeconds(1);
        attacking = false;
    }
}
