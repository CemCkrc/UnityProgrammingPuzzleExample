using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public bool canLook = true;


    public float bobbingSpeed = 0.1f;
    public float bobbingHeight = 0.2f;

    private float timer = 0.0f;

    public float minHigh = 0.6f;
    public float maxHigh = 0.7f;

    [Range(1f, 3f)]
    public float freakness = 1f;
    public float breathingTime;
    public float movement = 0.6f;
    private bool highBreath = false;
    public bool crouch = false;

    private bool isZoomed = false;
    private int zoom = 30; 
    private int normal = 60;
    public float smooth = 5f;

    public float mouseSensitivity;
    private Transform playerBody;
    private Camera playerCam;

    private float xAxisClamp;

    //Initialize variables before game starts
    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;
        playerBody = GameObject.FindGameObjectWithTag("Player").transform;
        playerCam = GetComponent<Camera>();
    }

    //Lock cursor (useful)
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (canLook)
        {
            CameraZoom();
            CameraRotation();
            HeadBobbing();
        }
    }

    //Right click to zoom or default
    private void CameraZoom()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isZoomed = !isZoomed;
        }

        if(isZoomed && playerCam.fieldOfView != zoom)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, zoom, Time.deltaTime * smooth);
        }

        if (!isZoomed && playerCam.fieldOfView != normal)
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, normal, Time.deltaTime * smooth);
        }
    }

    //Player looks
    private void CameraRotation()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -80.0f)
        {
            xAxisClamp = -80.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(80.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    //Limit the view of the player camera
    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    //Set player camera (Needs to fix!) 
    public void CrouchSet(bool setVal)
    {
        if (setVal == true)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0.4f, transform.localPosition.z);
            minHigh = 0.3f;
            maxHigh = 0.4f;
            movement = 0.4f;

        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0.6f, transform.localPosition.z);
            minHigh = 0.6f;
            maxHigh = 0.7f;
            movement = 0.6f;
        }
    }

    //If player is Idle
    public void Breathing()
    {
        if (highBreath)
        {
            movement = Mathf.Lerp(movement, maxHigh, Time.deltaTime * freakness);
            transform.localPosition = new Vector3(transform.localPosition.x, movement, transform.localPosition.z);
            if (movement >= maxHigh - 0.01f)
            {
                highBreath = !highBreath;
            }
        }
        else
        {
            movement = Mathf.Lerp(movement, minHigh, Time.deltaTime * freakness);
            transform.localPosition = new Vector3(transform.localPosition.x, movement, transform.localPosition.z);
            if (movement <= minHigh + 0.01f)
            {
                highBreath = !highBreath;
            }
        }

        if (freakness != 0)
        {
            freakness = Mathf.Lerp(freakness, 1f, Time.deltaTime * 0.2f);
        }
    }

    //If player is moving
    private void HeadBobbing()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cSharpConversion = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingHeight;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            cSharpConversion.y = movement + translateChange;
        }
        else
        {
            cSharpConversion.y = movement;
        }

        transform.localPosition = cSharpConversion;
    }

}
