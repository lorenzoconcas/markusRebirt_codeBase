using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadMarkusSpawner : MonoBehaviour {
    private GameObject[] enemies;
    public GameObject MadMarkus;
    public GameObject pauseOverlay;

    public AudioClip BossTheme;
    public AudioSource aSource;

    private float exposureStart = 0.6f;
    private float exposureEnd = 0.2f;
    public float currentExposure { get; private set; }
    private float fadeTiming = 0.5f;

    // Start is called before the first frame update
    void Start() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

    }

    // Update is called once per frame
    void Update() {

        int alive = 0;
        foreach (GameObject enemy in enemies) {
            if (enemy != null)
                alive++;
        }

        if (alive == 0)
            SpawnMadMarkus();

        //disattiviamo madMarkus se siamo in pausa
        if (MadMarkus.activeSelf) {
            MadMarkus.SetActive(!pauseOverlay.activeSelf);
        }
    }

    void SpawnMadMarkus() {
        if (!MadMarkus.activeSelf) {
            MadMarkus.SetActive(true);
            //attiviamo il tema del boss
            aSource.clip = BossTheme;
            aSource.Play();
            //abbassiamo l'esposizione dello skybox
            fadeDown();
        }
    }



    private void fadeDown() {
        StartCoroutine(Fade(exposureStart, exposureEnd));
    }

    IEnumerator Fade(float startAlpha, float endAlpha) {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTiming) {
            elapsedTime += Time.deltaTime;
            var currentExposure = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTiming));
            RenderSettings.skybox.SetFloat("_Exposure", currentExposure);
            yield return new WaitForEndOfFrame();
        }
    }
}
