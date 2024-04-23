using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;
    private enum State {
        Roaming,
        Attacking
    }

    private State state;

    private EnemyPathFinding enemyPathFinding;
    private Vector2 roamPos;
    private float timeRoaming = 0f;

    private void Awake() {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }
    
    private void Start() {
        roamPos = GetRoaminghPosition();
    }

    private void Update() {
        MovementStateControl();
    }

    private void MovementStateControl() {
        switch (state) {
            default:
            case State.Roaming:
                Roaming();
            break;
            case State.Attacking:
                Attacking();
            break;
        }
    }

    private void Roaming() {
        timeRoaming += Time.deltaTime;
        enemyPathFinding.MoveTo(roamPos);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackRange) {
            state = State.Attacking;
        }

        if (timeRoaming >= roamChangeDirFloat) {
            roamPos = GetRoaminghPosition();
        }
    }

    private void Attacking() {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange) {
            state = State.Roaming;
        }
        if (attackRange != 0 && canAttack) {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if(stopMovingWhileAttacking){
                enemyPathFinding.StopMoving();
            } else {
                enemyPathFinding.MoveTo(roamPos);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // private IEnumerator RoamingRoute() {
    //     while(state == State.Roaming) {
    //         Vector2 roamPos = GetRoaminghPosition();
    //         enemyPathFinding.MoveTo(roamPos);
    //         yield return new WaitForSeconds(roamChangeDirFloat);
    //     }
    // }

    private Vector2 GetRoaminghPosition() {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public int TakeDamage() {
        return this.damage;
    }

}
