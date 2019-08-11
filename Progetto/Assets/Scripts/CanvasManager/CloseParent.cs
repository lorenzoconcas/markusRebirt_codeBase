using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseParent : MonoBehaviour {

    public KeyCode key;
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
        player.gameObject.SetActive(true); //riattiviamo il player che è stato disattivato
        this.transform.parent.gameObject.SetActive(false);
        Cursor.visible = false;
        // parent.SetActive(false);
    }

}
