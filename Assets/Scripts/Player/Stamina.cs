using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 5;

    private int startingStamina = 3;

    private int maxStamina;

    private Transform staminaContainer;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake() {
        base.Awake();
        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start() {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    public void UseStamina() {
        CurrentStamina--;
        UpdateStaminImages();
        StopAllCoroutines();
        StartCoroutine(RefreshStaminaRoutine());
    }

    public void ReplenishStaminaOnDeath() {
        CurrentStamina = startingStamina;
        UpdateStaminImages();
    }

    public void RefreshStamina() {
        if (CurrentStamina < maxStamina && !PlayerHealth.Instance.IsDead) {
            CurrentStamina++;
        }
        UpdateStaminImages();
    }

    private IEnumerator RefreshStaminaRoutine() {
        while (true) {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    private void UpdateStaminImages() {
        for (int i = 0; i < maxStamina; i++) {
            Transform child = staminaContainer.GetChild(i);
            Image staminaImage = child?.GetComponent<Image>();
            
            if (i < CurrentStamina) {
                staminaImage.sprite = fullStaminaImage;
            } else {
                staminaImage.sprite = emptyStaminaImage;
            }
        }
    }
}
