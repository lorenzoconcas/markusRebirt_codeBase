using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataFile
{
   
    public Vector3 lastSpawn;
    public int lifes;
    public int level;
    public int health;

    public DataFile() {
        lifes = 3;
        lastSpawn = new Vector3();
        level = 0; //range {0-2}
    }
    
}
