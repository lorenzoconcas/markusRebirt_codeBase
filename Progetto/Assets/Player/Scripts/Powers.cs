using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powers : MonoBehaviour {
    private Data data;
    private GameObject[] enemies;
    private bool[] PowersAvailable = { true, true };
    [Header("CoolDown")]
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
    }
    // Update is called once per frame
    void Update() {
        var power = data.GetCurrentPowerEnum();
        var i = data.GetCurrentPower();
        var pLevels = data.getPowerLevel();
        if (Input.GetMouseButton(2) && i != 0 && pLevels[i] > 0 && PowersAvailable[i - 1]) {
            StartCoroutine("wait", new float[] { powerCoolDown, i - 1 });
            PowersAvailable[i - 1] = false;
            data.ReducePowerLevel(0.15f);
            DoPower(power);
        }
    }
    void DoPower(DataFile.PowerType i) {
        if (i == DataFile.PowerType.P2)
            power2();
        else
            power3();
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

        Instantiate(ParticleWindBeam, transform.position, transform.rotation); //particle system dell'allontanamento

        if (nearby != null)
            foreach (GameObject enemy in nearby) {
                enemy.transform.position -= enemy.transform.forward;
            }
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
