using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Angel Boss
public class AngelBoss : MonoBehaviour {

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
    float freezeTimeout = 2;

    // Combat
    public int attack;
    bool idle = true;

    // Health
    public int maxHp;
    public int hp;
    float recoveryTime = 0.5f;
    float recoveryDelta = 0.05f;
    public bool isRecovering = false;
    public bool invincible = false;

    // References
    GameObject player;
    public GameObject rock;
    public SpriteRenderer sprite;
    Color currentColor;

    // Phases
    int phase = 0;
    public GameObject barrier;
    public GameObject lightning;
    public GameObject holyBeam;
    public GameObject holyCharge;
    List<GameObject> attacks = new List<GameObject>();
    float minAttackInterval = 1f;
    float maxAttackInterval = 4f;

    // Initialize boss
    void Start() {
        // Boss already defeated
        PlayerData state;
        if (GameManager.Instance.warping) {
            state = GameManager.Instance.warpData;
        } else {
            state = GameManager.Instance.playerData;
        }
        if (state.spawnSkills.Contains("HolyBeam")) {
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
        attacks.Add(barrier);
        StartCoroutine(ChooseAttack());
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

        // Phases
        if (phase == 0 && hp <= maxHp * 0.7) {
            phase = 1;
            speed *= 1.5f;
            moveSpeed = speed;
            sprite.material.color = new Color(0, 0.5f, 10, 1);
            currentColor = sprite.material.color;
            attacks.Add(lightning);
            maxAttackInterval -= 1;
        } else if (phase == 1 && hp <= maxHp * 0.3) {
            phase = 2;
            speed *= 1.5f;
            moveSpeed = speed;
            sprite.material.color = new Color(0, 1, 30, 1);
            currentColor = sprite.material.color;
            attacks.Add(holyBeam);
            maxAttackInterval -= 1;
        }
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
        if (isRecovering || idle || invincible) {
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

    // Choose special skills to attack randomly
    IEnumerator ChooseAttack() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minAttackInterval, maxAttackInterval + 1));
            if (attacks.Count > 0 && !frozen && active && !idle) {
                int chosen = Random.Range(0, attacks.Count);
                yield return UseAttack(chosen);
            }
        }
    }

    // Use special attack
    IEnumerator UseAttack(int attackID) {
        if (attackID == 0) {  // Barrier
            GameObject barrierSkill = Instantiate(barrier, transform);
            barrierSkill.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            AudioManager.Instance.StartLoop("barrier");
            invincible = true;
            yield return new WaitForSeconds(1.5f);
            Destroy(barrierSkill);
            AudioManager.Instance.StopLoop("barrier");
            invincible = false;
        } else {
            moveSpeed = 0;
            anim.enabled = false;
            if (attackID == 1) {  // Storm
                GameObject stormSkill = Instantiate(lightning, transform.position, Quaternion.identity);
                stormSkill.GetComponent<Lightning>().AssignToEnemy();
                yield return new WaitForSeconds(2f);
            }
            if (attackID == 2) {  // Holy Beam
                Vector3 vectorToTarget = (player.transform.position - transform.position).normalized;
                Vector3 offset = (transform.position + (Vector3) vectorToTarget * 1.5f);
                Instantiate(holyCharge, offset, Quaternion.identity);
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
                Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
                GameObject holyBeamSkill = Instantiate(holyBeam, transform.position, targetRotation);
                holyBeamSkill.GetComponent<HolyBeam>().AssignToEnemy();
                yield return new WaitForSeconds(0.5f);
            }
            moveSpeed = speed;
            anim.enabled = true;
        }
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
        StopAllCoroutines();
        AudioManager.Instance.PlaySound("victory", 2f);
        AudioManager.Instance.PlaySoundtrack("heaven1");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject rock in obstacles) {
            Destroy(rock);
        }
        GUIManager.Instance.ShowEvent("You have learned the superior skill Holy Beam!");
        Inventory.Instance.SetSkill("HolyBeam");
        Destroy(gameObject);
    }

    // Activate boss
    void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (other.CompareTag("Player") && !moving) {
            Vector3 entranceA = new Vector3(51, 17.4f, 0);
            Vector3 entranceB = new Vector3(51, 19.7f, 0);
            Instantiate(rock, entranceA, Quaternion.identity);
            Instantiate(rock, entranceB, Quaternion.identity);
            idle = false;
            moving = true;
            AudioManager.Instance.PlaySoundtrack("angelBoss");
        }
    }
}
