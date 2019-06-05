using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject loadingOverlay;
    public GameObject scene;
    public GameObject scene2;
    public void GotoMainScene()
    {
        loadingOverlay.SetActive(true);
        SceneManager.LoadScene(1);
    }
    public void GotoSceneNumber(int scn)
    {
        loadingOverlay.SetActive(true);
        SceneManager.LoadScene(scn);
    }
    public void GotoMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowLevelSelector()
    {
        scene2.transform.position = scene.transform.position;
        scene.transform.position = new Vector2(-scene.transform.position.x, scene.transform.position.y);    
    }
    public void ShowMainMenu()
    {
        scene.transform.position = scene2.transform.position;
        scene2.transform.position = new Vector2(-scene.transform.position.x, scene.transform.position.y);
    }

   
}
