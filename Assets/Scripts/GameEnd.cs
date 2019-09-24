using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    public static void End()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
