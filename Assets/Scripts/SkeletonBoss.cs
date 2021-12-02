using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Skeleton Boss
public class SkeletonBoss : MonoBehaviour {

    // Movement
    public float speed;
    float moveSpeed;
    Rigidbody2D body;
    bool moving = false;
    Vector2 movement = Vector2.zero;
    Animator anim;

    // Combat
    public int attack;

    // Health
    public int maxHp;
    public int hp;
    float recoveryTime = 0;

    // References
    GameObject player;

    // Phases
    int phase = 0;
    float rageTimeout = 2;

    // Initialize boss
    void Start() {
        hp = maxHp;
        moveSpeed = speed;
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        player = Player.Instance.gameObject;
    }

    // Boss interactions
    void Update() {
        // Movement
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        movement = direction;
        anim.SetFloat("MoveX", movement[0]);
        anim.SetFloat("MoveY", movement[1]);

        // Boss Rage
        if (rageTimeout > 0) {
            rageTimeout -= Time.deltaTime;
            if (rageTimeout <= 0) {
                rageTimeout = 0;
                anim.SetBool("Enraged", false);
                moving = true;
                phase = 1;
            }
        }

        // Phases
        if (phase <= 1 && hp <= maxHp * 0.7) {
            phase = 2;
            moveSpeed = speed * 1.5f;
        } else if (phase == 2 && hp <= maxHp * 0.3) {
            phase = 3;
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
        if (recoveryTime > 0) {
            return false;
        }

        // Lose HP
        hp -= damage;

        // Death
        if (hp <= 0) {
            BossDeath();
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

    // Death sequence
    void BossDeath() {
        Destroy(gameObject);
    }

    // Activate boss
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (other.CompareTag("Player")) {
            anim.SetBool("Enraged", true);
        }
    }
}