using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalChange : MonoBehaviour {
    public GameObject brush;
    public Material mate;

    public Texture green;
    public Texture red;
    public Texture blue;

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
        if (Input.GetKeyDown("e")) {
            count++;
            if (count >= data.getEnabledPowersCount())
                count = 0;

            data.SetCurrentPower(count);

            switch (count) {
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


    }
}
