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
            case TP_Animator.CharacterState.Jumping:
                Play(Sounds[1]);
                break;
            case TP_Animator.CharacterState.Running:
                Play(Sounds[2]);
                break;
            case TP_Animator.CharacterState.Attacking:
                Play(Sounds[3]);
                break;
            case TP_Animator.CharacterState.Falling:
                break;
            case TP_Animator.CharacterState.Landing:
                Play(Sounds[4]);
                break;           
            case TP_Animator.CharacterState.Power1:
                Play(Sounds[5]);
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
