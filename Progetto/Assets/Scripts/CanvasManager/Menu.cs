using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject loadingOverlay;
   
    public int MainScene = 1;
    public int MenuScene = 2;

    void Start()
    {
        Cursor.visible = true;
    }
    public void GotoMainScene()
    {
        loadingOverlay.SetActive(true);
        SceneManager.LoadScene(MainScene);
       
    }

    
    public void GotoSceneNumber(int scn)
    {
        if (!CheckSaved.IsGameSaveAvaible() && scn == 6) {
            Debug.Log("Sorry no save data avaible");
        }
        else {
            loadingOverlay.SetActive(true);
            SceneManager.LoadScene(scn);
        }
    }
    public void GotoMenuScene()
    {
        SceneManager.LoadScene(MenuScene);
    }

 
}
