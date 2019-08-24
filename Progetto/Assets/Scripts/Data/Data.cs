using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour
{
    
    public GameObject player;
    private bool dead;
   
    private DataFile saveData;
    private int currentScene;
    private static string defaultLocation;
    private Screenshot screenshot;
    public Camera cam;

    

    public bool GetDead() {
        return dead;
    }

    public void SetDead(bool value) {
        dead = value;
        if (value)
            saveData.lifes--;
    }

    private void Start() {
        defaultLocation = Application.persistentDataPath + "/defaultSave.mdf";
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene >= 2 && currentScene <= 4) {
            if (GameObject.Find("DataLoader") != null) {
                Destroy(GameObject.Find("DataLoader"));

                LoadSave();
             
                player.transform.position = saveData.lastSpawn;

            }
            else {
                saveData = new DataFile();
                dead = false;
                saveData.lastSpawn = player.transform.position;
                screenshot = cam.GetComponent<Screenshot>();
                screenshot.folder = Application.persistentDataPath;
                screenshot.fName = "preview_wallpaper.png";
                screenshot.CaptureScreenshot();
            }
         
        }
      
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {         
            SaveGame();
        }
       
           
    }
    
    public Vector3 GetLastSpawn() {
        return saveData.lastSpawn;
    }
    public void SetLastSpawn(Vector3 spawn) {
        saveData.lastSpawn = spawn;
    }


    public void SaveGame() {
       
       
        var location = Application.persistentDataPath + "/defaultSave.mdf";
       
        string data = JsonUtility.ToJson(saveData, true); //pretty print!
        File.WriteAllText(location, data);
        try {
            screenshot.CaptureScreenshot();
        }
        catch { }

    }
    public void LoadSave(string location) {
        string retrievedData = File.ReadAllText(location);
        saveData = JsonUtility.FromJson<DataFile>(retrievedData);
       
        Debug.Log(saveData.lastSpawn);
        
    }
    public void LoadSave() {       
        LoadSave(defaultLocation);
    }
   
    public void PopulateSaveFile() {
     
        saveData.level = SceneManager.GetActiveScene().buildIndex - 2; //perchè ci sono due scene prima del primo livello
        saveData.health = 100;
    }


    public static bool SaveDataAvailable() {
        var exists = File.Exists(defaultLocation);     
        return exists;
    }

    public int GetLevel() {
        return saveData.level;
    }
    public void SetLevel(int level) {
        saveData.level = level;
    }

}
