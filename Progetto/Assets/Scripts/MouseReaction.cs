using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MouseReaction : MonoBehaviour
{
    private Color startcolor;
    private Text tm;
    void Start()
    {
        tm = GetComponent<Text>();
    }
    void OnMouseEnter()
    {
        var render = GetComponent<Renderer>();

        render.material.color = Color.red;

        tm.fontStyle = FontStyle.Bold;
    }
    void OnMouseExit()
    {
        var render = GetComponent<Renderer>();

        render.material.color = Color.white;
    }

    
}
