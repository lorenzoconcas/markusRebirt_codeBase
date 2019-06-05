using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MouseReaction : MonoBehaviour
{
    private Color startcolor;
    private UnityEngine.UI.Text test;
   public  void OnMouseOver()
    {

        Debug.Log(GetType());
    }

    public void OnMouseExit()
    {
       
        Debug.Log("Mouse is no longer on GameObject.");
    }
}
