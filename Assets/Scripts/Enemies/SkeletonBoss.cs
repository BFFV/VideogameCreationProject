using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Skeleton Boss
public class SkeletonBoss : MonoBehaviour {

    // Movement
    public float speed;
    float moveSpeed;
    public Rigidbody2D body;
    bool moving = false;
    Vector2 movement = Vector2.zero;
    Animator anim;
    bool active = true;
    public float activityRadius;
    public bool frozen = false;
    float freezeTimeout = 1;

    // Combat
    public int attack;

    // Health
    public int maxHp;
    public int hp;
    float recoveryTime = 0.5f;
    float recoveryDelta = 0.05f;
    public bool isRecovering = false;

    // References
    GameObject player;
    public GameObject rock;
    public SpriteRenderer sprite;
    Color currentColor;

    // Phases
    int phase = 0;
    float rageTimeout = 0;

    // Initialize boss
    void Start() {
        // Boss already defeated
        if (Player.Instance.skills.Contains("BlackHole")) {
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject rock in obstacles) {
                Destroy(rock);
            }
            Destroy(gameObject);
        }
        hp = maxHp;
        moveSpeed = speed;
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentColor = new Color(1, 1, 1, 1);
        player = Player.Instance.gameObject;
    }

    // Boss interactions
    void Update() {
        // Frozen
        if (frozen) {
            freezeTimeout -= Time.deltaTime;
            if (freezeTimeout <= 0) {
                freezeTimeout = 1;
                frozen = false;
                moveSpeed = speed;
                anim.enabled = true;
                sprite.material.color = currentColor;
            }
            return;
        }

        // Activity range
        float distance = (transform.position - Player.Instance.transform.position).magnitude;
        if (active && distance > activityRadius) {
            active = false;
            moveSpeed = 0;
        } else if (!active && distance <= activityRadius) {
            active = true;
            moveSpeed = speed;
        }
        if (!active) {
            return;
        }

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
            speed *= 1.5f;
            moveSpeed = speed;
            sprite.material.color = new Color(50, 0.5f, 0, 1);
            currentColor = sprite.material.color;
        } else if (phase == 2 && hp <= maxHp * 0.3) {
            phase = 3;
            sprite.material.color = new Color(255, 1, 0, 1);
            currentColor = sprite.material.color;
        }

        // Phase design

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
        if (isRecovering) {
            return;
        }

        // Lose HP
        hp -= damage;

        // Death
        if (hp <= 0) {
            BossDeath();
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
                sprite.material.color = currentColor;
            }
            yield return new WaitForSeconds(recoveryDelta);
        }
        sprite.material.color = currentColor;
        isRecovering = false;
    }

    // Freeze
    public void Freeze() {
        moveSpeed = 0;
        anim.enabled = false;
        frozen = true;
        sprite.material.color = new Color(0, 2f, 2f, 1);
    }

    // Death sequence
    void BossDeath() {
        AudioManager.Instance.PlaySoundtrack("lava");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject rock in obstacles) {
            Destroy(rock);
        }
        GUIManager.Instance.ShowEvent("You have learned the superior skill Black Hole!");
        Inventory.Instance.SetSkill("BlackHole");
        Destroy(gameObject);
    }

    // Activate boss
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (other.CompareTag("Player") && !moving) {
            Vector3 entrance = new Vector3(70, 8, 0);
            Instantiate(rock, entrance, Quaternion.identity);
            anim.SetBool("Enraged", true);
            rageTimeout = 0.5f;
            AudioManager.Instance.PlaySoundtrack("skeletonBoss");
        }
    }
}
