using UnityEngine;

// Change level music
public class MusicChange : MonoBehaviour {

    // Music
    public string music;

    // Player enters music trigger
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && AudioManager.Instance.currentMusic != music) {
            AudioManager.Instance.PlaySoundtrack(music);
        }
    }
}
