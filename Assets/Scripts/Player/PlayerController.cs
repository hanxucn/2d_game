using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;

    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private float startMoveSpeed;
    private Knockback knockback;

    private bool facingLeft = false;
    private bool isDashing = false;

    protected override void Awake() {
        base.Awake();
        playerControls = new PlayerControls();    
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void Start(){
        playerControls.Combat.Dash.performed += _ => Dash();
        startMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void FixedUpdate() {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider(){
        return weaponCollider;
    }


    private void PlayerInput()
    {
        moveInput = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", moveInput.x);
        myAnimator.SetFloat("moveY", moveInput.y);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) {
            return;
        }
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void AdjustPlayerFacingDirection(){
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        
        if(mousePosition.x < playerScreenPosition.x){
            // flip player srpite
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        } else {
            mySpriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash(){
        if(!isDashing && Stamina.Instance.CurrentStamina > 0){
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine(){
        float dashTime = 0.3f;
        float dashCD = 0.35f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

}
