using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// Player
public class Player : SceneSingleton<Player> {

    // Movement
    float speed = 4;
    Vector2 direction = Vector2.zero;
    Vector2 lastDirection = new Vector2(0, 1);
    Rigidbody2D body;
    Animator animator;

    // Combat
    bool attacking = false;
    public int attack;
    public List<string> weapons;
    public GameObject[] attacks;

    // Health
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
        // Body & animator
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Spawn state
        PlayerData state;
        if (GameManager.Instance.warping) {  // Warping to new level
            hp = GameManager.Instance.warpHp;
            state = GameManager.Instance.warpData;
        } else {  // Respawning at checkpoint
            state = GameManager.Instance.playerData;
        }
        transform.position = new Vector3(state.spawnPos[0], state.spawnPos[1], state.spawnPos[2]);
        exp = state.spawnExp;
        weapons = new List<string>(state.spawnWeapons);
        skills = new List<string>(state.spawnSkills);
        GUIManager.Instance.UpdatePlayerStatus(hp, exp);
        GUIManager.Instance.ToggleGunIcon(weapons.Contains("Gun"));  // TODO: change later
        skills.Add("Sprint");  // TODO: Remove later
        skills.Add("Lightning");  // TODO: Remove later
        skills.Add("Barrier");  // TODO: Remove later
    }

    // Player interactions
    void Update() {
        GetInput();  // Process player input for movement/skills
        HandleAttack();  // Process player input for weapons
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

    // Player movement
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

    // Movement animation
    public void AnimationMove() {
        if (direction.x != 0 || direction.y != 0) {
            animator.SetLayerWeight(1,1);
        } else {
            animator.SetLayerWeight(1,0);
        }
        animator.SetFloat("x", direction.x * speed);
        animator.SetFloat("y", direction.y * speed);
    }

    // Sword attack
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

    // Gun attack
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
        if (tag == "Enemy" || tag == "Boss") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy.attack);
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
    public void TakeDamage(int damage) {
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
