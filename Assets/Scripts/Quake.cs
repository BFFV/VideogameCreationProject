using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quake : MonoBehaviour
{
    public int damage;

    // Animator
    private Animator anim;

    public GameObject skeletonBoss;
    private Enemy bossObject;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bossObject = skeletonBoss.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossObject.moving) {
            Destroy(gameObject);
        }
    }

}
