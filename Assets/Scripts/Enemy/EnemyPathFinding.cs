using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
    }

    private void FixedUpdate() {
        if (knockback.GettingKnockedBack) {
            return;
        }
        Move();
    }

    private void Move()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        if(moveDir.x < 0) {
            spriteRenderer.flipX = true;
        } else if (moveDir.x > 0){
            spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPosition) {
        moveDir = targetPosition ;
    }

    public void StopMoving() {
        moveDir = Vector3.zero;
    }
}
