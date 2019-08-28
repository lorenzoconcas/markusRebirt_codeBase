using UnityEngine;

public class DeadEngine: MonoBehaviour {

    public string deadMessage = "Sei morto!";
    
    public float fallHeight = 250.0f;

    public AudioClip DeathSound;
    public AudioSource aSource;
    private StoryTeller sT;
    private Data dT;
    private bool dead;

    private void Start() {

        sT = GameObject.Find("Scripts").GetComponent<StoryTeller>();
      
        dT = GameObject.Find("Scripts").GetComponent<Data>();

        if (dT == null) {
            Debug.LogError("Data script not found");
            ExitGame.ExitStatic();
        }
    }

    void Update() {
        if (dead) {
            if (Input.GetKeyUp(KeyCode.Space)) {
                // sT.HideMessage(); non ne ho bisogno perchè ho già una funzione che chiude l'overlay
                dead = false;
            }
            return;
        }
        else {
            if (transform.position.y <= fallHeight) {
                ShowDead();
            }
        }


     
    }

    public void DoDamage() {
        if(GetComponent<TP_Animator>().State != TP_Animator.CharacterState.Defense)
            if (dT.SetDamage(50))
                ShowDead();

    }
    public void ShowDead() {
        aSource.clip = DeathSound;
        aSource.Play();
        sT.ShowMessage(deadMessage, "Premi SPACE per riprovare");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        dead = true;
        transform.position = dT.GetLastSpawn();
    }
}
