using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayShower : MonoBehaviour {
    public GameObject pauseOverlay;
    public GameObject introOverlay;
    public GameObject commandsOverlay;
    public GameObject enemiesContainer;
    public GameObject player;
    public AudioSource ThemeSpeaker;
    [Range(0f, 1f)] public float VolumeOnPause = 1.0f;
    
    private bool showIntro = !Data.SaveDataAvailable(); //se non ci sono salvataggi siamo alla prima partita

    private Data dT;
    void Start() {
        dT = GetComponentInParent<Data>();
        if (GameObject.Find("DataLoader") != null) {
            showIntro = false;
        }

        //evita di mostrare la splash screen ad ogni test
#if UNITY_EDITOR
        showIntro = false;
#endif
        if (showIntro && SceneManager.GetActiveScene().buildIndex == 2) { //lo mostro solo nel primo livello
            ToggleIntro(true);
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void ToggleIntro(bool enabled) {
        introOverlay.SetActive(enabled);
        SetVolume(enabled);
        Cursor.visible = enabled;
        player.GetComponent<TP_Animator>().State = TP_Animator.CharacterState.Idle;
        player.SetActive(!enabled);
        Cursor.lockState = CursorLockMode.Confined;

    }

    void Update() {

        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (!dT.GetDead()) {
                if (introOverlay.activeSelf) {
                    ToggleIntro(false);
                }
                else {
                    TogglePause(!pauseOverlay.activeSelf);
                }
            }
        }

    }

    private void TogglePause(bool enabled) {
        SetVolume(enabled);
        pauseOverlay.SetActive(enabled);
        Cursor.visible = enabled;
     
        player.SetActive(!enabled);

        try {
            enemiesContainer.SetActive(!enabled);
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }



        if (enabled) {
            Cursor.lockState = CursorLockMode.Confined;



            //var an = markus.GetComponent<TP_Animator>();
            //.State = TP_Animator.CharacterState.Idle;    
        }
        else {
            Cursor.lockState = CursorLockMode.Confined;
            commandsOverlay.SetActive(false);
        }
    }
    //serve per il tasto continua nel menu pausa
    public void ContinueGame() {
        TogglePause(false);
    }

    public void CloseCommandView() {
        commandsOverlay.SetActive(false);
    }
    public void OpenCommandsView() {
        commandsOverlay.SetActive(true);
    }

    public void SetVolume(bool reduce) {
        if (ThemeSpeaker != null) {
            if (reduce)
                ThemeSpeaker.volume = VolumeOnPause;
            else
                ThemeSpeaker.volume = 1.0f;
        }
    }

}
