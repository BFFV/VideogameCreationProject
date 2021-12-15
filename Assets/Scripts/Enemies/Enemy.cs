using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy
public class Enemy : MonoBehaviour {

    // Movement
    public float speed;
    float moveSpeed;
    public Rigidbody2D body;
    bool moving = true;
    public float timeBetweenMove;
    float timeBetweenMoveCounter;
    public float timeToMove;
    float timeToMoveCounter;
    Vector2 movement = Vector2.zero;
    Animator anim;
    bool active = true;
    public float activityRadius;
    public bool frozen = false;
    float freezeTimeout = 5;
    SpriteRenderer sprite;

    // Combat
    public int attack;
    public float timeBetweenAttack;
    float timeBetweenAttackCounter;
    public float attackDuration;
    public GameObject[] attacks;
    public GameObject barrier;
    public GameObject lightning;
    public bool storm = false;

    // Health
    public int maxHp;
    public int hp;
    float recoveryTime = 0.5f;
    float recoveryDelta = 0.05f;
    public bool isRecovering = false;

    // Experience
    public int expValue;

    // References
    GameObject player;
    public EnemySpawner spawner;

    // Enemy type
    public string enemyType;
    private List<string> chasingEnemies = new List<string>()
                    {
                        "Bat",
                        "LavaEnemy",
                        "Light",
                        "Lightball",
                        "Paladin"                    
                    };
    private List<string> attackingEnemies = new List<string>()
                    {
                        "LavaEnemy",
                        "Angel",
                        "Paladin"                    
                    };

    // Light elemental second form

    public GameObject secondForm;

    // Initialize enemy
    void Start() {
        hp = maxHp;
        moveSpeed = speed;
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = Player.Instance.gameObject;
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        if (!chasingEnemies.Contains(enemyType)) {
            moving = false;
        }
        if (attackingEnemies.Contains(enemyType)) {
            timeBetweenAttackCounter = timeBetweenAttack;
            StartCoroutine(ChooseAttack());
        }
    }

    // Enemy interactions
    void Update() {
        // Frozen
        if (frozen) {
            freezeTimeout -= Time.deltaTime;
            if (freezeTimeout <= 0) {
                freezeTimeout = 5;
                frozen = false;
                moveSpeed = speed;
                anim.enabled = true;
                sprite.material.color = new Color(1, 1, 1, 1);
            }
            return;
        }

        // Activity range
        float distance = (transform.position - Player.Instance.transform.position).magnitude;
        if (active && distance > activityRadius) {
            active = false;
            moveSpeed = 0;
            anim.enabled = false;
        } else if (!active && distance <= activityRadius) {
            active = true;
            moveSpeed = speed;
            anim.enabled = true;
        }
        if (!active) {
            return;
        }

        // Skeleton
        if (!chasingEnemies.Contains(enemyType)) {
            if (moving) {
                timeToMoveCounter -= Time.deltaTime;
                if (timeToMoveCounter < 0f) {
                    moving = false;
                    timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.9f, timeBetweenMove * 1.1f);
                    anim.SetFloat("MoveX", 0);
                    anim.SetFloat("MoveY", 0);
                    movement = Vector2.zero;
                }
            } else {
                timeBetweenMoveCounter -= Time.deltaTime;
                if (timeBetweenMoveCounter < 0f) {
                    moving = true;
                    timeToMoveCounter = Random.Range(timeToMove * 0.9f, timeToMove * 1.1f);
                    Vector3 direction = player.transform.position - transform.position;
                    direction.Normalize();
                    anim.SetFloat("MoveX", direction[0]);
                    anim.SetFloat("MoveY", direction[1]);
                    movement = direction;
                }
            }
        }

        // Bat
        if (chasingEnemies.Contains(enemyType)) {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            anim.SetFloat("MoveX", movement[0]);
            anim.SetFloat("MoveY", movement[1]);
            movement = direction;
        }

