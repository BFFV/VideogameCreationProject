using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Text that displays combat effects
public class CombatText : MonoBehaviour {

    // Setup
    float timeout = 10;
    Text txt;
    bool active = true;

    // Spawn
    void Start() {
        txt = GetComponent<Text>();
        //StartCoroutine(FadeTo(1, 0.5f));
    }

    // Fade In/Out
    IEnumerator FadeTo(float aValue, float aTime) {
        float alpha = txt.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            txt.color = newColor;
            yield return null;
        }
    }

    // Text activity
    void Update() {
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            if (active) {
                StartCoroutine(FadeTo(0, 0.5f));
                timeout = 1;
                active = false;
            } else {
                Destroy(gameObject);
            }
        }
    }
}
