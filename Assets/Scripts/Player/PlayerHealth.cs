using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] public int maxHealth = 6;
    [SerializeField] private float knockBackThrustAmount = 5f;
    [SerializeField] private float damageRecoveryTime = 0.9f;

    public bool IsDead { get; private set; }

    private Slider healthSlider;
    public int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_SCENE_TEXT = "Town";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake() {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        currentHealth = maxHealth;
        IsDead = false;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy) {
            // take damage
            TakeDamage(enemy.TakeDamage(), other.transform);
        }
    }

    public void HealPlayer() {
        
        if (currentHealth >= maxHealth) {
            currentHealth = maxHealth;
        } else {
            currentHealth += 1;
        }
        
        UpdateHealthSlider();
    }

    private void CheckIfPlayerDeath() {
        if (currentHealth <= 0 && !IsDead) {
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            IsDead = true;
            currentHealth = 0;
            Destroy(ActiveWeapon.Instance.gameObject);
            StartCoroutine(DeathLoadScenseRoutine());
            Stamina.Instance.ReplenishStaminaOnDeath();
        }
    }

    private IEnumerator DeathLoadScenseRoutine() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_SCENE_TEXT);
    }

    public void TakeDamage(int damage, Transform hitTransform) {
        CheckIfPlayerDeath();
        if (!canTakeDamage || IsDead){
            return;
        }
        ScreenShakerManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashSprite());
        canTakeDamage = false;
        currentHealth -= damage;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
    }

    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
