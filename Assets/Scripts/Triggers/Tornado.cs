using UnityEngine;

// Tornado
public class Tornado : MonoBehaviour {

    // Setup
    int damage = 15;
    float impulse = 1000f;
    bool close = false;

    // Play sound when player is near
    void Update() {
        float distance = (transform.position - Player.Instance.transform.position).magnitude;
        if (!close && distance < 20) {
            close = true;
            AudioManager.Instance.StartLoop("tornado");
        } else if (close && distance > 20) {
            close = false;
            AudioManager.Instance.StopLoop("tornado");
        }
    }

    // Hit the player
    void OnTriggerStay2D(Collider2D other) {
        if (other.isTrigger) {
            return;
        }
        string tag = other.gameObject.tag;
        if (tag == "Player") {
            Player player = Player.Instance;
            player.TakeDamage(damage);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            player.body.AddForce(direction * impulse, ForceMode2D.Impulse);
        }
    }
}
