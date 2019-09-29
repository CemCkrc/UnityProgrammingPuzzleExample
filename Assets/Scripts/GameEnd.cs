using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    //Restart Game
    public static void End()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            End();
    }
}
