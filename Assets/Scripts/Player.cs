using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : Singleton<Player> {

    // Movement

    // Player speed
    [SerializeField]
    private float speed;

    // Direction of movement
    private Vector2 direction;

    // Last direction of movement
    private Vector2 lastDirection;

    // Player body
    public Rigidbody2D body;

    // Attack

    // Player is attacking
    private bool attacking;

    // Player is using melee attack
    private bool melee;

    // Melee power
    public int attack;

    // Attack animation
    private Animator animator;

    // Player can shoot
    private bool hasGun;

    // Bullets
    [SerializeField]
    private GameObject[] projectiles;

    // Skills
    private bool fast;

    // Health

    // HP
    private int hp;

    // Player is recovering from attack
    private bool recovering;

    // Recovery Frames
    private float recoveryTime;

    // Stats

    // Level
    private int lvl;

    // GUI
    public int maxHp;
    public Text healthText;
    public Text expText;
    public Image gunIcon;

    // Experience
    public int exp;
    private int nextLvl;

    void Start() {
        hp = maxHp;
        attacking = false;
        melee = false;
        fast = false;
        recovering = false;
        recoveryTime = 1;
        direction = Vector2.zero;
        lastDirection = new Vector2(1, 0);
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // GUI
        gunIcon.enabled = false;

        // Exp
        exp = 0;
        nextLvl = 100;
        // lvl = 1;
        hasGun = false;

        // Events
        Inventory.Instance.onSkillActivatedCallback += ActivateFast;
    }


    void Update() {
        GetInput();
        UpdateGUI();
        HandleAttack();
        Recover();
    }

    void FixedUpdate() {
        Move();
    }

    // Receive input
    private void GetInput() {
        // Get movement input
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction.Normalize();

        // Save last direction of movement
        if (direction.x != 0 || direction.y != 0){
            lastDirection = direction;
        }

        // Fast run skill input
        if (fast) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                speed *= 2;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift)) {
                speed /= 2;
            }
        }
    }

    // Player Movement
    public void Move() {
        // Move body
        body.MovePosition(new Vector2(transform.position.x + direction.x * speed * Time.deltaTime,
                transform.position.y + direction.y * speed * Time.deltaTime));
    }

    // Start the attack
    public void HandleAttack() {
        if (!attacking) {
            if (Input.GetKey(KeyCode.O)) {
                StartCoroutine(Attack());
            } else if (hasGun && Input.GetKeyDown(KeyCode.L)) {
                StartCoroutine(Shoot());
            }
        }
    }

    // Melee attack
    private IEnumerator Attack() {
        animator.SetBool("Attacking", true);
        attacking = true;
        melee = true;
        yield return new WaitForSeconds(1);
        animator.SetBool("Attacking", false);
        attacking = false;
        melee = false;
    }

    // Ranged attack
    public IEnumerator Shoot() {
        attacking = true;
        float initX = (float) (transform.position.x + lastDirection.x);
        float initY = (float) (transform.position.y + lastDirection.y);
        GameObject newProjectile = Instantiate(projectiles[0], new Vector3(initX, initY, 0), transform.rotation);
        // Set direction of the bullet
        newProjectile.GetComponent<Bullet>().direction = lastDirection;
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    // Collisions
    void OnCollisionEnter2D(Collision2D other) {
        string[] enemies = {"Enemy", "Flying_enemy", "Boss"};
        string tag = other.gameObject.tag;
        if (enemies.Contains(tag)) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // Attack enemy
            if (melee) {
                int expGained = enemy.TakeDamage(attack);
                GainExperience(expGained);
            } else if (!recovering) {  // Take damage from enemy
                TakeDamage(enemy.attack);
            }
        }
    }

    void OnCollisionStay2D(Collision2D other) {
        string[] enemies = {"Enemy", "Flying_enemy", "Boss"};
        string tag = other.gameObject.tag;
        if (enemies.Contains(tag)) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // Attack enemy
            if (melee) {
                int expGained = enemy.TakeDamage(attack);
                GainExperience(expGained);
            } else if (!recovering) {  // Take damage from enemy
                TakeDamage(enemy.attack);
            }
        }
    }

    // Obtain experience
    public void GainExperience(int expObtained) {
        exp += expObtained;

        // Next level
        // if (exp >= nextLvl) {
            // nextLvl += 50;
        // }

        // Get gun
        if (!hasGun && exp >= 100) {
            hasGun = true;
            gunIcon.enabled = true;
        }
    }

    // Take damage from enemies
    void TakeDamage(int damage) {
        if (recovering) {
            return;
        }
        hp -= damage;
        if (hp <= 0) {
            GameManager.Instance.EndGame();
        }
        recovering = true;
        recoveryTime = 1;
    }

    // Recover from attacks
    void Recover() {
        if (recovering) {
            recoveryTime -= Time.deltaTime;
            if (recoveryTime <= 0) {
                recovering = false;
            }
        }
    }

    // Update GUI values
    void UpdateGUI() {
        healthText.text = "Health: " + hp.ToString() + "/" + maxHp.ToString();
        expText.text = "EXP: "+ exp.ToString() + "/" + nextLvl.ToString();
    }

    // Activate fast run ability
    void ActivateFast() {
        fast = true;
    }
}
