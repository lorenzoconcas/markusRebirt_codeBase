using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Splash : MonoBehaviour {

    public VideoPlayer VideoPlayer; // Drag & Drop the GameObject holding the VideoPlayer component
    public int SceneNumber;
    void Start() {
        Cursor.visible = false;
        VideoPlayer.loopPointReached += LoadScene;
    }
    void LoadScene(VideoPlayer vp) {
        SceneManager.LoadScene(SceneNumber);
    }
}
