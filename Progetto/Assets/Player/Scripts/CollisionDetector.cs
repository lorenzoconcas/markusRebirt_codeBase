using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetector : MonoBehaviour {
    public GameObject ScriptHolder;
    public AudioClip[] audioClips;
    public AudioSource aSource;
    public GameObject loadingOverlay;
    public string PortalMessage;
    private Data dS;
  
    public Material checkPointMaterial;

    public bool portalEntered { get; private set; }

    private void Start() {
    

        dS = ScriptHolder.GetComponent<Data>();
        if (dS == null)
            Debug.LogError("Script not found");

    }

     void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && portalEntered) {
            //load world
             dS.SaveGame();
             int scene = SceneManager.GetActiveScene().buildIndex+1;
             loadingOverlay.SetActive(true);
             DontDestroyOnLoad(GameObject.FindGameObjectWithTag("DataKeeper"));
             SceneManager.LoadScene(scene);
           // ExitGame.ExitStatic();

        }
    }

    public void OnTriggerEnter(Collider collider) {
        var tag = collider.gameObject.tag;
        var name = collider.gameObject.name;
      
        if (tag.Contains("Collectible")) {
            Destroy(collider.gameObject);
            PlayAudio(0);
            if (name.Contains("Drop"))
                dS.SetLowestPowerLevel(1.0f);
            else if (name.Contains("Heart"))
                dS.IncreaseLife();

            dS.SaveGame();
        }
        if (tag.Contains("checkpoint")) {
            //cambio la texture della tela
            collider.gameObject.transform.Find("Plane").gameObject.GetComponent<Renderer>().material = checkPointMaterial;
            collider.gameObject.GetComponent<Collider>().enabled = false;
            dS.SetLastSpawn(transform.position);
            dS.SaveGame();

            PlayAudio(1);

        }
        if (tag.Contains("PowerCrystal")) {
            var powerID = int.Parse(tag.Remove(0, 12)) - 1;
            dS.SetEnabledPowers(powerID);
            dS.SetCurrentPower(powerID);
            Destroy(collider.gameObject);
            dS.SaveGame();
            PlayAudio(0);
        }

        if (tag.Contains("Portal")) {
            portalEntered = true;
            var sT = GameObject.Find("Scripts").GetComponent<StoryTeller>();

            sT.ShowMessage(PortalMessage, "Premi SPACE per continuare");
        }
    }

   

    void PlayAudio(int id) {
        aSource.clip = audioClips[id];
        aSource.Play();
    }
}
