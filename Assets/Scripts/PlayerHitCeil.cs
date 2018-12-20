using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Check if player head is hitting

public class PlayerHitCeil : MonoBehaviour {

    public bool isHitCeil;

    void OnTriggerEnter(Collider col)
    {
        isHitCeil = true;
    }

    void OnTriggerStay(Collider col)
    {
        isHitCeil = true;
    }

    void OnTriggerExit(Collider col)
    {
        isHitCeil = false;
    }
}
