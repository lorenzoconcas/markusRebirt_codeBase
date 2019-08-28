using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataFile
{
    public enum PowerType {P1, P2, P3 }
    public Vector3 lastSpawn;
    public int lifes;
    public int level;
    public int health;
    public PowerType power;
    public float[] powerLevel = { 0.6f, 1.0f, 0.7f };
    public DataFile() {
        power = PowerType.P1;
       
        health = 100;
        lifes = 3;
        lastSpawn = new Vector3();
        level = 0; //range {0-2}
    }
    
}
