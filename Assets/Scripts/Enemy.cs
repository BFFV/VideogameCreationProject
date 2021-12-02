using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float speed;

    private float moveSpeed;

    private Rigidbody2D myRigidbody;

    public bool moving;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;

    public float timeToMove;
    private float timeToMoveCounter;

    private Vector2 moveDirection;

    private GameObject player;
    private Vector2 movement;

    // Animator
    private Animator anim;

    // Attack
    public int attack;

    // Health
    public int hp;
    public int maxHp;
    private float recoveryTime = 0;
    public float hpLimit;  // Boss hp limit trigger (hp %)

    // Experience
    public int expValue;

    // Boss quake
    private GameObject quake;

    // Spawner
    public EnemySpawner spawner;

    // Enemy type
    public string enemyType;

    // Use this for initialization
    void Start() {
        hp = maxHp;
        moveSpeed = 0;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        myRigidbody = this.GetComponent<Rigidbody2D>();
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        quake = GameObject.FindGameObjectWithTag("Quake");
    }

    // Update is called once per frame
    void Update() {
        if (enemyType == "Skeleton") {
            if (moving) {
                timeToMoveCounter -= Time.deltaTime;
                myRigidbody.velocity = moveDirection;

                if (timeToMoveCounter < 0f) {
                    moving = false;
                    timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                    anim.SetFloat("MoveX", 0);
                    anim.SetFloat("MoveY", 0);
                }

            }
            else {
                timeBetweenMoveCounter -= Time.deltaTime;
                myRigidbody.velocity = Vector2.zero;
                if (timeBetweenMoveCounter < 0f) {
                    moving = true;
                    timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                    moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    Vector2 auxDirection = moveDirection;
                    auxDirection.Normalize();
                    anim.SetFloat("MoveX", auxDirection[0]);
                    anim.SetFloat("MoveY", auxDirection[1]);
                }
            }
        } else {
            if (enemyType == "SkeletonBoss") {
                if (hp > (hpLimit + 0.1) * maxHp) {
                    moving = false;
                } else if (hp <= (hpLimit + 0.1) * maxHp && hp >= hpLimit * maxHp ) {
                    anim.SetBool("Enraged", true);
                } else if (hp < hpLimit * maxHp) {
                    moving = true;
                    anim.SetBool("Enraged", false);
                }

                if (hp < (1 - hpLimit) * maxHp) {
                    moveSpeed = speed * 1.5f;
                }
            }
            if (enemyType == "Bat" || (enemyType == "SkeletonBoss" && moving)) {
                Vector3 direction = player.transform.position - transform.position;
                direction.Normalize();
                movement = direction;
                anim.SetFloat("MoveX", movement[0]);
                anim.SetFloat("MoveY", movement[1]);
            }
        }
        Recover();
    }

    private void FixedUpdate() {
        if (enemyType != "Skeleton") {
            MoveCharacter(movement);
        } else if (moving) {
            MoveCharacter(moveDirection);
        }
    }

    void MoveCharacter(Vector2 dir) {
        myRigidbody.MovePosition((Vector2)transform.position + (dir * moveSpeed * Time.deltaTime));
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
            // May be removed
            if (enemyType == "SkeletonBoss") {
                Destroy(quake);
            }
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

    // Activity radius
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (other.CompareTag("Player")) {
            moveSpeed = speed;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && enemyType != "Boss") {
            moveSpeed = 0;
        }
    }
}
