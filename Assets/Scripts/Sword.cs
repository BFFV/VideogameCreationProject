using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update

    private int timeExecution;
    private Player player;
    private Rigidbody2D body;

    private int damage;
    
    void Start()
    {
        // Sword attack takes one second (60 frames)
        timeExecution = 60;
        damage = 10;

        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeExecution > 0) {
            timeExecution--;
        } else {
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D other) {
        string[] enemies = {"Enemy", "Flying_enemy", "Boss"};
        string tag = other.gameObject.tag;
        if (enemies.Contains(tag)) {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            int expGained = enemy.TakeDamage(damage);
            player.GainExperience(expGained);
        }
    }
}
