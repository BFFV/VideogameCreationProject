using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// Player
public class Player : SceneSingleton<Player> {

    // Movement
    float speed = 4;
    Vector2 direction = Vector2.zero;
    Vector2 dynamicDirection = new Vector2(0, 1);
    Vector2 fixedDirection = new Vector2(0, 1);
    Vector3 lastPosition = new Vector2(0, 0);
    public Rigidbody2D body;
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
    bool sprinting = false;
    bool invincible = false;
    GameObject barrierSkill = null;
    public GameObject barrier;
    public GameObject lightning;
    public bool storm = false;
    public GameObject portal;
    public bool gateActive = false;
    GameObject gate;
    public GameObject explosion;
    public bool blast = false;
    public bool vortex = false;
    public GameObject blackHole;

    // Checkpoints
    public Checkpoint currentCheckpoint = null;

    // Environmental damage
    int lavaDamage = 4;

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

        // TODO: testing only
        weapons.Add("Gun");
        skills.Add("BlackHole");
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
                fixedDirection = new Vector2(-1, 0);
            } else if (Math.Abs(direction.x) > Math.Abs(direction.y) && direction.x > 0) {
                fixedDirection = new Vector2(1, 0);
            } else if (Math.Abs(direction.y) > Math.Abs(direction.x) && direction.y < 0) {
                fixedDirection = new Vector2(0, -1);
            } else if (Math.Abs(direction.y) > Math.Abs(direction.x) && direction.y > 0) {
                fixedDirection = new Vector2(0, 1);
            }
        }

        // Last direction for skills
        dynamicDirection = (transform.position - lastPosition).normalized;
        if (dynamicDirection.magnitude == 0) {
            dynamicDirection = new Vector2(fixedDirection.x, fixedDirection.y);
        }
        lastPosition = transform.position;

        // Sprint skill input
        if (skills.Contains("Sprint")) {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !sprinting) {
                sprinting = true;
                speed *= 2.5f;
                AudioManager.Instance.PlaySound("sprint");
            } else if (Input.GetKeyUp(KeyCode.LeftShift) && sprinting) {
                sprinting = false;
                speed /= 2.5f;
            }
        }

        // Lightning skill input
        if (skills.Contains("Lightning")) {
            if (Input.GetKeyDown(KeyCode.R) && !storm) {
                Instantiate(lightning, transform.position, Quaternion.identity);
                storm = true;
            }
        }

        // Barrier skill input
        if (skills.Contains("Barrier")) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                invincible = true;
                barrierSkill = Instantiate(barrier, transform);
                AudioManager.Instance.StartLoop("barrier");
            } else if (Input.GetKeyUp(KeyCode.Space)) {
                invincible = false;
                Destroy(barrierSkill);
                AudioManager.Instance.StopLoop();
            }
        }

        // Teleport skill input
        if (skills.Contains("Teleport")) {
            if (Input.GetKeyDown(KeyCode.F) && !gateActive) {
                float initX = (float) (transform.position.x + dynamicDirection.x);
                float initY = (float) (transform.position.y + dynamicDirection.y);
                gate = Instantiate(portal, new Vector3(initX, initY, 0), Quaternion.identity);
                gate.GetComponent<Teleport>().direction = dynamicDirection;
                gateActive = true;
            } else if (Input.GetKeyDown(KeyCode.F) && gate != null) {
                gate.GetComponent<Teleport>().Warp();
            }
        }

        // Explosion skill input
        if (skills.Contains("Explosion")) {
            if (Input.GetKeyDown(KeyCode.Q) && !blast) {
                Instantiate(explosion, transform.position, Quaternion.identity);
                blast = true;
            }
        }

        // Black Hole skill input
        if (skills.Contains("BlackHole")) {
            if (Input.GetKeyDown(KeyCode.L) && !vortex) {
                Instantiate(blackHole, transform.position, Quaternion.identity);
                vortex = true;
            }
        }

        // Save game input
        if (Input.GetKeyDown(KeyCode.G) && currentCheckpoint != null) {
            currentCheckpoint.SaveGame();
        }
    }

    // Player movement
    public void Move() {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    // Start the attack
    public void HandleAttack() {
        if (!attacking) {
            if (Input.GetKey(KeyCode.O)) {
                StartCoroutine(Attack());
            } else if (weapons.Contains("Gun") && Input.GetKeyDown(KeyCode.P)) {
                StartCoroutine(Shoot());
            } else if (weapons.Contains("Wind") && Input.GetKeyDown(KeyCode.J)) {
                StartCoroutine(WindShoot());
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
        float initX = (float) (transform.position.x + fixedDirection.x);
        float initY = (float) (transform.position.y + fixedDirection.y);
        GameObject sword = Instantiate(attacks[0], new Vector3(initX, initY, 0), transform.rotation);
        yield return new WaitForSeconds(1);
        animator.SetLayerWeight(2,0);
        Destroy(sword);
        attacking = false;
    }

    // Gun attack
    public IEnumerator Shoot() {
        attacking = true;
        float initX = (float) (transform.position.x + dynamicDirection.x);
        float initY = (float) (transform.position.y + dynamicDirection.y);
        GameObject newProjectile = Instantiate(attacks[1], new Vector3(initX, initY, 0), transform.rotation);

        // Set direction of the bullet
        newProjectile.GetComponent<Bullet>().direction = dynamicDirection;
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    // Wind attack
    public IEnumerator WindShoot() {
        float initX = (float) (transform.position.x + 1.2 * dynamicDirection.x);
        float initY = (float) (transform.position.y + 1.2 * dynamicDirection.y);
        GameObject newProjectile = Instantiate(attacks[2], new Vector3(initX, initY, 0), transform.rotation);

        // Set direction of wind
        newProjectile.GetComponent<Wind>().direction = dynamicDirection;
        yield return new WaitForSeconds(0.5f);
    }

    // Enemy damage
    void OnCollisionStay2D(Collision2D other) {
        string tag = other.gameObject.tag;
        // TODO: add other bosses
        if (tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy.attack);
        } else if (tag == "Boss1") {
            SkeletonBoss boss = other.gameObject.GetComponent<SkeletonBoss>();
            TakeDamage(boss.attack);
        }
    }

    // Environmental damage
    void OnTriggerStay2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag == "Lava") {
            TakeDamage(lavaDamage);
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

    // Warp effect
    public void OnWarp() {
        gateActive = false;
        StartCoroutine(FadeTo(1, 0.5f));
    }

    // Fade In/Out
    IEnumerator FadeTo(float cValue, float cTime) {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        float g = sprite.material.color.g;
        float b = sprite.material.color.b;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / cTime) {
            Color newColor = new Color(1, Mathf.Lerp(g, cValue, t), Mathf.Lerp(b, cValue, t), 1);
            sprite.material.color = newColor;
            yield return null;
        }
    }
}
