using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool GettingKnockedBack { get; private set;}
    [SerializeField] private float knockBackTime = 0.2f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust) {
        if(PlayerHealth.Instance.IsDead) { return; }
        GettingKnockedBack = true;
        Vector2 difference = transform.position - damageSource.position;
        difference = difference.normalized * knockBackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine() {
        yield return new WaitForSeconds(knockBackTime);
        if(PlayerHealth.Instance.IsDead) {
            rb.velocity = Vector2.zero; // 如果玩家死亡，立即停止移动
        }
        GettingKnockedBack = false;
    }
}
