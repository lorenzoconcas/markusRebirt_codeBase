using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    private Vector3 lastSpawn;
    public GameObject player;
    private bool dead;
    private int lifeCounter = 3;

   

    public bool GetDead() {
        return dead;
    }

    public void SetDead(bool value) {
        dead = value;
        if (value)
            lifeCounter--;
    }

    

    private void Start() {
        dead = false;
        lastSpawn = player.transform.position;
       
    }
    void  GoToLoadSave() {
        /*if (CheckSaved.IsGameSaveAvaible()) {
            LoadSc
        }*/
    }
    public Vector3 GetLastSpawn() {
        return lastSpawn;
    }

}
