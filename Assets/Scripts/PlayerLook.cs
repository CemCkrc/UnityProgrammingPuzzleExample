using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    float verLook, horizLook;
    public float mouseSensitivity = 60f;
    private float xAxisClamp;
    [SerializeField] private Transform playerBody;

    private float timer = 0.0f;
    public float bobbingSpeed = 0.1f;
    public float bobbingHeight = 0.2f;
    public float midPoint = 0.9f;

    public bool isZoomed = false;
    private Camera cam;
    float normalFOV = 60f, zoomedFOV = 30f;

    public void isCrouched(bool crouch)
    {
        if (crouch)
            midPoint = 0.4f;
        else
            midPoint = 0.9f;
    }

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        LockCursor();
        Cursor.visible = false;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void Look()
    {
        verLook = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        horizLook = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        xAxisClamp += verLook;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            verLook = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -80.0f)
        {
            xAxisClamp = -80.0f;
            verLook = 0.0f;
            ClampXAxisRotationToValue(80.0f);
        }

        transform.Rotate(Vector3.left * verLook);
        playerBody.Rotate(Vector3.up * horizLook);
        HeadBobbing();

        if (Input.GetMouseButton(2))
        {
            cam.fieldOfView = zoomedFOV;
            isZoomed = true;
        }
        else
        {
            cam.fieldOfView = normalFOV;
            isZoomed = false;
        }
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    private void HeadBobbing()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float waveslice = 0.0f;

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
            cSharpConversion.y = midPoint + translateChange;
        }
        else
        {
            cSharpConversion.y = midPoint;
        }

        transform.localPosition = cSharpConversion;
    }
}
