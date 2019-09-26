using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(TP_Controller))]
[RequireComponent(typeof(TP_Animator))]
public class TP_Animator : MonoBehaviour {

    public enum Direction {
        Stationary,
        Forward,
        Backward,
        Left,
        Right,
        LeftForward,
        RightForward,
        LeftBackward,
        RightBackward
    }

    public enum CharacterState {
        Idle,
        Walking,
        Running,
        WalkingBackwards,
        StrafingLeft,
        StrafingRight,
        Jumping,
        Falling,
        Landing,
        Power1,
        Using,
        Dead,
        Attacking,
        Defense,
        ActionLocked
    }

    
    public static TP_Animator instance;

    [Header("Tempo di salita")]
    public float acendingTime = 2.0f;
    #region ANIMATIONS_SPEED

    [Header("Velocità Animazioni")]
    [Tooltip("Calcola la durata con la formula t = (f_count/fps)/multiplier")]
    //valori da regolare secondo t = (f_count/fps)/multiplier
    public float NormalAttack = 0.92f;
    public float RotoAttack = 0.7f;

    #endregion

    #region
    [Header("Brushblade")]
    public GameObject brushflow;
    public GameObject brushblade;
    #endregion
    #region PRIVATE_VARIABLES


    private Animator animator;

    public float runSpeed = 25.0f;

    private float JumpTimer =  0.0f ;

    public float FallHeight = 0.72f; //altezza oltre la quale è considerata come una caduta 

   
    #endregion


    private Data data;
    private bool RotoAttackAvailable = true;
    private bool NormalAttackAvailable = true;
   


    #region PUBLIC_PROPERTIES
    public Direction MoveDirection { get; set; }            // Property holds the movement direction.
    public CharacterState State { get; set; }               // Property holds the animation state.

    #endregion

    #region UNITY_FUNCTIONS

    // Use this for initialization
    void Awake() {
        if (instance != this) {
            instance = this;
        }

        //anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        State = CharacterState.Idle;
        data = GameObject.Find("Scripts").GetComponent<Data>();
       // brushflow = GameObject.FindGameObjectWithTag("Brushflow");
    }

    // Update is called once per frame
    void Update() {
      
        DetermineCurrentCharacterState();
        ProcessCurrentCharacterState();
     
    }

    #endregion

    #region PUBLIC_FUNCTIONS

