using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float moveSpeed;

    private Rigidbody2D myRigidbody;

    private bool moving;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;

    public float timeToMove;
    private float timeToMoveCounter;

    private Vector3 moveDirection;

    public GameObject player;
    private Vector2 movement;

    // Use this for initialization
    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody2D>();

        timeBetweenMoveCounter = Random.Range (timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range (timeToMove * 0.75f, timeToMove * 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Enemy") {
            if (moving)
            {
                timeToMoveCounter -= Time.deltaTime;
                myRigidbody.velocity = moveDirection;

                if (timeToMoveCounter < 0f)
                {
                    moving = false;
                    timeBetweenMoveCounter = Random.Range (timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                }

            }
            else
            {
                timeBetweenMoveCounter -= Time.deltaTime;
                myRigidbody.velocity = Vector2.zero;
                if (timeBetweenMoveCounter < 0f)
                {
                    moving = true;
                    timeToMoveCounter = Random.Range (timeToMove * 0.75f, timeToMove * 1.25f);
                    moveDirection = new Vector3(Random.Range(-1f, 1f)* moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);
                }
            }
        } else if (gameObject.tag == "Flying_enemy") {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            movement = direction;
        }
    }
    private void FixedUpdate() {
        if (gameObject.tag == "Flying_enemy") {
            moveCharacter(movement);
        }
    }
    void moveCharacter(Vector2 dir) {
        myRigidbody.MovePosition((Vector2)transform.position + (dir * moveSpeed * Time.deltaTime));
    }

}
