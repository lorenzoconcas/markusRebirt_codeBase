using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public GameObject loadingOverlay;  
    [Header("Main menu settings")]
    public GameObject dataKeeper;
  
    public Image preview;
    public int MainScene = 1;
    public int MenuScene = 2;
    public GameObject loadGameButton;
    
    public Sprite blackHUD;
    void Start() {
        Cursor.visible = true;

        if (preview != null) {
            var path = Application.persistentDataPath + "/preview_wallpaper.png";

            if (File.Exists(path)) {
                preview.sprite = IMG2Sprite.instance.LoadNewSprite(path);
            }
            else {
                preview.sprite = blackHUD;//(Sprite)Resources.Load("Assets/Resources/hud_background.png");
            }
        }


    }
    private void Update() {
        //qui e non in Start() perchè ha un leggero delay e non viene attivato
        if (Data.SaveDataAvailable() && loadGameButton != null) {
            loadGameButton.GetComponent<Text>().color = Color.white;
        }
    }
    public void GotoMainScene() {
        loadingOverlay.SetActive(true);
        SceneManager.LoadScene(MainScene);

    }


    public void GotoSceneNumber(int scn) {
        if (!Data.SaveDataAvailable() && scn == 6) {
            Debug.Log("Sorry no save data avaible");
        }
        else {
            loadingOverlay.SetActive(true);
            SceneManager.LoadScene(scn);
        }
    }
    public void GotoMenuScene() {
        SceneManager.LoadScene(MenuScene);
    }

    public void LoadGame() {

        if (!Data.SaveDataAvailable()) {
            Debug.Log("Sorry no save data avaible");
        }
        else {
            Data data = GetComponent<Data>();
            data.LoadSave();
            int scene = data.GetLevel() + 2;

            loadingOverlay.SetActive(true);
            DontDestroyOnLoad(dataKeeper);

            SceneManager.LoadScene(scene);


        }
    }


}
