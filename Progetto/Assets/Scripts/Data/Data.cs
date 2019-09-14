using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Data : MonoBehaviour {

    public GameObject player;


    private DataFile saveData;
    private int currentScene;
    private static string defaultLocation;
    private Screenshot screenshot;
    public Camera cam;



    public bool GetDead() {
        return saveData.lifes <= 0;
    }

    public int Lifes() {
        return saveData.lifes;
    }
    public int getHealth() {
        return saveData.health;
    }
    public float[] getPowerLevel() {
        return saveData.powerLevel;
    }
    //questa funzione segna il danno e restituisce se il player
    //non ha più vite (morto)
    public bool SetDamage(int damage) {
        // Debug.Log("Health : "+saveData.health+" Lifes : "+saveData.lifes);
        saveData.health -= damage;
        if (saveData.health <= 0) {
            saveData.health = 100;
            saveData.lifes--;
        }
        //Debug.Log("New Health : " + saveData.health + " Lifes : " + saveData.lifes);
        if (saveData.lifes <= 0)
            return true;

        return false;
    }


    private void Start() {

        defaultLocation = Application.persistentDataPath + "/defaultSave.mdf";
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene >= 2 && currentScene <= 4) {
            Cursor.visible = false;
            if (GameObject.Find("DataLoader") != null) { //dobbiamo caricare un salvataggio 
                Destroy(GameObject.Find("DataLoader"));
                GetComponent<OverlayShower>().showIntro = false;
                LoadSave();

                player.transform.position = saveData.lastSpawn;

            }
            else {
                GetComponent<OverlayShower>().showIntro = true;

                saveData = new DataFile();

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

        string data = JsonUtility.ToJson(saveData, true);
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
    public void SetAttack(DataFile.PowerType pType, float level) {
        switch (pType) {
            case DataFile.PowerType.P1:
                saveData.powerLevel[0] -= level;
                break;
            case DataFile.PowerType.P2:
                saveData.powerLevel[1] -= level;
                break;
            case DataFile.PowerType.P3:
                saveData.powerLevel[2] -= level;
                break;
        }

    }

    public bool[] getEnabledPowers() {
        return saveData.unlockedPowers;
    }
    public int getEnabledPowersCount() {
        int i = 0; foreach (bool b in saveData.unlockedPowers) {
            if (b) i++;
        }
        return i;
    }
    public void SetEnabledPowers(int powerID) {
        saveData.unlockedPowers[powerID] = true;
    }
    public void SetCurrentPower(int PowerID) {
        saveData.currentPower = (DataFile.PowerType)PowerID;
    }
    public int GetCurrentPower() {
        return (int)saveData.currentPower;
    }
}
