using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float speed = 2f;
    private float duration = 1f;
    private Color textColor;

    public static DamagePopup Create(Vector3 position, int damage) {
        Transform dmPopupT = Instantiate(GameManager.Instance.damagePopup, position, Quaternion.identity);
        DamagePopup dp = dmPopupT.GetComponent<DamagePopup>();
        dp.Setup(damage);
        return dp;
    }

    public void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount) {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
    }


    // Update is called once per frame
    void Update() {
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        duration -= Time.deltaTime;
        if (duration < 0) {
            textColor.a -= 1;// disspearSpeed * Time.deltaTime;
            textMesh.color= textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        } 
    }
}
