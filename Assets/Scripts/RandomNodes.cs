using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNodes : MonoBehaviour
{
    public int row, column;
    public Material greenMat;
    public bool[,] pos;

    GameObject[,] nodes;

    private void Start()
    {
        pos = new bool[row,column];
        nodes = new GameObject[row, column];
        SetPuzzle();
    }

    void SetPuzzle()
    {
        int count = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                pos[i, j] = true;
                nodes[i, j] = this.gameObject.transform.GetChild(count).gameObject;
                if (Random.Range(0, 2) > 0)
                {
                    pos[i, j] = false;
                    nodes[i, j].GetComponent<MeshRenderer>().material = greenMat;
                }
                count++;
            }
        }
    }
}
