using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour {
    private Animator animator;
    private EnemyMotor movScript;
    void Start() {
        movScript = GetComponent<EnemyMotor>();
        animator = GetComponent<Animator>();
    }


    void LateUpdate() {


        //riproduciamo l'animazione necessaria
        switch (movScript.status) {
            case EnemyMotor.Status.FROZEN:
                animator.enabled = false; //pausa l'animazione
                break;
            case EnemyMotor.Status.ATTACK_DONE:
            case EnemyMotor.Status.IDLE:
                animator.enabled = true;
                animator.Play("Idle");
                break;
            case EnemyMotor.Status.WALKING:
                animator.enabled = true;
                animator.Play("Walk");
                break;
            case EnemyMotor.Status.ATTACKING:
                animator.enabled = true;
                animator.Play("Attack");
                break;
        }
    }
}
