using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Holy Charge (from Holy Beam)
public class HolyCharge : MonoBehaviour {

    // Setup
    float timeout = 6;
    bool active = true;

    // Start skill
    void Start() {
        AudioManager.Instance.PlaySound("holyCharge");
        StartCoroutine(Cast(0.6f, 1));
    }

    // Cast skill
    IEnumerator Cast(float xValue, float sTime) {
        float x = transform.localScale.x;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / sTime) {
            transform.localScale = new Vector3(Mathf.Lerp(x, xValue, t), Mathf.Lerp(x, xValue, t), 1);
            yield return null;
        }
    }

    // Skill activity
    void Update() {
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            if (active) {
                StartCoroutine(Cast(0, 1));
                timeout = 2;
                active = false;
            } else {
                Destroy(gameObject);
            }
        }
    }
}
