using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Warping portal
public class Portal : MonoBehaviour {

    // Rotate forever
    void Update() {
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }
}
