using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalSpawner : MonoBehaviour
{

    private GameObject[] enemies;
    public Light[] lights;
    public GameObject Portal;
    
    public Material darkSky;
    public AudioClip PortalOpeningSound;
    public AudioSource aSource;
    public AudioSource themeSpeaker;
    private bool PortalOpened = false;
    private float timer = 0.0f;
    private float LuxClock = 0.01f; //default 2 sec

    [Header("Data Management")]
    public GameObject loadingOverlay;
    public GameObject dataKeeper;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
       
    }

    // Update is called once per frame
    void Update()
    {
 
        var alive = 0;
        foreach(GameObject enemy in enemies) {
            if (enemy != null)
                alive++;
        }
        if (alive == 0) {
            LightsOut();
            OpenPortal();       
          
        }
    }

    void LightsOut() {
        timer += Time.deltaTime;
        if (timer >= LuxClock) {
            timer -= LuxClock;
            foreach (Light light in lights)
                light.intensity -= 0.01f;          
        }
    }
    void OpenPortal() {
        if (!PortalOpened) {
            Portal.SetActive(true);
            Instantiate(dataKeeper);
            DontDestroyOnLoad(dataKeeper);
            RenderSettings.skybox = darkSky;
            PortalOpened = true;

            aSource.clip = PortalOpeningSound;
            aSource.Play();
            themeSpeaker.volume = 0;
        }
    }

}
