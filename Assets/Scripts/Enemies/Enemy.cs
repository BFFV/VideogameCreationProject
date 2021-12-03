using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy
public class Enemy : MonoBehaviour {

    // Movement
    public float speed;
    float moveSpeed;
    Rigidbody2D body;
    bool moving = true;
    public float timeBetweenMove;
    float timeBetweenMoveCounter;
    public float timeToMove;
    float timeToMoveCounter;
    Vector2 movement = Vector2.zero;
    Animator anim;

    // Combat
    public int attack;
    public float timeBetweenAttack;
    float timeBetweenAttackCounter;
    public GameObject[] attacks;

    // Health
    public int maxHp;
    public int hp;
    float recoveryTime = 0;

    // Experience
    public int expValue;

    // References
    GameObject player;
    public EnemySpawner spawner;

    // Enemy type
    public string enemyType;

    // Initialize enemy
    void Start() {
        hp = maxHp;
        moveSpeed = speed;
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        player = Player.Instance.gameObject;
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        if (enemyType == "Skeleton") {
            moving = false;
        }
        if (enemyType == "LavaEnemy") {
            timeBetweenAttackCounter = timeBetweenAttack;
        }
    }

    // Enemy interactions
    void Update() {
        // Skeleton
        if (enemyType == "Skeleton") {
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
        if (enemyType == "Bat" || enemyType == "LavaEnemy") {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            anim.SetFloat("MoveX", movement[0]);
            anim.SetFloat("MoveY", movement[1]);
            movement = direction;
        }

        // Lava Enemy
        if (enemyType == "LavaEnemy") {
            timeBetweenAttackCounter -= Time.deltaTime;
            if (timeBetweenAttackCounter <= 0f) {
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
                timeBetweenAttackCounter = timeBetweenAttack;
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
        body.MovePosition((Vector2) transform.position + (dir * moveSpeed * Time.deltaTime));
    }

    // Take damage from player
    public bool TakeDamage(int damage, bool forced = false) {
        // Invincibility
        if (recoveryTime > 0 && !forced) {
            return false;
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
            return true;
        }

        // Recovery frames
        recoveryTime = 0.5f;
        return false;
    }

    // Recover from attacks
    void Recover() {
        if (recoveryTime > 0) {
            recoveryTime -= Time.deltaTime;
            if (recoveryTime <= 0) {
                recoveryTime = 0;
            }
        }
    }
}
