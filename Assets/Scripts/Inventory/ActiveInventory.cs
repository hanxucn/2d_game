using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private  PlayerControls playerControls;

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start() {
        playerControls.Inventory.Keyboard.performed += ctx => ChangeActiveSlot((int)ctx.ReadValue<float>());

        ToggleActiveHighlight(0);
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void ChangeActiveSlot(int numValue) {
        ToggleActiveHighlight(numValue);
    }

    private void ToggleActiveHighlight(int indexNum) {
        if (indexNum <= 1) activeSlotIndexNum = 0;
        else activeSlotIndexNum = indexNum - 1;

        foreach (Transform inventorySlot in this.transform) {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(activeSlotIndexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();

    }

    public void EquipStartingWeapon() {
        ToggleActiveHighlight(0);
    }

    private void ChangeActiveWeapon() {
        // foreach (Transform inventorySlot in this.transform) {
        //     inventorySlot.GetChild(0).gameObject.SetActive(false);
        // }
        // this.transform.GetChild(activeSlotIndexNum).GetChild(0).gameObject.SetActive(true);
        if(PlayerHealth.Instance.IsDead) { return; }
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null) {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        if (transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo() == null) {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weapToSpawn = transform.GetChild(activeSlotIndexNum).
        GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;
        
        GameObject newWeapon = Instantiate(weapToSpawn, ActiveWeapon.Instance.transform.position, quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
        
    }
}
