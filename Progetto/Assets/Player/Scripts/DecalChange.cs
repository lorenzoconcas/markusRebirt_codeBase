using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalChange : MonoBehaviour {

    [Header("Objects")]
    public GameObject brush;
    public Material mate;

    [Header("Materials")]
    public Texture green;
    public Texture red;
    public Texture blue;

    [Header("Sounds")]
    public AudioClip PowerChangeEffect;
    public AudioSource aSource;

    private int count = 0;
    private Data data;
    // Start is called before the first frame update
    void Start() {
        data = GameObject.Find("Scripts").GetComponent<Data>();
        if (data == null)
            Debug.LogError("Script Holder non trovato");
    }

    // Update is called once per frame
    void Update() {

        UpdateDecal(data.GetCurrentPower());

        if (Input.GetKeyDown("e")) {
            if (data.getEnabledPowersCount() == 0)
                Debug.LogWarning("Non hai ancora sbloccato poteri");
            else {
                Play(PowerChangeEffect);
                var enabledP = data.getEnabledPowers();
                count = data.GetCurrentPower();
                count++;
                switch (count) {
                    case 0:
                        if (enabledP[0])
                            break;
                        if (enabledP[1] && !enabledP[2])
                            count = 1;
                        else
                            count = 2;
                        break;
                    case 1:
                        if (enabledP[1])
                            break;
                        if (enabledP[0] && !enabledP[2])
                            count = 0;
                        else
                            count = 2;

                        break;
                    case 2:
                        if (enabledP[2])
                            break;

                        if (enabledP[0])
                            count = 0;
                        else if (enabledP[1])
                            count = 1;
                        else
                            count = 0;
                        break;
                    default: //questo caso dovrebbe essere irraggiungibili ma è messo per sicurezza
                        count = firstEnabled();
                        break;
                }
                data.SetCurrentPower(count);
            }
        }
    }

    void UpdateDecal(int selected) {
        switch (selected) {
            case 0:
                mate.SetTexture("_DecalTex", red);
                break;
            case 1:
                mate.SetTexture("_DecalTex", green);
                break;
            case 2:
                mate.SetTexture("_DecalTex", blue);
                break;
        }
    }

    int firstEnabled() {
        var enabled = data.getEnabledPowers();
        for (int i = 0; i < 3; i++) {
            if (enabled[i]) return i;
        }
        return 0;
    }

    private void Play(AudioClip clip) {
        if (!(aSource.clip == clip && aSource.isPlaying))
            aSource.clip = clip;
        if (!aSource.isPlaying)
            aSource.Play();
    }
}
