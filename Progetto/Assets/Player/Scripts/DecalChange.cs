using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalChange : MonoBehaviour
{
    public GameObject brush;
    public Material mate;

    public Texture green;
    public Texture red;
    public Texture blue;

    private int count = 0;
 
    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            count++;

            if (count >= 3)
                count = 0;

            switch (count)
            {
                case 0:
                    mate.SetTexture("_DecalTex", green);
                    break;
                case 1:
                    mate.SetTexture("_DecalTex", red);
                    break;
                case 2:
                    mate.SetTexture("_DecalTex", blue);
                    break;
            }

        }


    }
}
    