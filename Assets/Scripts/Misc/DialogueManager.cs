using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    public Button swordButton, bowButton, staffButton;
    [SerializeField] public WeaponInfo swordInfo;
    [SerializeField] public WeaponInfo bowInfo;
    [SerializeField] public WeaponInfo staffInfo;
    private int currentGold;

    private enum WeaponType {
        Sword,
        Bow,
        Staff
    }

    protected override void Awake() {
        base.Awake();
        if (dialoguePanel == null)
        {
            dialoguePanel = GameObject.Find("DialoguePanel"); // 确保这个名字与你的 GameObject 名称匹配
        }
    }

    private void Start() {
        currentGold = EconomyManager.Instance.currentGold;
        swordButton.onClick.AddListener(() => PurchaseItem(WeaponType.Sword, 10));
        bowButton.onClick.AddListener(() => PurchaseItem(WeaponType.Bow, 10));
        staffButton.onClick.AddListener(() => PurchaseItem(WeaponType.Staff, 10));

        dialoguePanel.SetActive(false);
    }

    public void ToggleDialogue(bool show)
    {
        if (dialoguePanel == null)
        {
            dialoguePanel = GameObject.Find("DialoguePanel"); // 确保这个名字与你的 GameObject 名称匹配
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(show);
        }
        else
        {
            Debug.LogError("DialoguePanel not found in the scene.");
        }
    }

    private void PurchaseItem(WeaponType weaponType, int price) {
        currentGold = EconomyManager.Instance.currentGold;
        if (currentGold >= price) {
            currentGold -= price;
            EconomyManager.Instance.UpdateCurrentGoldByAmount(currentGold);
            UpdateWeaponPower(weaponType);
        } else {
            dialogueText.text = "You do not have enough gold to purchase a " + weaponType + "!";
        }
        dialoguePanel.SetActive(true);
    }

    public bool IsActiveInHierarchy() {
        if (dialoguePanel == null)
        {
            dialoguePanel = GameObject.Find("DialoguePanel"); // 确保这个名字与你的 GameObject 名称匹配
        }

        if(dialoguePanel){
            return dialoguePanel.activeInHierarchy;
        } else{
            Debug.LogError("DialoguePanel not found in the scene.");
            return false;
        }
        
    }

    private void UpdateWeaponPower(WeaponType weaponType) {
        switch(weaponType) {
            case WeaponType.Sword:
                if(swordInfo.weaponDamage < 20){

                    int newDamage = swordInfo.weaponDamage + 1;
                    dialogueText.text = "Sword damage is " + swordInfo.weaponDamage;
                    swordInfo.weaponDamage = newDamage;
                    dialogueText.text = "Sword damage increased to: " + swordInfo.weaponDamage;
                } else {
                    dialogueText.text = "Sword damage is 20 already at maximum value!";
                }
                break;
            case WeaponType.Bow:
                if(bowInfo.weaponRange < 20){
                    float newRange = bowInfo.weaponRange + 0.5f;
                    dialogueText.text = "Bow range is " + bowInfo.weaponRange;
                    bowInfo.weaponRange = newRange;
                    dialogueText.text = "Bow range increased to: " + bowInfo.weaponRange;
                } else {
                    dialogueText.text = "Bow range is 20 already at maximum value!";
                }
                break;
            case WeaponType.Staff:
                if (staffInfo.weaponCooldown > 0.2f) {
                    float newCD = staffInfo.weaponCooldown - 0.1f;
                    dialogueText.text = "Staff cooldown is " + staffInfo.weaponCooldown;
                    staffInfo.weaponCooldown = newCD;
                    dialogueText.text = "Staff cooldown decreased to: " + staffInfo.weaponCooldown;
                    
                } else {
                    dialogueText.text = "Staff cooldown is 0.1 already at minimum value!";
                }
                break;
            default:
                break;
        }
    }
}
