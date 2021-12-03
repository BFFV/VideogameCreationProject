using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Zap from lightning skill
public class Zap : MonoBehaviour {

    // Setup
    Material sprite;
    float timeout = 0.3f;

    // Start zap
    void Start() {
        sprite = gameObject.GetComponent<SpriteRenderer>().material;
        sprite.color = new Color(1, 1, 1, 0);
        StartCoroutine(ZapCycle());
    }

    // Fade In/Out
    IEnumerator FadeTo(float aValue, float aTime) {
        float alpha = sprite.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            sprite.color = newColor;
            yield return null;
        }
    }

    // Zap activity cycle
    IEnumerator ZapCycle() {
        yield return StartCoroutine(FadeTo(1.0f, 0.05f));
        AudioManager.Instance.PlaySound("thunder");
        yield return new WaitForSeconds(timeout);
        yield return StartCoroutine(FadeTo(0.0f, 0.1f));
        Destroy(gameObject);
    }
}
