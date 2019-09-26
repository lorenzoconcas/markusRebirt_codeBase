using System;
using System.Collections;
using UnityEngine;

public class DeadEngine : MonoBehaviour {

    public string deadMessage = "Sei morto!";

    public float fallHeight = 250.0f;

    public AudioClip DeathSound;
    public AudioClip ShieldSound;
    public AudioClip PlayerDamageSound;
    public AudioSource aSource;
    private StoryTeller sT;
    private Data dT;
    private bool dead;
    private GameObject[] enemies;
    private Vector3[] positions;

    private void Start() {

        sT = GameObject.Find("Scripts").GetComponent<StoryTeller>();

        dT = GameObject.Find("Scripts").GetComponent<Data>();

        if (dT == null) {
            Debug.LogError("Data script not found");
            ExitGame.ExitStatic();
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        positions = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++) {
            positions[i] = enemies[i].gameObject.transform.position;
        }

    }

    void Update() {
      
        if (dead) {
            if (Input.GetKeyUp(KeyCode.Space)) {
                // sT.HideMessage(); non ne ho bisogno perchè ho già una funzione che chiude l'overlay
                dead = false;
                restoreEnemiesPosition();
                dT.SetLifes(3);
                dT.SetHealth(100);
            }
            return;
        }
        else
            if (transform.position.y <= fallHeight)
            ShowDead();
    }

    public void DoDamage() {
        var state = GetComponent<TP_Animator>().State;
        if (state != TP_Animator.CharacterState.Defense || state != TP_Animator.CharacterState.Power1) {
            if (dT.SetDamage(50))
                ShowDead();
            Play(PlayerDamageSound);
        }
        else if (state == TP_Animator.CharacterState.Defense)
            Play(ShieldSound);
    }
    public void ShowDead() {
        Play(DeathSound);
      
        sT.ShowMessage(deadMessage, "Premi SPACE per riprovare");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        dead = true;
        transform.position = dT.GetLastSpawn();

    }
    private void restoreEnemiesPosition() {
        int i = 0;
        foreach (GameObject g in enemies) {
            try {
                g.transform.position = positions[i];
                g.GetComponent<Animator>().Play("Idle");
            }
            catch {
                Debug.LogWarning("Nemico probabilmente eliminato o disattivato");
            }
            i++;
        }

    }

    private void Play(AudioClip clip) {
        if (!(aSource.clip == clip && aSource.isPlaying))
            aSource.clip = clip;
        if (!aSource.isPlaying)
            aSource.Play();
    }
}
