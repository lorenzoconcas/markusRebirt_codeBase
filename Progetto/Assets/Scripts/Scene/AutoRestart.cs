using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoRestart : MonoBehaviour
{
    public float SceneTimeOut = 10f;
    public int MainMenuSceneNumber = 1;
    private float Timer;

    private void Start() {
        Timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= SceneTimeOut)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            SceneManager.LoadScene(MainMenuSceneNumber);
#endif
    }
}
