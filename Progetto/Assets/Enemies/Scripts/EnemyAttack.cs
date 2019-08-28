using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    private GameObject player;
    private DeadEngine DeadEngine;
    public float coolDown = 1.0f;
    private float timer = 0.0f;
    void Start() {
        player = GameObject.Find("Markus");
        DeadEngine = player.GetComponent<DeadEngine>();
    }


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("Markus")) {
            DeadEngine.DoDamage();
        }
    }
}
