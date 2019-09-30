using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour {
    #region PUBLIC_VARIABLES
    public int life = 1;
    #endregion

    [Header("Unity Setup")]
    public ParticleSystem deathParticles;
    public GameObject HitParticles;
    [Header("Suoni")]
    public AudioClip HitEffect;
    public AudioClip DeathEffect;
    public AudioSource aSource;
    /*elimina il nemico quando viene colpito dal pennello*/
   
    private void OnTriggerEnter(Collider other) {
        if (other.name.Contains("BrushBlade")) {
            HitReceived();
        }
    }

    void HitReceived() {
        var state = GameObject.Find("Markus").GetComponent<TP_Animator>().State;
        switch (state) {
            case TP_Animator.CharacterState.Attacking:
                life--;
                break;
            case TP_Animator.CharacterState.Power1:
                life -= 2;
                break;
        }
        if (state == TP_Animator.CharacterState.Attacking || state == TP_Animator.CharacterState.Power1) {
            if (life <= 0) {
                Play(DeathEffect);
                Destroy(gameObject);
                Instantiate(deathParticles, transform.position, Quaternion.identity);
            }
            else {
                Play(HitEffect);
                Instantiate(HitParticles, transform.position, Quaternion.identity);
            }
        }
    }
    private void Play(AudioClip clip) {
        if (!(aSource.clip == clip && aSource.isPlaying) && aSource != null)
            aSource.clip = clip;
        if (!aSource.isPlaying)
            aSource.Play();
    }
}
