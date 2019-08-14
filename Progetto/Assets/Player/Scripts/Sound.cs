using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sound : MonoBehaviour
{
    public AudioClip[] Sounds;
    public AudioSource aSource;

    public TP_Animator animator;
    public TP_Animator.CharacterState state;
    private void Start() {
    
       
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        state = animator.State;
      
        switch (state) {
            case TP_Animator.CharacterState.Idle:
               // aSource.clip = Sounds[2];
                break;
            case TP_Animator.CharacterState.Walking:
                Debug.Log("ciao : "+Sounds[0]);
                aSource.clip = Sounds[0];
                aSource.loop = true;
                break;
            case TP_Animator.CharacterState.Running:
               
                break;
            case TP_Animator.CharacterState.WalkingBackwards:
               
                break;
            case TP_Animator.CharacterState.StrafingLeft:
              
                break;
            case TP_Animator.CharacterState.StrafingRight:
               
                break;
            case TP_Animator.CharacterState.Jumping:
                aSource.clip = Sounds[0];
                break;
            case TP_Animator.CharacterState.Falling:
               
                break;
            case TP_Animator.CharacterState.Landing:
               
                break;
            case TP_Animator.CharacterState.Climbing:
                break;
            case TP_Animator.CharacterState.Sliding:
                break;
            case TP_Animator.CharacterState.Using:
                
                break;
            case TP_Animator.CharacterState.Attacking:
               
                break;
            case TP_Animator.CharacterState.Victorious:
                
                break;
            case TP_Animator.CharacterState.Dead:
                break;
            case TP_Animator.CharacterState.ActionLocked:
                break;
        }
       
        aSource.Play();
    }
}
