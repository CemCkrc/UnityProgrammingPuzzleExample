using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Values
    private CharacterController charController;
    private PlayerLook pLook;

    Vector3 moveDirection;
    private float horiz, vert;

    private GameObject handObj = null;
    private Transform handPos;

    private bool isCrouching = false;
    public bool isAnimating { get; private set; }

    private int gravity = 10;

    [Range(5, 12)]
    public float playerSpeed;

    #endregion

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        pLook = GetComponentInChildren<PlayerLook>();

        handPos = gameObject.transform.Find("HandPos").transform;
    }

    private void Update()
    {
        if (!isAnimating)
        {
            pLook.Look();
            MoveInput();
            CrouchInput();
        }
        Interact();
    }

    //Basic move input
    private void MoveInput()
    {
        if (charController.isGrounded)
        {
            horiz = Input.GetAxis("Horizontal");
            vert = Input.GetAxis("Vertical");

            moveDirection = new Vector3(horiz, 0f, vert) * Time.deltaTime * playerSpeed;
            moveDirection = transform.TransformDirection(moveDirection);

        }

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        charController.Move(moveDirection * Time.deltaTime);

        charController.Move(moveDirection);
    }

    //Check for crouch
    private void CrouchInput()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
            charController.height = isCrouching ? 1f : 2f;
        }
    }
    
    private void Interact()
    {
        if (Input.GetMouseButtonDown(0) && !isAnimating)
        {
            if (!handObj) TakeObj(pLook.Take());
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (pLook.Hit()) isAnimating = true;
            else if (isAnimating) isAnimating = false;
            else DropObj();
        }
    }

    private void TakeObj(GameObject obj)
    {
        if (!obj) return;
        if (handObj) return;

        handObj = obj;
        handObj.transform.SetPositionAndRotation(handPos.position, handPos.rotation);
        handObj.transform.SetParent(handPos);
        handObj.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void DropObj()
    {
        if (handObj)
        {
            handObj.GetComponent<Rigidbody>().isKinematic = false;
            handObj.transform.parent = null;
            handObj = null;
        }
    }
}