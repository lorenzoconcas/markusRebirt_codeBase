﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class l3PlatformRotation : MonoBehaviour {
    void Update() {
        transform.Rotate(new Vector3(0f, 60f, 0f) * Time.deltaTime);
    }
}
