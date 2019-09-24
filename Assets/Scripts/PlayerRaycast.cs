using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    #region Values
    [Range(1, 4)]
    public float rayLength;

    protected bool isWorking = true;

    Camera playerCam;

    #endregion

    private void Awake() => playerCam = GetComponent<Camera>();


    public GameObject Take()
    {
        if (!isWorking)
            return null;

        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Interactable")))
                return hit.collider.gameObject;
        return null;
    }

    public bool Hit()
    {
        if (!isWorking)
            return false;

        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Interactable")))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            //InteractableItem interactableItem = hit.collider.GetComponent<InteractableItem>();

            if (interactable)
            {
                interactable.Interact(transform);
                isWorking = false;
                return true;
            }
            else if(interactable)
            {
                //interactable.Interact();
                return true;
            }
        }
        return false;
    }


    /*
    public float rayLength;

    
    PlayerController pC;

    void Awake()
    {
        pC = GetComponentInParent<PlayerController>();
    }

    void Start()
    {
        rayLength = 2f;
    }

    void Update()
    {
        if (pC.isAnimating)
            return;

        if (Time.timeScale == 0)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Interactable")))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.tag == "Item")
                {
                        pC.HoldObject(hit.collider.gameObject);
                }
                
                else if (hit.collider.tag == "Terminal")
                {
                    if(!pC.isAnimating)
                    {
                        pC.HoldObject(hit.collider.gameObject);
                    }
                }

                else if(hit.collider.tag == "Button")
                {
                    hit.collider.GetComponent<ButtonController>().CheckPuzzle();
                }
            }
            if(Input.GetMouseButtonDown(1))
            {
                if (hit.collider.tag == "Terminal")
                {
                    if (!pC.isAnimating)
                    {
                        hit.collider.gameObject.GetComponent<TerminalController>().StartTer();
                        pC.SetPlayerPos(hit.collider.gameObject.GetComponent<TerminalController>().terminalPos);
                        pC.StartTerminal();
                    }
                }

                else if (hit.collider.tag == "NodePuzzle")
                {
                    if (!pC.isAnimating)
                    {
                        hit.collider.gameObject.GetComponent<NodePuzzle>().SetController();
                    }
                }
            }
        }
    }*/
}
