using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Block {
        public GameObject checkpoint;
        public GameObject[] nearbyEnemies;
    }

    public Block[] blocks;
   
    void Update()
    {
      foreach(Block block in blocks) {
            int alive = 0;
            foreach (GameObject enemy in block.nearbyEnemies)
                if (enemy != null)                    
                    alive++;

            if (alive == 0)
                block.checkpoint.SetActive(true);
      }
    }

   
}
