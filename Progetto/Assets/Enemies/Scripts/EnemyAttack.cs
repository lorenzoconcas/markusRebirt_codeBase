using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    private GameObject player;
    private DeadEngine DeadEngine;
    public EnemyMotor eMotor;
    //public GameObject enemyRoot;
    
    void Start() {
        player = GameObject.Find("Markus");
        DeadEngine = player.GetComponent<DeadEngine>();
        //eMotor = enemyRoot.GetComponent<EnemyMotor>();
        if (eMotor == null)
            Debug.LogError("EnemyMotor Non trovato");
       // eMotor = GetComponent<EnemyMotor>();
    }


    void OnTriggerEnter(Collider other) {    
        if (other.gameObject.name.Contains("Markus")) {

            if (eMotor.status != EnemyMotor.Status.FROZEN) {
                DeadEngine.DoDamage();
                eMotor.attack();
            }
        }
    }   
}
 