    public void DetermineCurrentMoveDirection() {
        bool forward = false;
        bool backward = false;
        bool left = false;
        bool right = false;

        if (TP_Motor.instance.MoveVector.z > 0) {
            forward = true;
        }
        else if (TP_Motor.instance.MoveVector.z < 0) {
            backward = true;
        }

        if (TP_Motor.instance.MoveVector.x > 0) {
            right = true;
        }
        else if (TP_Motor.instance.MoveVector.x < 0) {
            left = true;
        }

        if (forward) {
            if (left) {
                MoveDirection = Direction.LeftForward;
            }
            else if (right) {
                MoveDirection = Direction.RightForward;
            }
            else {
                MoveDirection = Direction.Forward;
            }
        }
        else if (backward) {
            if (left) {
                MoveDirection = Direction.LeftBackward;
            }
            else if (right) {
                MoveDirection = Direction.RightBackward;
            }
            else {
                MoveDirection = Direction.Backward;
            }
        }
        else if (left) {
            MoveDirection = Direction.Left;
        }
        else if (right) {
            MoveDirection = Direction.Right;
        }
        else {
            MoveDirection = Direction.Stationary;
        }
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    void DetermineCurrentCharacterState() {

        if (State == CharacterState.Dead) {
            return;
        }


        if (!TP_Controller.characterController.isGrounded) {
            /*
             * Controlliamo che il player abbia davver fatto un salto o stia cadendo
             * è necessario questo controllo per le situazioni come il ponte
             * per farlo utilizziamo un raycast che calcola la distanza massima oltre la quale
             * viene considerata come una caduta
             */
            Ray downRay = new Ray(transform.position, Vector3.down);
            /*
             * controlliamo che il raycast intereschi con un collider
             * ossiamo semplicemente se tocca qualcosa
             * il raggio è puntato verso il basso rispetto al transform del player
             * se tocca qualcosa controlliamo che la distanza dai piedi del player 
             * all'oggetto sia entro un certo valore
             * in caso sia maggiore stiamo cadendo
             * in caso negativo stiamo ancora camminando
            */

            if (Physics.Raycast(downRay, out RaycastHit hit)) {
                if (hit.distance > FallHeight) {
                    State = CharacterState.Falling;
                }
                else {
                    /*                
                     * Se ho saltalto aspettiamo che finisca l'animazione del salto
                     * e poi imposto lo stato su caduta      
                     * altrimenti lascio lo stato attuale
                     */

                    JumpTimer += Time.deltaTime;

                    if (State == CharacterState.Jumping) {
                        if (JumpTimer >= acendingTime) {
                            JumpTimer = JumpTimer - acendingTime;
                            State = CharacterState.Falling;
                        }
                    }
                }
            }
            /*
             * Se non trovo niente col raycast significa che o sto cadendo
             * nel vuoto, imposto perciò subito lo stato di caduta
             * Nota : si potrebbe implementare stato morte
             */
            else {
                State = CharacterState.Falling;
            }
        }
        else {
            //alcuni casi hanno bisogno di una gestione specifica per ciò usiamo lo switch
            
            switch (State) {
                case CharacterState.Falling://se sto cadendo e sono ground atterro
                    State = CharacterState.Landing;
                    break;
                case CharacterState.Landing://se sono atterrato vado in idle
                    State = CharacterState.Idle;
                    break;
                case CharacterState.Power1:
                    if (RotoAttackAvailable) {
                        StartCoroutine("wait", new float[] { RotoAttack, 0f });
                        RotoAttackAvailable = false;
                        data.ReducePowerLevel(0.09f);
                        
                        //Instanziamo l'effetto del rotoattack, poi lo spostiamo di posizione
                        Instantiate(brushflow, brushblade.transform.position, Quaternion.identity);
                        var test = GameObject.FindGameObjectWithTag("Brushflow");  
                        test.transform.SetParent(brushblade.transform, true);
                        test.transform.position += new Vector3(1.0f, 0f, 1.0f);
                    }

                    break;
                case CharacterState.Attacking:
                    if (NormalAttackAvailable) {
                        StartCoroutine("wait", new float[] { RotoAttack, 1f });
                        NormalAttackAvailable = false;                       
                    }
                  
                    break;

                default:
                    //controllo che il player non sia in difesa
                    if (!Input.GetMouseButton(1))
                        switch (MoveDirection) {
                            case Direction.Forward:
                            case Direction.LeftForward:
                            case Direction.RightForward:
                            case Direction.Backward:
                                State = CharacterState.Walking;
                                break;
                            case Direction.LeftBackward:
                            case Direction.RightBackward:
                                State = CharacterState.WalkingBackwards;
                                break;
                            case Direction.Left:
                                State = CharacterState.StrafingLeft;
                                break;
                            case Direction.Right:
                                State = CharacterState.StrafingRight;
                                break;
                            case Direction.Stationary:
                                State = CharacterState.Idle;
                                break;
                        }

                    if (State == CharacterState.Walking && TP_Motor.instance.forwardSpeed == runSpeed)
                        State = CharacterState.Running;
                    break;
            }
        }

    }
    public IEnumerator wait(float[] parms) {
     
        yield return new WaitForSeconds(parms[0]);
        switch ((int)parms [1]) {
            case 0:
                State = CharacterState.Idle;
                RotoAttackAvailable = true;              
                break;
            case 1:
                State = CharacterState.Idle;
                NormalAttackAvailable = true;
                break;
           
        }
       
    }
    void ProcessCurrentCharacterState() {

        switch (State) {
            case CharacterState.Idle:
                animator.Play("Idle");
                break;
            case CharacterState.Walking:
                animator.Play("Walk");
                break;
            case CharacterState.Running:
                animator.Play("Run");
                break;
            case CharacterState.WalkingBackwards:
                break;

            case CharacterState.Jumping:
                animator.Play("JumpStart");
                break;
            case CharacterState.Falling:
                animator.Play("Fall");

                break;
            case CharacterState.Landing:
                animator.Play("JumpEnd");
                //Landing();
                break;
            case CharacterState.Power1:
                animator.Play("RotoAttack");
                break;

            case CharacterState.Attacking:
                animator.Play("Attack");
                break;
            case CharacterState.Defense:
                animator.Play("Defend");
                break;
            case CharacterState.Dead:
                break;
            case CharacterState.ActionLocked:
                break;
        }
    }

    #endregion



    #region START_ACTION_METHOD

    public void Defend() {
        State = CharacterState.Defense;
    }
    public void Attack() {
        State = CharacterState.Attacking;
    }
    public void RotatingAttack() {
        var pLevel = data.getPowerLevel();
        if (pLevel[0] > 0)
            State = CharacterState.Power1;
        else
            Debug.LogWarning("Hai finito il potere 1 ");
    }

    public void Jump() {
         instance.State = CharacterState.Jumping;    
    }
    #endregion
}
