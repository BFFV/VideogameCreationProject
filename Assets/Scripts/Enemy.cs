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
    }

    // Enemy interactions
    void Update() {
        // Skeleton
        if (enemyType == "Skeleton") {
            if (moving) {
                timeToMoveCounter -= Time.deltaTime;
                if (timeToMoveCounter < 0f) {
                    moving = false;
                    timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                    anim.SetFloat("MoveX", 0);
                    anim.SetFloat("MoveY", 0);
                    movement = Vector2.zero;
                }
            } else {
                timeBetweenMoveCounter -= Time.deltaTime;
                if (timeBetweenMoveCounter < 0f) {
                    moving = true;
                    timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                    timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                    Vector3 direction = player.transform.position - transform.position;
                    direction.Normalize();
                    anim.SetFloat("MoveX", direction[0]);
                    anim.SetFloat("MoveY", direction[1]);
                    movement = direction;
                }
            }
            return;
        }

        // Bat
        if (enemyType == "Bat") {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            anim.SetFloat("MoveX", movement[0]);
            anim.SetFloat("MoveY", movement[1]);
            movement = direction;
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

    // Activate enemy
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (other.CompareTag("Player")) {
            moveSpeed = speed;
        }
    }

    // Deactivate enemy
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            moveSpeed = 0;
        }
    }
}
