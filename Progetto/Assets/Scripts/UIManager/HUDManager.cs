using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public Image[] hearts;
    public Image[] powerLevels;
    public GameObject[] activeLevelIndicators;
    private Data data;
    void Start() {
        data = GetComponent<Data>();
        hearts[2].fillAmount = 100;
        hearts[1].fillAmount = 100;
        hearts[0].fillAmount = 100;
    }
    void Update() {
        var lifes = data.Lifes(); //valori {1-3}
        float health = (float)data.getHealth() / 100; //il cast viene effettuato prima per non perdere il valore

        switch (lifes) {
            case 2: //nascondiamo un cuore
                hearts[2].fillAmount = 0;
                break;
            case 1: //nascondiamo due cuori
                hearts[2].fillAmount = 0;
                hearts[1].fillAmount = 0;
                break;
                /*
            case 0: //li nascondiamo tutti, ma non penso servirà
                hearts[2].fillAmount = 0;
                hearts[1].fillAmount = 0;
                hearts[0].fillAmount = 0;
                break;*/
        }
        if (lifes - 1 >= 0)
            hearts[lifes - 1].fillAmount = health;


        var pLevels = data.getPowerLevel();
        var enabledPowers = data.getEnabledPowers();


        //attiviamo le barre di livello dei poteri sbloccati
        for (int i = 0; i < 3; i++) {
            powerLevels[i].transform.parent.gameObject.SetActive(enabledPowers[i]);
        }
        //e le riempiamo 
        if (enabledPowers[0])
            powerLevels[0].fillAmount = pLevels[0];
        if (enabledPowers[1])
            powerLevels[1].fillAmount = pLevels[0];
        if (enabledPowers[2])
            powerLevels[2].fillAmount = pLevels[0];

        //infine mettiamo l'indicatore sul potere attivo

        foreach (GameObject g in activeLevelIndicators) {
            g.SetActive(false);
        }
        var active = data.GetCurrentPower();
        //  Debug.Log(active);
        activeLevelIndicators[active].SetActive(true);


        //METTERE QUESTA ROBA NEL COLLIDER DEL PENNELLEN
        //  GameObject.Find("Scripts").GetComponent<Data>().SetAttack(DataFile.PowerType.P1, 0.01f);
    }

}
