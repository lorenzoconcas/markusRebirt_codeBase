using UnityEngine;

public class DontDeadOpenInside : MonoBehaviour {

    public GameObject scriptHolder;
    public float fallHeight = 250.0f;
    private StoryTeller sT;
    private Data dT;

    private bool t; //che odio

    private void Start() {

        sT = GetComponent<StoryTeller>();
        dT = scriptHolder.GetComponent<Data>();
        if (dT == null) {
            Debug.LogError("Data script not found");
            ExitGame.ExitStatic();
        }
    }

    void Update() {
        if (dT.GetDead()) {
            if (Input.GetKeyUp(KeyCode.Space)) {
               // sT.HideMessage(); non ne ho bisogno perchè ho già una funzione che chiude l'overlay
                dT.SetDead(false);

            }
            return;
        }
        else {
            if (transform.position.y <= fallHeight) {

                sT.ShowMessage("Sei caduto nel precipizio piu' oscuro della tua mente", "Premi SPACE per riprovare");
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                dT.SetDead(true);
                transform.position = dT.GetLastSpawn();

            }
        }
    }
}
