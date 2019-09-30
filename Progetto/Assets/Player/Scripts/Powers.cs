using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powers : MonoBehaviour {
    private Data data;
    private GameObject[] enemies;
    private bool[] PowersAvailable = { true, true };
    [Header("Power Consumption")]
    [Range(0f, 1f)] public float P1Cost = 0.09f;
    [Range(0f,1f)] public float P2Cost = 0.15f;
    [Range(0f, 1f)] public float P3Cost = 0.30f;
    [Header("CoolDown")]
    public float EstrangeDistance = 3.0f;
    public float powerCoolDown = 2.0f;
    [Header("Beheavior")]
    public float triggerDistance = 3.0f;
    public float freezeTime = 2.0f;
    [Header("Sounds")]
    public AudioSource aSource;
    public AudioClip Power2AudioEffect;
    public AudioClip Power3AudioEffect;
    [Header("Particle Effects")]
    public GameObject ParticleIceBeam;
    public GameObject ParticleWindBeam;


    private void Start() {
        data = GameObject.Find("Scripts").GetComponent<Data>();
        GetComponent<TP_Animator>().P1Cost = P1Cost;
        
    }
    // Update is called once per frame
    void Update() {
        var power = data.GetCurrentPowerEnum();
        var i = data.GetCurrentPower();
        var pLevels = data.getPowerLevel();
        if (Input.GetMouseButton(2) && i != 0 && pLevels[i] > 0 && PowersAvailable[i - 1]) {
            StartCoroutine("wait", new float[] { powerCoolDown, i - 1 });
            PowersAvailable[i - 1] = false;
           
            DoPower(power);
        }
    }
    void DoPower(DataFile.PowerType i) {
        if (i == DataFile.PowerType.P2) {
            power2();
            data.ReducePowerLevel(P2Cost);
        }
        else {
            power3();
            data.ReducePowerLevel(P3Cost);
        }
    }


    //where's il primo potere?
    /*
     * poichè coinvolge l'animator e il motor di markus (essendo un attacco fisico)
     * è posizionato in TP_Animator e possiede il proprio stato (a differenza di questi due)
     */

    //potere del congelamento
    private void power3() {
        if (Power3AudioEffect != null) {
            aSource.clip = Power3AudioEffect;
            aSource.Play();
        }
        var nearby = nearbyEnemies();

        if (ParticleIceBeam != null)
            Instantiate(ParticleIceBeam, transform.position, transform.rotation); //particle system del congelamento

        if (nearby != null)
            foreach (GameObject enemy in nearby) {
                enemy.GetComponent<EnemyMotor>().freeze(3.0f);
            }
    }
    //potere allontanamento
    private void power2() {
        if (Power2AudioEffect != null) {
            aSource.clip = Power2AudioEffect;
            aSource.Play();
        }
        var nearby = nearbyEnemies();
        if(ParticleWindBeam != null)
          Instantiate(ParticleWindBeam, transform.position, transform.rotation); //particle system dell'allontanamento

        if (nearby != null)
            foreach (GameObject enemy in nearby) {
              
                enemy.transform.position -= EstrangeDistance * enemy.transform.forward;
            }
        else
            Debug.LogWarning("Non ci sono nemici abbastanza vicini");
    }

    private GameObject[] nearbyEnemies() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject[] nearbyEnemies = new GameObject[0];
        int size = nearbyEnemies.Length;

        foreach (GameObject enemy in enemies) {
            var distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= triggerDistance) {
                size++;
                Array.Resize<GameObject>(ref nearbyEnemies, size);
                nearbyEnemies[size - 1] = enemy;
            }

        }


        return size == 0 ? null : nearbyEnemies;
    }

    /*
     * params :
     * float[0] = time to wait
     * float[1] = which case
    */
    public IEnumerator wait(float[] parms) {
        yield return new WaitForSeconds(parms[0]);
        PowersAvailable[(int)parms[1]] = true;
    }

}
