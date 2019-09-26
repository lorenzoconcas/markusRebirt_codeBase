using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour {
    /*
     * Utilizziamo uno script generico e un animator specifico
     */
    public enum Status { WALKING, ATTACKING, IDLE, FROZEN, ATTACK_DONE }

    private GameObject player;
    private float rotationSpeed = 10f;
    private float moveSpeed = 3;
    private CharacterController cCOntroller;

    private float distance;

    //timer e cooldown attacco
    private float attackTimer = 0.0f;
    private float coolDown = 2.0f;
    private bool rejected = false;
    //distanza di trigger, attacco e rimbalzo dei nemici
    public float triggerDistance = 10.0f;
    public float attackDistance = 4.0f;
    public float estrangeDistance = 4.0f;

    private float freezeTime = 0.0f;
    private float frozenTimer = 0.0f;

    public Status status;

 
    public Material FrozenTexture;
    public Material NormalTexture;

  
    public GameObject FreezeParticleEffect;

    void Start() {
        cCOntroller = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        status = Status.IDLE;
        if (attackDistance > triggerDistance)
            Debug.LogError("La distanza di attacco non può essere maggiore di quella di trigger");
      
    }

    void Update() {

        switch (status) {
            case Status.FROZEN: {
                    frozenTimer += Time.deltaTime;
                    if (frozenTimer >= freezeTime) {
                        transform.Find("Cube").GetComponent<Renderer>().material = NormalTexture;
                      
                        status = Status.IDLE;
                        frozenTimer -= freezeTime;
                    }
                    break;
                }
            case Status.ATTACK_DONE: {
                    attackTimer += Time.deltaTime;
                    if (!rejected) {
                        rejected = true;
                        transform.position -= estrangeDistance * transform.forward;
                    }
                    if (attackTimer >= coolDown) {
                        status = Status.IDLE;
                        attackTimer -= coolDown;
                        rejected = false;
                    }
                    break;
                }
            default: {
                    distance = Vector3.Distance(transform.position, player.transform.position);
                    status = Status.IDLE;
                    if (distance <= triggerDistance) {
                        transform.rotation = Quaternion.Slerp(transform.rotation,
                             Quaternion.LookRotation(player.transform.position - transform.position),
                             rotationSpeed * Time.deltaTime);

                        if (distance > attackDistance) {
                            status = Status.WALKING;
                            cCOntroller.Move(transform.forward * moveSpeed * Time.deltaTime);
                        }

                        else {
                            status = Status.ATTACKING;
                        }
                    }
                    break;
                }
        }

    }

    public void freeze(float time) {
        if (status != Status.FROZEN) {
            status = Status.FROZEN;
            Instantiate(FreezeParticleEffect, transform.position, Quaternion.identity);
            transform.Find("Cube").GetComponent<Renderer>().material = FrozenTexture;
            freezeTime = time;
        }
    }

    public void attack() {
        status = Status.ATTACK_DONE;
    }
}

