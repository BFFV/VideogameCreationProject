using System.Collections;
using UnityEngine;

// Laser trap
public class Laser : MonoBehaviour {

    // Setup
    float maxTime = 15.6f;
    float timeout;
    int damage = 50;
    public Vector2 direction = new Vector2(-1, 0);
    float speed = 10;
    bool active = false;
    bool close = false;

    // Create laser
    void Start() {
        transform.Rotate(0, 0, 90);
        timeout = maxTime;
        StartCoroutine(Spawn(3.1f, 0.5f));
    }

    // Spawn
    IEnumerator Spawn(float yValue, float sTime) {
        float x = 3.1f;
        float y = transform.localScale.y;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / sTime) {
            transform.localScale = new Vector3(x, Mathf.Lerp(y, yValue, t), 1);
            yield return null;
        }
        active = true;
    }

    // Switch direction
    void Update() {
        if (!active) {
            return;
        }
        timeout -= Time.deltaTime;
        if (timeout <= 0) {
            timeout = maxTime;
            direction *= -1;
        }
        transform.Translate(direction * speed * Time.deltaTime);
        float distance = (transform.position - Player.Instance.transform.position).magnitude;
        if (!close && distance < 20) {
            close = true;
            AudioManager.Instance.StartLoop("laser");
        } else if (close && distance > 20) {
            close = false;
            AudioManager.Instance.StopLoop();
        }
    }

    // Damage player
    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;

        // Laser damage
        if (tag == "Player") {
            Player.Instance.TakeDamage(damage);
        }
    }
}
