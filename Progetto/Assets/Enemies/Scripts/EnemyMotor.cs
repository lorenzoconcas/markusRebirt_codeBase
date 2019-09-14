using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour {
    /*
     * Utilizziamo uno script generico e un animator specifico
     */
    public enum Status { WALKING, ATTACKING, IDLE }

    private GameObject player;
    private float rotationSpeed = 10f;
    private float moveSpeed = 3;
    private CharacterController cCOntroller;

    private float distance;
    public float triggerDistance = 10.0f;
    public float attackDistance = 4.0f;
    public Status status;

    void Start() {
        cCOntroller = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        status = Status.IDLE;
        if (attackDistance > triggerDistance)
            Debug.LogError("La distanza di attacco non può essere maggiore di quella di trigger");
    }

    // Update is called once per frame
    void Update() {
        distance = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log("Distance from player "+ distance);
        status = Status.IDLE;
        if (distance <= triggerDistance) {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                 Quaternion.LookRotation(player.transform.position - transform.position),
                 rotationSpeed * Time.deltaTime);

            if (distance > attackDistance) {
                status = Status.WALKING;
                cCOntroller.Move(transform.forward * moveSpeed * Time.deltaTime);
            }
            else {// if(distance <= attackDistance) {
                status = Status.ATTACKING;
            }
        }

    }


}
