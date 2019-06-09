using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
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
    }
}
