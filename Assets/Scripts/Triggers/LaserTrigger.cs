using UnityEngine;

// Spawn deadly laser
public class LaserTrigger : MonoBehaviour {

    // References
    public GameObject laser;
    public Vector2 position;
    public Vector2 direction;

    // Player enters laser trigger
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && GameObject.FindGameObjectWithTag("Laser") == null) {
            GameObject laserObj = Instantiate(laser, position, Quaternion.identity);
            laserObj.GetComponent<Laser>().direction = direction;
        }
    }
}
