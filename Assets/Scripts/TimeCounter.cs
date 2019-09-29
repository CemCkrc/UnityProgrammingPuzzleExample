using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Decrease time using deltaTime
//Restart scene if time is less than zero
public class TimeCounter : MonoBehaviour
{
    TextMesh mesh;
    Transform player;

    private float timeLeft = 200;

    private void Awake() => mesh = GetComponentInChildren<TextMesh>();

    private void Start() => player = GameObject.FindObjectOfType<PlayerController>().transform;

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            GameEnd.End();
        }
        
        mesh.text = timeLeft.ToString("F0");
        transform.LookAt(player, Vector3.up);
    }
}
