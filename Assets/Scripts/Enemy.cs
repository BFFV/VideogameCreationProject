using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float moveSpeed;

    private Rigidbody2D myRigidbody;

    private bool moving;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;

    public float timeToMove;
    private float timeToMoveCounter;

    private Vector2 moveDirection;

    private GameObject player;
    private Vector2 movement;

    // Attack
    public int attack;

    // Health

    // HP
    public int hp;

    // Enemy is recovering from attack
    private bool recovering;

    // Recovery Frames
    private float recoveryTime;

    // Experience
    public int expValue;

    // Use this for initialization
    void Start() {
        recovering = false;
        recoveryTime = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        myRigidbody = this.GetComponent<Rigidbody2D>();
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
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
                }

            }
            else {
                timeBetweenMoveCounter -= Time.deltaTime;
                myRigidbody.velocity = Vector2.zero;
                if (timeBetweenMoveCounter < 0f) {
                    moving = true;
                    timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                    moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                }
            }
        } else {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            movement = direction;
        }
        Recover();
    }

    private void FixedUpdate() {
        if (gameObject.tag != "Enemy") {
            MoveCharacter(movement);
        } else {
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
                GameManager.Instance.EndGame();
            }
            Destroy(gameObject);
            return expValue;
        }
        recovering = true;
        recoveryTime = 1;
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
}
