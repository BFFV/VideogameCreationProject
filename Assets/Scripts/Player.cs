using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

// Player
public class Player : SceneSingleton<Player> {

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

    // Combat

    // Player is attacking
    private bool attacking;

    // Melee power
    public int attack;

    // Attack animation
    private Animator animator;

    // Weapons
    public List<string> weapons;

    // Bullets
    [SerializeField]
    private GameObject[] attacks;

    // Skills
    public List<string> skills;

    // Health

    // HP
    public int hp;

    // Player is recovering from attack
    private bool recovering;

    // Recovery Frames
    private float recoveryTime;

    // GUI
    public int maxHp;
    public Text healthText;
    public Text expText;
    public Image gunIcon;

    // Experience
    public int exp;

    // Checkpoints
    public Checkpoint currentCheckpoint = null;

    void Start() {

        // Base stats
        hp = maxHp;
        attacking = false;
        recovering = false;
        recoveryTime = 1;
        direction = Vector2.zero;
        lastDirection = new Vector2(0, 1);

        // Body & animator
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Spawn state
        PlayerData state;
        if (GameManager.Instance.warping) {  // Warping to new level
            hp = GameManager.Instance.warpHp;
            state = GameManager.Instance.warpData;
        } else {  // Respawning at checkpoint
            hp = maxHp;
            state = GameManager.Instance.playerData;
        }
        transform.position = new Vector3(state.spawnPos[0], state.spawnPos[1], state.spawnPos[2]);
        exp = state.spawnExp;
        weapons = new List<string>(state.spawnWeapons);
        gunIcon.enabled = weapons.Contains("Gun");  // Needs to be removed later (replaced by current weapon icon)
        skills = new List<string>(state.spawnSkills);
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

    // Receive player input
    private void GetInput() {
        // Get movement input
        if (!attacking) {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        } else {
            direction = Vector2.zero;
        }

        // Animate the player movement
        AnimationMove();
        direction.Normalize();

        // Save last direction of movement
        if (direction.x != 0 || direction.y != 0){
            if (Math.Abs(direction.x) > Math.Abs(direction.y) && direction.x < 0 ) {
                lastDirection = new Vector2(-1,0);
            } else if (Math.Abs(direction.x) > Math.Abs(direction.y) && direction.x > 0) {
                lastDirection = new Vector2(1,0);
            } else if (Math.Abs(direction.y) > Math.Abs(direction.x) && direction.y < 0) {
                lastDirection = new Vector2(0, -1);
            } else if (Math.Abs(direction.y) > Math.Abs(direction.x) && direction.y > 0) {
                lastDirection = new Vector2(0, 1);
            }
        }

        // Fast run skill input
        if (skills.Contains("Sprint")) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                speed *= 2;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift)) {
                speed /= 2;
            }
        }

        // Save game input
        if (Input.GetKeyDown(KeyCode.G) && currentCheckpoint != null) {
            currentCheckpoint.SaveGame();
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
            } else if (weapons.Contains("Gun") && Input.GetKeyDown(KeyCode.L)) {
                StartCoroutine(Shoot());
            }
        }
    }

    public void AnimationMove() {
        if (direction.x != 0 || direction.y != 0) {
            animator.SetLayerWeight(1,1);
        } else {
            animator.SetLayerWeight(1,0);
        }
        animator.SetFloat("x", direction.x * speed);
        animator.SetFloat("y", direction.y * speed);
    }

    // Melee attack
    private IEnumerator Attack() {
        animator.SetLayerWeight(2,1);
        attacking = true;

        // Set Sword Object
        float initX = (float) (transform.position.x + lastDirection.x);
        float initY = (float) (transform.position.y + lastDirection.y);
        GameObject sword = Instantiate(attacks[1], new Vector3(initX, initY, 0), transform.rotation);
        yield return new WaitForSeconds(1);
        animator.SetLayerWeight(2,0);
        Destroy(sword);
        attacking = false;
    }

    // Ranged attack
    public IEnumerator Shoot() {
        attacking = true;
        float initX = (float) (transform.position.x + lastDirection.x);
        float initY = (float) (transform.position.y + lastDirection.y);
        GameObject newProjectile = Instantiate(attacks[0], new Vector3(initX, initY, 0), transform.rotation);

        // Set direction of the bullet
        newProjectile.GetComponent<Bullet>().direction = lastDirection;
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    // Collisions
    void OnCollisionStay2D(Collision2D other) {
        string[] enemies = {"Enemy", "Flying_enemy", "Boss"};
        string tag = other.gameObject.tag;
        if (enemies.Contains(tag)) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy.attack);
        }
    }

    // Collisions
    private void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Quake") {
            Quake quake = other.gameObject.GetComponent<Quake>();
            TakeDamage(quake.damage);
        }
    }

    // Lava damage
    private void OnTriggerStay2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Lava") {
            TakeDamage(2);
        }
    }

    // Obtain experience
    public void GainExperience(int expObtained) {
        exp += expObtained;

        // Heal by defeating enemy
        if (expObtained > 0 && hp < maxHp) {
            hp += 1;
        }
    }

    // Take damage from enemies
    void TakeDamage(int damage) {
        if (recovering) {
            return;
        }
        if (hp < damage) {
            hp = 0;
        } else {
            hp -= damage;
        }
        if (hp <= 0) {
            GameManager.Instance.StartGame();
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
        healthText.text = "HP: " + hp.ToString() + "/" + maxHp.ToString();
        expText.text = "EXP: "+ exp.ToString();
    }
}
