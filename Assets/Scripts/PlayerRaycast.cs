using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour {

    PlayerController playerC;
    TerminalInteraction terminal;

    [SerializeField] Camera playerCam;
    [SerializeField] private float rayLength;

    //Initialize variables before game starts
    void Awake()
    {
        playerC = GetComponent<PlayerController>();
        terminal = null;
    }

    //Check for if camera hits something (useful)
    void Update()
    {
        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if(hit.collider)
            {
                playerC.isRunning = false;
            }

            if(hit.collider.isTrigger)
            {
                //using tags
                if (hit.collider.tag == "ClimbUp")
                {
                    playerC.canClimb = true;
                    if(!playerC.charController.isGrounded && !playerC.isAnimating)
                    {
                        playerC.isAnimating = true;
                        playerC.isJumping = false;
                        StartCoroutine(playerC.AnimationEvent("ClimbUp", 2.0f));
                    }
                }
                if (hit.collider.tag == "HalfClimbUp")
                {
                    playerC.canHalfClimbForwad = true;
                    if (playerC.isJumping)
                    {
                        StartCoroutine(playerC.AnimationEvent("HalfClimbUp", 1.8f));
                    }
                }
                if(hit.collider.tag == "Terminal")
                {
                    if(Input.GetButtonDown("Interaction"))
                    {
                        terminal = hit.collider.GetComponent<TerminalInteraction>();
                        if (terminal.isActiveted == false && !playerC.isAnimating)
                        {
                            Transform pos = hit.collider.transform.parent.Find("TerminalZone");
                            StartCoroutine(playerC.TerminalEvent(pos,false));
                            terminal.isActiveted = true;
                            playerC.SetTerminal(false);
                        }
                    }
                    if(terminal != null && terminal.wantExit)
                    {
                        terminal.wantExit = false;
                        terminal.Exit();
                        StartCoroutine(playerC.TerminalEvent(playerC.regularCam.transform, true));
                        playerC.SetTerminal(true);
                    }
                }
            }
        }
        else if(playerC.canClimb == true)
        {
            playerC.canClimb = false;
        }
        else if (playerC.canHalfClimbForwad == true)
        {
            playerC.canHalfClimbForwad = false;
        }
    }
}
