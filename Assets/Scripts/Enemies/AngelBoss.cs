using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelBoss : MonoBehaviour
{
    // Movement
    public float speed;
    float moveSpeed;
    public Rigidbody2D body;
    bool moving = true;
    Vector2 movement = Vector2.zero;
    Animator anim;
    bool active = true;
    public float activityRadius;
    public bool frozen = false;
    float freezeTimeout = 1;

    // Combat
    public int attack;
    bool idle = true;

    // Health
    public int maxHp;
    public int hp;
    float recoveryTime = 0.5f;
    float recoveryDelta = 0.05f;
    public bool isRecovering = false;

    // References
    GameObject player;
    public SpriteRenderer sprite;
    Color currentColor;

    // Phases
    int phase = 0;
    public GameObject barrier;
    public GameObject lightning;
    public bool storm = false;
    public GameObject holyBeam;
    List<GameObject> attacks = new List<GameObject>();
    float minAttackInterval = 1f;
    float maxAttackInterval = 4f;

    // Start is called before the first frame update
    void Start()
    {
        // Boss already defeated
        if (Player.Instance.skills.Contains("HolyBeam")) {
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

    // Update is called once per frame
    void Update()
    {
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
            attacks.Add(holyBeam);
        } else if (phase == 1 && hp <= maxHp * 0.3) {
            phase = 2;
            attacks.Add(lightning);
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
        if (isRecovering || idle) {
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
            if (attacks.Count > 0 && !frozen && active) {
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
            isRecovering = true;
            yield return new WaitForSeconds(1.5f);
            Destroy(barrierSkill);
            AudioManager.Instance.StopLoop();
            isRecovering = false;
        } else {
            moveSpeed = 0;
            anim.enabled = false;
            if (attackID == 1) {  // Holy Beam
                GameObject holyBeamSkill = Instantiate(holyBeam, transform.position, Quaternion.identity);
                holyBeamSkill.GetComponent<HolyBeam>().AssignToEnemy();
                yield return new WaitForSeconds(0.5f);
            }
            if (attackID == 2) { // Storm
                GameObject stormSkill = Instantiate(lightning, transform.position, Quaternion.identity);
                stormSkill.GetComponent<Lightning>().AssignToEnemy();
                storm = true;
                yield return new WaitForSeconds(2f);
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
            //rageTimeout = 0.5f;
            //idle = false;
            //AudioManager.Instance.PlaySoundtrack("skeletonBoss");
        }
    }
}
