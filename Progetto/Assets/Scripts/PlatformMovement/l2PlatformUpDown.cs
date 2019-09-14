using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class l2PlatformUpDown : MonoBehaviour {
    public GameObject[] targetPoint;
    public GameObject player;
    int current = 0;
    public float speed;
    float WPradius = 1;
    void Update() {
        if (Vector3.Distance(targetPoint[current].transform.position, transform.position) < WPradius) {
            current = Random.Range(0, targetPoint.Length);
            if (current >= targetPoint.Length) {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPoint[current].transform.position, Time.deltaTime * speed);

    }

    void OnTriggerEnter(Collider n) {
        if (n.gameObject == player) {
            player.transform.parent = transform;
        }
    }
    void OnTriggerExit(Collider n) {
        if (n.gameObject == player) {
            player.transform.parent = null;
        }
    }
}
