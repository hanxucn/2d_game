using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimationSpawnPoint;
    [SerializeField] private WeaponInfo weaponInfo;

    private Animator myAnimator;
    private Transform weaponCollider;
    // private PlayerController playerController;
    // private ActiveWeapon activeWeapon;

    private GameObject slashAnimation;

    private void Awake() {
        // playerController = GetComponentInParent<PlayerController>();
        // activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimationSpawnPoint = GameObject.Find("SlashAnimationSpawnPoint").transform;
    }

    // Update is called once per frame
    private void Update() {
        MouseFollowWithOffset();
    }

    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }
    
    public void Attack()
    {
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);
        slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnPoint.position, Quaternion.identity);
        slashAnimation.transform.parent = this.transform.parent;
        
        // StartCoroutine(AttackRoutine());
    }

    // private IEnumerator AttackRoutine()
    // {
    //     yield return new WaitForSeconds(attackSpeed);
    //     ActiveWeapon.Instance.ToggleIsAttacking(false);
    // }

    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnim()
    {
        if (slashAnimation != null) {
            slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

            if (PlayerController.Instance.FacingLeft)
            {
                slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

    }

    public void SwingDownFlipAnim()
    {
        if (slashAnimation != null) {
            slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (PlayerController.Instance.FacingLeft)
            {
                slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
            }
   
        }
 
    }

    private void MouseFollowWithOffset()
    {
        if (Camera.main != null && PlayerController.Instance != null && ActiveWeapon.Instance != null && weaponCollider != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
            float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

            if(mousePosition.x < playerScreenPosition.x){
                // flip weapon srpite
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
                weaponCollider.rotation = Quaternion.Euler(0, -180, 0);
            } else {
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
                weaponCollider.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        
    }
}
