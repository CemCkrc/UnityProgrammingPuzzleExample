using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : Interactable
{
    #region Values
    Camera cam, playerCam;
    RenderTexture targetText;
    
    float horiz, vert;
    Vector3 turnDirection;

    Rigidbody rb;

    [SerializeField] float speed;
    [SerializeField] float rayLength;

    bool connected = false;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        playerCam = Camera.main;

        targetText = cam.targetTexture;

        cam.depth = -1;

        Functions.instance.AddFunction("connectCam", ConnectCam);
        Functions.instance.AddFunction("resetCamPos", ResetCam);
    }

    public override void Interact()
    {
        ConnectCam(null);
    }

    public int ConnectCam(dynamic empty)
    {
        playerCam.depth = -1;
        cam.depth = 0;

        isWorking = true;
        cam.targetTexture = null;

        TerminalController.camChanged = true;
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

        if (Input.GetMouseButtonDown(0) && !connected)
            CheckOtherCam();
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
        cam.depth = -1;
        playerCam.depth = 0;
        cam.targetTexture = targetText;

        TerminalController.camChanged = false;
        connected = false;
        isWorking = false;
    }

    void CheckOtherCam()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Interactable")))
        {
            if(hit.collider.tag == "Camera")
            {
                cam.depth = -1;
                cam.targetTexture = targetText;
                connected = true;
                hit.collider.GetComponent<Interactable>().Interact(playerCam.transform);
            }
        }
    }

    /* AudioSource camAudio;
     Rigidbody rb;
     Camera cam;

     public bool isWorking;

     public float speed;
     float horiz, vert;

     Vector3 turnDirection;

     RenderTexture textureHolder;


     private void Awake()
     {
         cam = GetComponentInChildren<Camera>();
         camAudio = GetComponent<AudioSource>();
         rb = GetComponent<Rigidbody>();
     }

     private void Start()
     {
         GetComponentInChildren<AudioListener>().enabled = false;

         textureHolder = cam.targetTexture;
         isWorking = false;

         TerminalController.AddFunction("Connect", SetController);
         TerminalController.AddFunction("ResetPos", ResetRot);
     }

     private void FixedUpdate()
     {
         if(isWorking)
         {
             if(Input.GetMouseButtonDown(2))
             {
                 SetController();
             }

             if(gameObject.transform.rotation.eulerAngles.x > 45 && gameObject.transform.rotation.eulerAngles.x < 340)
             {
                 return;
             }

             horiz = Input.GetAxis("Horizontal");
             vert = Input.GetAxis("Vertical");

             turnDirection = new Vector3(0f, horiz, 0f);

             Quaternion deltaRotation = Quaternion.Euler(turnDirection * speed * Time.deltaTime);

             rb.MovePosition(transform.position + transform.right * vert * Time.deltaTime);
             rb.MoveRotation(rb.rotation * deltaRotation);
         }
     }

     public void SetController()
     {
         GameObject player = GameObject.FindGameObjectWithTag("Player");
         GameObject terminal = GameObject.FindGameObjectWithTag("Terminal");

         isWorking = !isWorking;

         if (isWorking)
         {
             GetComponentInChildren<AudioListener>().enabled = true;
             cam.targetTexture = null;
             player.GetComponentInChildren<AudioListener>().enabled = false;
             player.GetComponentInChildren<Camera>().depth = -1;
             terminal.GetComponent<TerminalController>().isWorking = false;
         }
         else
         {
             GetComponentInChildren<AudioListener>().enabled = false;
             cam.targetTexture = textureHolder;
             player.GetComponentInChildren<AudioListener>().enabled = true;
             player.GetComponentInChildren<Camera>().depth = 1;
             terminal.GetComponent<TerminalController>().isWorking = true;
         }
     }

     public void ResetRot()
     {
         this.gameObject.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
     }*/
}
