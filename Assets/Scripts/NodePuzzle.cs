using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodePuzzle : MonoBehaviour
{
    public int row, column;
    public Transform puzzlePos;
    GameObject[,] nodes;
    
    int pointerPosX, pointerPosY;

    GameObject posObj;

    [HideInInspector] public bool[,] avaliablePos;
    
    void Start()
    {
        nodes = new GameObject[row, column];
        avaliablePos = new bool[row, column];
        FillNodes();
    }
    

    void StartController(NodeController nodeController)
    {
        nodeController.puzzle = this;
        nodeController.isWorking = true;
    }

    public void SetController()
    {
        PlayerController pC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        NodeController nodeController = GameObject.Find("NodeController").GetComponent<NodeController>();

        if (nodeController == null)
        {
            Debug.LogError("NodeController is missing");
            return;
        }

        if (nodeController.isWorking)
        {
            StartCoroutine(pC.SetPos());
            nodeController.isWorking = false;
        }
        else
        {
            pC.isAnimating = true;
            StartCoroutine(pC.SetPos(puzzlePos));
            StartController(nodeController);
        }
    }

    void FillNodes()
    {
        int count = 0;

        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                nodes[i,j] =  this.gameObject.transform.GetChild(count).gameObject;
                nodes[i, j].GetComponent<Image>().color = Color.white;
                avaliablePos[i, j] = true;
                count++;
            }
        }

        pointerPosX = 0;
        pointerPosY = 0;
        nodes[0, 0].GetComponent<Image>().color = Color.red;
    }
    

    public void MovePointer(int x, int y)
    {
        int newPosX = pointerPosX + x;
        int newPosY = pointerPosY + y;

        if (newPosX == row || newPosX < 0)
            return;
        if (newPosY == column || newPosY < 0)
            return;
        
        if (avaliablePos[newPosX, newPosY])
        {
            if(avaliablePos[pointerPosX, pointerPosY])
            {
                nodes[pointerPosX, pointerPosY].GetComponent<Image>().color = Color.white;
            }
            nodes[newPosX, newPosY].GetComponent<Image>().color = Color.red;

            pointerPosX = newPosX;
            pointerPosY = newPosY;
        }
    }

    public void AddPoint()
    {
        nodes[pointerPosX, pointerPosY].GetComponent<Image>().color = Color.green;
        avaliablePos[pointerPosX, pointerPosY] = false;
    }

    public void ResetNodes()
    {
        FillNodes();
    }
}
