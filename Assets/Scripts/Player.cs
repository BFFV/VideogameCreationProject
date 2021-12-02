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
    private Vector2 direction = Vector2.zero;

    // Last direction of movement
    private Vector2 lastDirection = new Vector2(0, 1);

    // Player body
    public Rigidbody2D body;

    // Combat

    // Player is attacking
    private bool attacking = false;

    // Melee power
    public int attack;

    // Attack animation
    private Animator animator;

    // Weapons
    public List<string> weapons;

    // Bullets
    [SerializeField]
    private GameObject[] attacks;

    // Health
    public int maxHp;
    public int hp;
    float recoveryTime = 0;

    // Skills & Experience
    public int exp;
    public List<string> skills;
    bool invincible = false;
    GameObject barrierSkill = null;
    public GameObject barrier;
    public GameObject lightning;

    // Checkpoints
    public Checkpoint currentCheckpoint = null;

    // Initialize player
    void Start() {
        // Base HP
        hp = maxHp;

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
        skills = new List<string>(state.spawnSkills);
        GUIManager.Instance.UpdatePlayerStatus(hp, exp);
        GUIManager.Instance.ToggleGunIcon(weapons.Contains("Gun"));  // will be changed later
    }

    // Player interactions
    void Update() {
        GetInput();  // Process player input
        HandleAttack();  // Process attack
        Recover();  // Recovery time
    }

    // Rigidbody movement
    void FixedUpdate() {
        Move();
    }

    // Receive player input
    private void GetInput() {
        // Movement input
        if (!attacking) {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        } else {
            direction = Vector2.zero;
        }

        // Animate player movement
        AnimationMove();
        direction.Normalize();

        // Last direction of movement
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

        // Sprint skill input
        if (skills.Contains("Sprint")) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                speed *= 2;
            } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
                speed /= 2;
            }
        }

        // Lightning skill input
        if (skills.Contains("Lightning")) {
            if (Input.GetKeyDown(KeyCode.R)) {
                Instantiate(lightning, transform.position, Quaternion.identity);
            }
        }

        // Barrier skill input
        if (skills.Contains("Barrier")) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                invincible = true;
                barrierSkill = Instantiate(barrier, transform);
            } else if (Input.GetKeyUp(KeyCode.Space)) {
                invincible = false;
                Destroy(barrierSkill);
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

    // Enemy damage
    void OnCollisionStay2D(Collision2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy.attack);
        }
    }

    // Impulse damage
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Quake") {
            Quake quake = other.gameObject.GetComponent<Quake>();
            TakeDamage(quake.damage);
        }
    }

    // Environmental damage
    void OnTriggerStay2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Lava") {
            TakeDamage(2);
        }
    }

    // Take damage
    void TakeDamage(int damage) {
        // Invincibility
        if (recoveryTime > 0 || invincible) {
            return;
        }

        // Lose HP
        if (hp < damage) {
            hp = 0;
        } else {
            hp -= damage;
        }
        GUIManager.Instance.UpdatePlayerHealth(hp);

        // Death
        if (hp <= 0) {
            GameManager.Instance.StartGame();
        }

        // Recovery frames
        recoveryTime = 0.8f;
    }

    // Recovery state
    void Recover() {
        if (recoveryTime > 0) {
            recoveryTime -= Time.deltaTime;
            if (recoveryTime <= 0) {
                recoveryTime = 0;
            }
        }
    }

    // Gain experience
    public void GainExp(int expValue) {
        exp += expValue;
        GUIManager.Instance.UpdatePlayerExp(exp);
    }
}
