using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    AudioSource camAudio;
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
        isWorking = !isWorking;

        if (isWorking)
        {
            GetComponentInChildren<AudioListener>().enabled = true;
            cam.targetTexture = null;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AudioListener>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().depth = -1;
            GameObject.FindGameObjectWithTag("Terminal").GetComponent<TerminalController>().isWorking = false;
        }
        else
        {
            GetComponentInChildren<AudioListener>().enabled = false;
            cam.targetTexture = textureHolder;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AudioListener>().enabled = true;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().depth = 1;
            GameObject.FindGameObjectWithTag("Terminal").GetComponent<TerminalController>().isWorking = true;
        }
    }

    public void ResetRot()
    {
        this.gameObject.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }
}
