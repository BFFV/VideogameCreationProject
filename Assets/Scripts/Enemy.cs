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

    // HP
    public int hp;
    public int maxHp;

    // Enemy is recovering from attack
    private bool recovering;

    // Recovery Frames
    private float recoveryTime;

    // Experience
    public int expValue;
    // Boss hp limit trigger (hp %)
    public float hpLimit;
    // Boss quake
    private GameObject quake;

    // Spawner
    public EnemySpawner spawner;

    // Use this for initialization
    void Start() {
        hp = maxHp;
        moveSpeed = 0;
        anim = GetComponent<Animator>();
        recovering = false;
        recoveryTime = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        myRigidbody = this.GetComponent<Rigidbody2D>();
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        quake = GameObject.FindGameObjectWithTag("Quake");
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.tag == "Enemy") {
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
            if (gameObject.tag == "Boss") {
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
            if (gameObject.tag == "Flying_enemy" || (gameObject.tag == "Boss" && moving)) {
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
        if (gameObject.tag != "Enemy") {
            MoveCharacter(movement);
        } else if (moving) {
            MoveCharacter(moveDirection);
        }
    }

    void MoveCharacter(Vector2 dir) {
        myRigidbody.MovePosition((Vector2)transform.position + (dir * moveSpeed * Time.deltaTime));
    }

    // Take damage from player
    public int TakeDamage(int damage) {
        if (recovering) {
            return 0;
        }
        hp -= damage;
        if (hp <= 0) {
            if (gameObject.tag == "Boss") {
                Destroy(quake);
            }
            spawner.EnemyDestroyed();
            Destroy(gameObject);
            return expValue;
        }
        recovering = true;
        recoveryTime = 0.5f;
        return 0;
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

    // Activity radius
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (other.CompareTag("Player")) {
            moveSpeed = speed;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && gameObject.tag != "Boss") {
            moveSpeed = 0;
        }
    }
}
