using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    #region Values
    public float mouseSensitivity = 60f;

    float verLook, horizLook;
    float clampVal;

    GameObject camPos;

    [Range(1, 4)]
    public float rayLength, dropLength;

    private static bool isWorking = true;
    Camera playerCam;
    #endregion

    private void Awake()
    {
        playerCam = GetComponent<Camera>();

        camPos = new GameObject();
        camPos.transform.SetPositionAndRotation(transform.position, transform.rotation);
        camPos.transform.SetParent(transform.parent);

        clampVal = 0f;
    }

    private void Start() => SetCursor.SetCursorState(CursorLockMode.Locked, false);

    public void Look()
    {
        verLook = Input.GetAxis("Mouse Y") * mouseSensitivity;
        horizLook = Input.GetAxis("Mouse X") * mouseSensitivity;

        clampVal += verLook;

        if (clampVal > 70.0f)
        {
            clampVal = 70.0f;
            verLook = 0.0f;
        }
        else if (clampVal < -80.0f)
        {
            clampVal = -80.0f;
            verLook = 0.0f;
        }

        transform.Rotate(Vector3.left * verLook);
        transform.parent.Rotate(Vector3.up * horizLook);
    }

    public GameObject Take()
    {
        if (!isWorking)
            return null;

        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Interactable")))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable)
            {
                if (interactable.canTake)
                    return hit.collider.gameObject;
            }
            else
                return hit.collider.gameObject;
        }
        return null;
    }

    public bool Hit()
    {
        if (!isWorking)
        {
            transform.SetPositionAndRotation(camPos.transform.position, camPos.transform.rotation);
            isWorking = true;
            return false;
        }

        camPos.transform.SetPositionAndRotation(transform.position, transform.rotation);

        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, LayerMask.GetMask("Interactable")))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable)
            {
                interactable.Interact(transform);
                isWorking = false;
                return true;
            }
        }
        return false;
    }
}

//Set Cursor Mode
public static class SetCursor
{
    /// <summary>
    /// Set Cursor lock mode and visibility
    /// </summary>
    /// <param name="mode">Cursor Lock Mode</param>
    /// <param name="isVisible">Set Cursor Visible</param>
    public static void SetCursorState(CursorLockMode mode, bool isVisible)
    {
        Cursor.lockState = mode;
        Cursor.visible = isVisible;
    }
}