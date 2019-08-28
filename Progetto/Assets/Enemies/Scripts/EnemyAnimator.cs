using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemyMotor movScript;
    void Start()
    {
        movScript = GetComponent<EnemyMotor>();
        animator = GetComponent<Animator>();

    }

    
    void LateUpdate()
    {
        
      
        //riproduciamo l'animazione necessaria
        switch (movScript.status) {
            case EnemyMotor.Status.IDLE:
                animator.Play("Idle");
                break;
            case EnemyMotor.Status.WALKING:
                animator.Play("Walk");
                break;
            case EnemyMotor.Status.ATTACKING:
                animator.Play("Attack");
                break;
        }
    }
}
