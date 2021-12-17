using UnityEngine;
using TMPro;

// Combat text
public class DamagePopup : MonoBehaviour {

    // References
    TextMeshPro textMesh;
    float speed = 3f;
    float duration = 0.5f;
    Color textColor;

    // Create damage popup
    public static DamagePopup Create(Vector3 position, int damage) {
        Transform dmPopupT = Instantiate(GUIManager.Instance.damagePopup, position, Quaternion.identity);
        DamagePopup dp = dmPopupT.GetComponent<DamagePopup>();
        dp.Setup(damage);
        return dp;
    }

    // Awake
    public void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    // Setup
    public void Setup(int damageAmount) {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
    }

    // Smooth
    void Update() {
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        duration -= Time.deltaTime;
        if (duration < 0) {
            textColor.a -= 1;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }
}
