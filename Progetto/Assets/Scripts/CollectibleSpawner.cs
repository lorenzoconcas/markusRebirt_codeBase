    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour {
    #region PrivateValues
    private Data data;
    private int[] OriginalSize;
    private bool[] instantiated;
    private Vector3 lastValidPos;
    #endregion
    [System.Serializable]
    public struct Block {
        public GameObject collectible;
        public GameObject[] nearbyEnemies;
    }

    #region "Collectible and Enemy Container"
    [Header("Objects Container")]
    public Block[] blocks;
    #endregion

    #region
    [Header("Spawner Beheavior")]
    public int EnemyNecessaryToSpawnHearts = 2;
    public int LifeLosedToSpawnHearts = 1;
    public GameObject HeartModel;
    #endregion

    #region
    [Header("Refill Materials & UI")]
    public Material redMat;
    public Material greenMat;
    public Material blueMat;

    public float HeartScale = 0.6f;
    public float HeartYOffset = 2.0f;
    #endregion

    private void Start() {
        data = GameObject.Find("Scripts").GetComponent<Data>();
        OriginalSize = new int[blocks.Length];
        instantiated = new bool[blocks.Length];

       
        for (int i = 0; i < blocks.Length; i++) {
            OriginalSize[i] = blocks[i].nearbyEnemies.Length;
        }
    }


    void Update() {
        int i = 0;
        int alive = 0;
        foreach (Block block in blocks) {
            //contiamo quanti nemici sono vivi
            foreach (GameObject enemy in block.nearbyEnemies)
                if (enemy != null) {
                    alive++;
                    lastValidPos = enemy.transform.position;
                }

            //spawniamo un cuore se sono morti due nemici e il player ha perso almeno una vita
            if (alive <= OriginalSize[i] - EnemyNecessaryToSpawnHearts && data.Lifes() <= 3 - LifeLosedToSpawnHearts && !instantiated[i]) {
                HeartModel.transform.localScale = new Vector3(HeartScale, HeartScale, HeartScale );
                lastValidPos += new Vector3(0, HeartYOffset, 0); //alziamo un pò il cuore
                Instantiate(HeartModel, lastValidPos, Quaternion.identity);
               
                instantiated[i] = true;
            }


            //spawniamo un refill del potere più consumato se tutti i nemici vicini sono morti
            if (alive == 0 && block.collectible != null) {
                
                block.collectible.SetActive(true);

                var data = GameObject.Find("Scripts").GetComponent<Data>();

                switch (data.LowestPower()) {
                    case 0:
                        block.collectible.GetComponentInChildren<Renderer>().material = redMat;
                        block.collectible.GetComponentInChildren<Light>().color = Color.red;
                        
                        break;
                    case 1:
                        block.collectible.GetComponentInChildren<Renderer>().material = greenMat;
                        block.collectible.GetComponentInChildren<Light>().color = Color.green;
                        break;
                    case 2:
                        block.collectible.GetComponentInChildren<Renderer>().material = blueMat;
                        block.collectible.GetComponentInChildren<Light>().color = Color.blue;
                        break;
                }

            }

            i++;
        }

      
    }
}
