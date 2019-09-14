using UnityEngine;
using System.Collections;
public class Gyro : MonoBehaviour {

    void Start() {
        Input.gyro.enabled = true;
    }

    void Update() {
        transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
    }
}