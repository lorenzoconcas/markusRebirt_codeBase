using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayShower : MonoBehaviour
{
    public GameObject pauseOverlay;
    public GameObject introOverlay;
    public GameObject backgroundObject;
    public GameObject camera;
    public bool showIntro = false;
    // public GameObject Camera;

    void Start()
    {
        if (showIntro) //da cambiare per farlo vedere solo la prima volta in accordo col salvataggio
        {
            camera.SetActive(false);

            Toggle();
            ToggleGameObjectVisibilty(introOverlay);          
        }
    }


    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape) && !introOverlay.activeSelf){
            Toggle();
            ToggleGameObjectVisibilty(pauseOverlay);          
        }       
        else        
         if (introOverlay.activeSelf)
            if (Input.anyKey)               
                  StartGame();
        
    }
    public void StartGame() //il tasto "Inizia" mostrato nell'introduzione del gioco
    {       
        Toggle();
        ToggleGameObjectVisibilty(introOverlay);
        camera.SetActive(true);
    }
   
    public void ContinueGame()
    {
        Toggle();
        ToggleGameObjectVisibilty(pauseOverlay);
    }
    //inverte lo stato degli oggetti connessi
    private void Toggle()
    {
       
        if (Cursor.visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;

        ToggleGameObjectVisibilty(backgroundObject);
       
    }
    private void ToggleGameObjectVisibilty(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }
}
