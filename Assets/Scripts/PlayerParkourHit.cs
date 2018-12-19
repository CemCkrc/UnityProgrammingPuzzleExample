using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParkourHit : MonoBehaviour {

    PlayerController playerC;
    [SerializeField] Camera parkourCam;
    [SerializeField] private float rayLength = 0.1f;

    void Awake()
    {
        playerC = GetComponentInParent<PlayerController>();
        parkourCam = GetComponent<Camera>();
    }

    void Update()
    {

        RaycastHit hit;
        Ray ray = parkourCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider && playerC.isSliding)
            {
                playerC.isSliding = false;
                playerC.ParAnim.Stop();
            }
        }
    }
}
