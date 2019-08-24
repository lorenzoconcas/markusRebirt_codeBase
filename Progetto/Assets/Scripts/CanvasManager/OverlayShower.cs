using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayShower : MonoBehaviour {
    public GameObject pauseOverlay;
    public GameObject introOverlay;

    public GameObject player;
    public bool showIntro = !Data.SaveDataAvailable(); //se non ci sono salvataggi siamo alla prima partita

    private Data dT;
    void Start() {
        dT = GetComponentInParent<Data>();
        if (showIntro){ //da cambiare per farlo vedere solo la prima volta in accordo col salvataggio
        
            ToggleIntro(true);
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void ToggleIntro(bool enabled) {
        if (enabled) {
            introOverlay.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            player.SetActive(false);
        }
        else {
            introOverlay.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            player.SetActive(true);
        }
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
        if (enabled) {
            pauseOverlay.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            player.SetActive(false);
        }
        else {
            pauseOverlay.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            player.SetActive(true);
        }
    }

    public void ContinueGame() {
        TogglePause(false);
    }
}
