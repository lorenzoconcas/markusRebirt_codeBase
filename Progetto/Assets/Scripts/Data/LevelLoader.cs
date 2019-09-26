using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    private GameObject dataLoad;
    private Data data;
    private GameObject player;
    void Start() {
        data = GetComponent<Data>();
        player = GameObject.Find("Markus");
        try {
            dataLoad = GameObject.Find("DataLoader(Clone)");
        }
        catch {

        }

        if (dataLoad != null) {
            Destroy(dataLoad);
            data.LoadSave();           
            switch (data.GetLevel()){
                case 1:
                    //se arriviamo dal primo livello aggiorniamo il valore livello e salviamo alcuni dati
                    data.SetLevel(2);
                    data.SetLastSpawn(player.transform.position);
                    break;
                case 2:
                    //se arriviamo dal secondo livello carichiamo l'ultima posizione
                    player.transform.position = data.GetLastSpawn();
                    break;
            }     
        }
    }
}
