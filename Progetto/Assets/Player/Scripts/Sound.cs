using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sound : MonoBehaviour {
    public AudioClip[] Sounds;
    public AudioSource aSource;

    public TP_Animator animator;
    public TP_Animator.CharacterState state;

    private void Start() {

        aSource.Play();

    }

    // Update is called once per frame
    void Update() {

        state = animator.State;

        switch (state) {
            case TP_Animator.CharacterState.Idle:
                aSource.clip = null;
                break;
            case TP_Animator.CharacterState.Walking:
                Play(Sounds[0]);
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
                Play(Sounds[1]);
                break;
            case TP_Animator.CharacterState.Falling:

                break;
            case TP_Animator.CharacterState.Landing:

                break;
            /*  case TP_Animator.CharacterState.Climbing:
                  break;
              case TP_Animator.CharacterState.Sliding:
                  break;*/
            case TP_Animator.CharacterState.Using:

                break;
            case TP_Animator.CharacterState.Attacking:

                break;

            case TP_Animator.CharacterState.Dead:
                break;
            case TP_Animator.CharacterState.ActionLocked:
                break;
        }




    }


    private void Play(AudioClip clip) {
        if (!(aSource.clip == clip && aSource.isPlaying))
            aSource.clip = clip;
        if (!aSource.isPlaying)
            aSource.Play();
    }
}
