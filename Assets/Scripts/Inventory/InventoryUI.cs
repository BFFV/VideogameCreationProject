using UnityEngine;

public class InventoryUI : MonoBehaviour {

    Inventory inventory;

    public GameObject skillSlot;
    public GameObject inventoryUI;

    SkillSlot skill;

    void Start() {
        inventory = Inventory.Instance;
        inventory.onSkillActivatedCallback += UpdateUI;
        skill = skillSlot.GetComponent<SkillSlot>();
    }

    void Update() {
        if (Input.GetButtonDown("Inventory")) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI() {
        skill.ActivateSkill(0);
    }
}
