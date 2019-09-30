using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    private GameObject dataLoad;
    private Data data;
    private GameObject player;
    void Start() {
        var thisLevel = SceneManager.GetActiveScene().buildIndex - 2;
        data = GetComponent<Data>();
        player = GameObject.Find("Markus");
        try {
            dataLoad = GameObject.Find("DataLoader(Clone)");
        }
        catch {

        }
        //se dataload esiste dobbiamo caricare il gioco per continuare la partita
        if (dataLoad != null) {
            Destroy(dataLoad);
            //carichiamo i dati precedenti
            data.LoadSave();
            //se stiamo caricando un salvataggio dal menu ricarichiamo la posizione del player
            if (data.GetLevel() == thisLevel)
                player.transform.position = data.GetLastSpawn();
            else
                data.SetLastSpawn(player.transform.position);
            //incrementiamo il livello nel dataFile
            data.IncreaseLevel(); //incrementiamo il livello
            data.SaveGame();

        }
        else {
            //sblocchiamo i poteri se partiamo dal livello 2 o 3, umpf
            switch (thisLevel) {
                case 1:
                    data.SetEnabledPowers(0);
                    break;
                case 2:
                    data.SetEnabledPowers(0);
                    data.SetEnabledPowers(1);
                    break;
            }
        }

    }
}
