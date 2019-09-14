using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseParent : MonoBehaviour {

    public KeyCode key;
    public Camera camera;
    public GameObject enemies;
    public GameObject player;
    public void closeParent() {
        close();
    }
    private void Update() {
        if (Input.GetKeyDown(key)) {
            close();
        }
    }
    private void close() {
        camera.gameObject.SetActive(true); //riattiviamo il la camera che è stata disattivata
        transform.parent.gameObject.SetActive(false);
        Cursor.visible = false;
        try {
            if (player.activeSelf == false)
                player.SetActive(true);
            if (enemies.activeSelf == false)
                enemies.SetActive(true);
        }
        catch {

        }

    }

}
