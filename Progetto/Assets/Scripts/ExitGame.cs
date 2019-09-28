using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {
    public static void ExitStatic() {
        RenderSettings.skybox.SetFloat("_Exposure", 1.0f);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
         
#else
        Application.Quit(0);
#endif
      
    }
    public void Exit() {
        RenderSettings.skybox.SetFloat("_Exposure", 1.0f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(0);
#endif
       
    }
   
}

