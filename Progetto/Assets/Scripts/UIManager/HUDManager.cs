using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Image[] hearts;
    public Image[] powerLevels;
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

        var pLevels =  data.getPowerLevel();
        var scene = SceneManager.GetActiveScene().buildIndex;

     

        if ( scene >= 2) {
            powerLevels[0].fillAmount = pLevels[0];
            if (scene >= 3) {
                powerLevels[1].fillAmount = pLevels[1];
                if(scene == 4)
                    powerLevels[2].fillAmount = pLevels[2];
            }
        }

        //METTERE QUESTA ROBA NEL COLLIDER DEL PENNELLEN
        //  GameObject.Find("Scripts").GetComponent<Data>().SetAttack(DataFile.PowerType.P1, 0.01f);
    }

}
