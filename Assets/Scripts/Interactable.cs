using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Transform camPos;

    protected bool isWorking = false;
    
    public virtual void Interact() => isWorking = true;

    public virtual void Interact(Transform playerCam)
    {
        isWorking = true;
        SetCamera(playerCam);
    }

    protected void SetCamera(Transform _camPos) => _camPos.SetPositionAndRotation(camPos.position, camPos.rotation);

}
