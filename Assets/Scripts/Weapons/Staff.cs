using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{

    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator myAnimator;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        weaponInfo.weaponCooldown = 1.2f;
    }

    private void Update() {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);
        //GameObject newMagicLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        // ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        GameObject newMagicLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        newMagicLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        if (Camera.main != null && PlayerController.Instance != null && ActiveWeapon.Instance != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
            float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

            if(mousePosition.x < playerScreenPosition.x){
                // flip weapon srpite
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
                // weaponCollider.rotation = Quaternion.Euler(0, -180, 0);
            } else {
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
                // weaponCollider.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        
    }
}
