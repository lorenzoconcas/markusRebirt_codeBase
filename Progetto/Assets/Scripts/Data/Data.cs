using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    private Vector3 lastSpawn;
    public GameObject player;
    private bool dead;

    public bool GetDead() {
        return dead;
    }

    public void SetDead(bool value) {
        dead = value;
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
