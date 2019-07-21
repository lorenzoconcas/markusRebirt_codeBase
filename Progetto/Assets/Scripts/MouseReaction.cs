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
    /*void OnMouseEnter()
    {
        var render = GetComponent<Renderer>();

        render.material.color = Color.red;

        tm.fontStyle = FontStyle.Bold;
        transform.position.Set(50, 0, 0);
    }
    void OnMouseExit()
    {
        var render = GetComponent<Renderer>();

        render.material.color = Color.white;
    }*/

    void OnMouseDown()
    {
        Debug.Log("entrato");
    }
    void OnMouseEnter()
    {
        print("Enter");
    }
    void OnMouseUp()
    {
        print("up");
    }
    void OnMouseOver()
    {
        print("Over");
    }


}
