using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesEffect : MonoBehaviour
{
    [Header("Unity Setup")]
    public float time;
    void Start()
    {
        Destroy(gameObject, time);
    }
}
