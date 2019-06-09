using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public NodePuzzle puzzle;

    public bool isWorking = false;
    
    private void Update()
    {
        if(isWorking)
        {
            if(Input.GetMouseButtonDown(0))
            {
                puzzle.SetController();
                return;
            }

            if(Input.GetKeyDown(KeyCode.W))
            {
                puzzle.MovePointer(-1,0);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                puzzle.MovePointer(0, -1);
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                puzzle.MovePointer(1, 0);
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                puzzle.MovePointer(0, 1);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                puzzle.AddPoint();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                puzzle.ResetNodes();
            }
        }
    }

}
