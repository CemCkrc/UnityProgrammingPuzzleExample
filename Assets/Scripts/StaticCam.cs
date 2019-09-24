using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCam : Interactable
{
    /*public override void Interact(Transform playerCam)
    {
        base.Interact();
        isWorking = true;
        
    }

    private void FixedUpdate()
    {
        if (isWorking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Interactable")))
                {
                    if (hit.collider.tag == "Camera")
                    {
                        GetComponentInChildren<Camera>().depth = -1;
                        isWorking = false;
                        hit.collider.GetComponent<Interactable>().Interact();
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                GetComponentInChildren<Camera>().depth = -1;
                isWorking = false;
            }
        }
    }*/
}
