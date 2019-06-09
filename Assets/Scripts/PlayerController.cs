using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    Vector3 moveDirection;

    CharacterController charController;

    public GameObject handObj;
    public Vector3 handPos;
    public Quaternion handRot;
    public Transform dropPos;

    PlayerLook pLook;

    public Camera playerCam;
    Transform camPos;
    [HideInInspector] public AudioSource audioSrc;

    float horizontal, vertical;

    public float playerSpeed = 2f;
    public float playerWalkSpeed = 1f;
    public float playerRunSpeed = 2f;
    public float jumpForce = 2f;
    public float gravity = 10f;

    public bool usingTerminal;
    public bool isAnimating;
    public bool hasObj;

    bool isCrouching = false;


    void Awake()
    {
        camPos = gameObject.transform.Find("CamPos").GetComponent<Transform>();
        Transform hand = gameObject.transform.Find("HandPos");
        handPos = hand.localPosition;
        handRot = hand.localRotation;
        audioSrc = GetComponent<AudioSource>();

        pLook = GetComponentInChildren<PlayerLook>();
        charController = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        usingTerminal = false;
        isAnimating = false;
        hasObj = false;
    }

    void Update()
    {
        if (!isAnimating)
        {
            pLook.Look();
            MoveInput();
            CrouchInput();
            if (Input.GetMouseButtonDown(1))
            {
                DropObject();
            }
        }
        else if (audioSrc.isPlaying)
            audioSrc.Stop();
    }


    void MoveInput()
    {
        if (charController.isGrounded)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            moveDirection = new Vector3(horizontal, 0f, vertical);
            moveDirection = transform.TransformDirection(moveDirection);
            playerSpeed = playerWalkSpeed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpForce;
            }

            if (Input.GetButton("Run"))
            {
                audioSrc.pitch = 1.6f;
                playerSpeed = playerRunSpeed;
            }
            else
            {
                audioSrc.pitch = 1f;
            }
            moveDirection = moveDirection * playerSpeed;

            if (moveDirection == Vector3.zero)
            {
                audioSrc.Stop();
                audioSrc.loop = false;
            }
            else
            {
                if (audioSrc.loop == false)
                {
                    audioSrc.loop = true;
                    audioSrc.Play();
                }
            }
        }

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        charController.Move(moveDirection * Time.deltaTime);
    }

    void CrouchInput()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                charController.height = 1.5f;
                camPos.localPosition = new Vector3(camPos.localPosition.x, 0.4f, camPos.localPosition.z);
            }

            else
            {
                charController.height = 2f;
                camPos.localPosition = new Vector3(camPos.localPosition.x, 0.8f, camPos.localPosition.z);
            }

            pLook.isCrouched(isCrouching);
        }
    }

    public void HoldObject(GameObject item)
    {
        if (item != null && !hasObj)
        {
            hasObj = true;
            handObj = item;
            handObj.GetComponent<Rigidbody>().isKinematic = true;
            handObj.layer = LayerMask.NameToLayer("IgnoreInteract");
            handObj.transform.parent = transform;
            handObj.transform.localPosition = handPos;
            handObj.transform.localRotation = handRot;
        }
    }

    public void DropObject()
    {
        if (hasObj)
        {
            handObj.transform.position = dropPos.position;
            handObj.transform.parent = null;
            handObj.layer = LayerMask.NameToLayer("Interactable");
            handObj.GetComponent<Rigidbody>().isKinematic = false;
            hasObj = false;
        }
    }

    public void SetPlayerPos(Transform playerPos)
    {
        moveDirection.Set(0f, 0f, 0f);
        audioSrc.loop = false;
        isAnimating = true;
        if (pLook.isZoomed == true)
            playerCam.fieldOfView = 60f;
        StartCoroutine(SetPos(playerPos));
    }

    public void StartTerminal()
    {
        usingTerminal = true;
    }

    public void ExitTerminal()
    {
        usingTerminal = false;
        StartCoroutine(SetPos());
    }

    public void SetCharController()
    {
        charController.enabled = !charController.enabled;
    }

    public Quaternion SetCamera()
    {
        return camPos.localRotation;
    }

    public IEnumerator SetPos()
    {
        Transform pos = camPos;
        StartCoroutine(SetPos(pos));
        yield break;
    }

    public IEnumerator SetPos(Transform posDestination)
    {

        Vector3 setPos = posDestination.position;
        Quaternion setRot = posDestination.rotation;

        float multiplier = 6f;
        float time = 0f;
        do
        {
            time += Time.deltaTime;
            playerCam.transform.position = Vector3.Slerp(playerCam.transform.position, setPos, Time.deltaTime * multiplier);
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, setRot, Time.deltaTime * multiplier * 2);
            if (time > 1.2f)
                break;
            yield return null;
        } while (playerCam.transform.position.x != posDestination.position.x
        && playerCam.transform.position.y != posDestination.position.y
        && playerCam.transform.position.z != posDestination.position.z);

        if (posDestination == camPos)
        {
            usingTerminal = false;
            isAnimating = false;
        }


        yield break;
    }

}