        // Light elemental second form
        if (enemyType == "Light") {
            if (hp <= Mathf.Floor(maxHp / 2)) {
                GameObject lightBall = Instantiate(secondForm, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        // Recovery frames
        Recover();
    }

    // Rigidbody movement
    void FixedUpdate() {
        if (moving) {
            MoveCharacter(movement);
        }
    }

    // Enemy movement
    void MoveCharacter(Vector2 dir) {
        transform.Translate(dir * moveSpeed * Time.fixedDeltaTime);
    }

    // Take damage from player
    public void TakeDamage(int damage, bool forced = false) {
        // Invincibility
        if (isRecovering && !forced) {
            return;
        }

        // Lose HP
        hp -= damage;

        // Death
        if (hp <= 0) {
            // Tell spawner that this enemy is dead
            if (spawner != null) {
                spawner.EnemyDestroyed();
            }
            // Reward player with exp
            Player.Instance.GainExp(expValue);
            Destroy(gameObject);
            return;
        }

        // Recovery frames
        StartCoroutine(Recover());
    }

    // Recovery state
    IEnumerator Recover() {
        isRecovering = true;
        for (float i = 0; i < recoveryTime; i += recoveryDelta) {
            if (sprite.material.color.a == 1) {
                sprite.material.color = new Color(1, 1, 1, 0);
            } else {
                sprite.material.color = new Color(1, 1, 1, 1);
            }
            yield return new WaitForSeconds(recoveryDelta);
        }
        sprite.material.color = new Color(1, 1, 1, 1);
        isRecovering = false;
    }

    // Freeze
    public void Freeze() {
        moveSpeed = 0;
        anim.enabled = false;
        frozen = true;
        sprite.material.color = new Color(0, 2f, 2f, 1);
    }

    IEnumerator ChooseAttack() {
        while (true) {
            yield return new WaitForSeconds(timeBetweenAttack);
            if (!frozen && active) {
                if (enemyType == "LavaEnemy") {
                    yield return shootFireball();
                } else {
                    int attackID = enemyType == "Paladin" ? 0 : 1;
                    yield return useAttack(attackID);
                }
            }
        }
    }

    // Lava enemy shoot fireball
    IEnumerator shootFireball() {
        float initX = (float) (transform.position.x);
        float initY = (float) (transform.position.y);
        float playerX = initX - player.transform.position.x;
        float playerY = initY - player.transform.position.y;
        float angle = Mathf.Atan(playerY / playerX) * Mathf.Rad2Deg;
        if (angle < 0.0f) {
            angle = 360 + angle;
        }
        if (playerX < 0.0f) {
            angle = angle + 180;
        }
        if (Mathf.Abs(playerX) > Mathf.Abs(playerY)) {
            if (playerX < 0) {
                initX += 1.5f;
            } else {
                initX -= 2f;
            }
        } else {
            if (playerY < 0) {
                initY += 1.5f;
            } else {
                initY -= 2f;
            }
        }
        GameObject newProjectile = Instantiate(attacks[0], new Vector3(initX, initY, 0), transform.rotation);
        // Set direction of the fireball
        newProjectile.GetComponent<Fireball>().direction = player.transform.position - transform.position;
        newProjectile.GetComponent<Fireball>().transform.Rotate(0.0f, 0.0f, angle, Space.Self);
        yield return new WaitForSeconds(attackDuration);
        timeBetweenAttackCounter = timeBetweenAttack;
    }

    IEnumerator useAttack(int attackID) {
        moveSpeed = 0;
        anim.enabled = false;
        if (attackID == 0) {  // Barrier
            GameObject barrierSkill = Instantiate(barrier, transform);
            AudioManager.Instance.StartLoop("barrier");
            isRecovering = true;
            yield return new WaitForSeconds(attackDuration);
            Destroy(barrierSkill);
            AudioManager.Instance.StopLoop("barrier");
            isRecovering = false;
        }
        if (attackID == 1) {  // Storm
            GameObject stormSkill = Instantiate(lightning, transform.position, Quaternion.identity);
            stormSkill.GetComponent<Lightning>().AssignToEnemy();
            storm = true;
            yield return new WaitForSeconds(2f);
        }
        moveSpeed = speed;
        anim.enabled = true;
    }
}
