using UnityEngine;

//This is a camera script made by Haravin (Daniel Valcour).
//This script is public domain, but credit is appreciated!

[RequireComponent(typeof(Camera))]
public class Camera3rd : MonoBehaviour
{
    public float HorizontalSensivity = 0.3f;
    public float VerticalSensivity = 0.3f;
    public float moveSpeed = 1.0f;
    public float shiftAdditionalSpeed ;
    public float mouseSensitivity  = 1.0f;
    public bool invertMouse;
    public bool autoLockCursor;
    public float speed = 1.0f;
    private Camera cam;

    void Awake()
    {
        cam = this.gameObject.GetComponent<Camera>();
        this.gameObject.name = "MainCamera";
        Cursor.lockState = (autoLockCursor) ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update()
    {
        //(moveSpeed + (Input.GetAxis("Fire3") * shiftAdditionalSpeed));
        // this.gameObject.transform.Translate(Vector3.forward * speed * Input.GetAxis("Vertical"));
        //  this.gameObject.transform.Translate(Vector3.right * speed * Input.GetAxis("Horizontal"));
        //  this.gameObject.transform.Translate(Vector3.up * speed * (Input.GetAxis("Jump") + (Input.GetAxis("Fire1") * -1)));

        float inputMX = Input.GetAxis("Mouse X");
        float inputMY = Input.GetAxis("Mouse Y");
        if (check(inputMX, HorizontalSensivity) || check(inputMY, VerticalSensivity))
        {
            this.gameObject.transform.Rotate(inputMY * mouseSensitivity * ((invertMouse) ? 1 : -1), inputMX * mouseSensitivity * ((invertMouse) ? -1 : 1), 0);
           
        }
      //  this.gameObject.transform.localEulerAngles = new Vector3(this.gameObject.transform.localEulerAngles.x, this.gameObject.transform.localEulerAngles.y, 0);
 Input.ResetInputAxes();
    }

    private bool check(float value, float sensivity)
    {
        return value < -sensivity || value > sensivity;
    }
}