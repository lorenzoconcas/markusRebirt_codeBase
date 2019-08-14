using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBShine : MonoBehaviour {
    private Light light;
    private Color color;
    private Color previousColor;
    private float hue = 0;
    public float colorChangeTime = 1.5f; //default 2 sec
    private float timer = 0.0f;
    public float hueIncrement = 0.03f;
    void Start() {
        light = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update() {

        timer += Time.deltaTime;
        if (timer >= colorChangeTime) {
            timer -= colorChangeTime;
            previousColor = light.color;
            light.color = Color.Lerp(previousColor, Color.HSVToRGB(hue, 1.0f, 1.0f), colorChangeTime); //new HSBColor(hue, 1.0f, 1.0f).ToColor();
            hue += hueIncrement;

            if (hue > 1.0f)
                hue = 0;
        }     
    
    }
}