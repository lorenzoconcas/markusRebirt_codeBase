using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataFile {
    public enum PowerType { P1, P2, P3 }
    public Vector3 lastSpawn;
    public int lifes;
    public int level;
    public int health;
    public PowerType currentPower;
    public bool[] unlockedPowers = { false, false, false };
    public float[] powerLevel = { 1.0f, 1.0f, 1.0f };
    public DataFile() {
        currentPower = PowerType.P1;

        health = 100;
        lifes = 3;
        lastSpawn = new Vector3();
        level = 0; //range {0-2}
    }

}
