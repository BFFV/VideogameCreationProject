using UnityEngine;

// Inventory UI
public class InventoryUI : MonoBehaviour {
    // References
    Inventory inventory;
    public GameObject skillSlot;
    public GameObject inventoryUI;
    SkillSlot skill;

    // Setup
    void Start() {
        inventory = Inventory.Instance;
        inventory.onSkillActivatedCallback += UpdateUI;
        skill = skillSlot.GetComponent<SkillSlot>();
    }

    // Toggle inventory
    void Update() {
        if (Input.GetButtonDown("Inventory")) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    // Update skill slot
    void UpdateUI() {
        skill.ActivateSkill(0);
    }
}
