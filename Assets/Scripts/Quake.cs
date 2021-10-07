using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quake : MonoBehaviour {
    public int damage;

    // Animator
    private Animator anim;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
    }
}
