using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : Interactable
{
    #region Values
    float horiz, vert;
    Vector3 turnDirection;

    Rigidbody rb;

    [SerializeField] float speed;
    #endregion

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void Start()
    {
        Functions.instance.AddFunction("connectCam", ConnectCam);
        Functions.instance.AddFunction("resetCamPos", ResetCam);
    }

    public override void Interact() => ConnectCam(null);

    public int ConnectCam(dynamic empty)
    {
        TerminalController.controllerChange = true;
        isWorking = true;
        return 0;
    }

    public int ResetCam(dynamic empty)
    {
        this.gameObject.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        return 0;
    }

    private void FixedUpdate()
    {
        if (!isWorking)
            return;

        else if (Input.GetMouseButtonDown(1))
            CamExit();

        if (gameObject.transform.rotation.eulerAngles.x > 45 &&
            gameObject.transform.rotation.eulerAngles.x < 340)
            return;

        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        turnDirection = new Vector3(0f, horiz, 0f);

        Quaternion deltaRotation = Quaternion.Euler(turnDirection * speed * Time.deltaTime);

        rb.MovePosition(transform.position + transform.right * vert * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void CamExit()
    {
        TerminalController.controllerChange = false;
        isWorking = false;
    }
}
