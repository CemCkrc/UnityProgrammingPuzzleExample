using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : Interactable
{
    public Door door;

    public NodePuzzle puzzle;

    public Transform startPos;

    Material[,] displayPuzzle;

    Material prevMat;

    private int posX, posY;

    private void Awake() => isWorking = false;

    private void Start()
    {
        CreatePuzzle();
        Functions.instance.AddFunction("connectPuzzle", ConnectPuzzle);

        posX = 0;
        posY = 0;
    }


    private void Update()
    {
        if (isWorking)
            CheckInput();
        if (Input.GetMouseButtonDown(1))
        {
            if (isCompleted())
                isWorking = false;
        }
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            MovePointer(-1, 0);
        else if (Input.GetKeyDown(KeyCode.A))
            MovePointer(0, -1);
        else if (Input.GetKeyDown(KeyCode.S))
            MovePointer(1, 0);
        else if (Input.GetKeyDown(KeyCode.D))
            MovePointer(0, 1);

        if (Input.GetKeyDown(KeyCode.Space))
            AddPoint();
        else if (Input.GetKeyDown(KeyCode.R))
            ResetNodes();
    }

    int ConnectPuzzle(dynamic empty)
    {
        Interact(Camera.main.transform);
        return 0;
    }

    bool isCompleted()
    {
        for (int y = 0; y < puzzle.puzzleSize; y++)
        {
            for (int x = 0; x < puzzle.puzzleSize; x++)
            {
                if(displayPuzzle[x,y] != puzzle.nodes[x,y])
                {
                    if(displayPuzzle[x, y] != prevMat)
                        return false;
                }
            }
        }
        return true;
    }

    void CreatePuzzle()
    {
        displayPuzzle = new Material[puzzle.puzzleSize, puzzle.puzzleSize];

        for (int y = 0; y < puzzle.puzzleSize; y++)
        {
            for (int x = 0; x < puzzle.puzzleSize; x++)
            {
                new Vector3(startPos.position.x + (x * puzzle.gapSize),
                    startPos.position.y + (y * puzzle.gapSize),
                    startPos.position.z);

                GameObject clone = Instantiate(puzzle.puzzlePiece,
                    startPos.position, startPos.rotation, this.transform);

                Renderer cloneRenderer = clone.GetComponent<Renderer>();
                cloneRenderer.material = puzzle.incorrect;

                displayPuzzle[x, y] = clone.GetComponent<Renderer>().material;
            }
        }

        prevMat = displayPuzzle[0, 0];
        displayPuzzle[0, 0] = puzzle.selected;
        ResetNodes();
    }

    void MovePointer(int plusX, int plusY)
    {
        int x = posX + plusX;
        int y = posY + plusY;

        if ((x > 0 || x < puzzle.puzzleSize) && (y > 0 || y < puzzle.puzzleSize))
        {
            if (displayPuzzle[x, y] == puzzle.incorrect)
            {
                displayPuzzle[posX, posY] = prevMat;
                prevMat = displayPuzzle[x, y];
                displayPuzzle[x, y] = puzzle.selected;

                posX = x;
                posY = y;
            }
        }
    }

    void AddPoint()
    {
        displayPuzzle[posX, posY] = puzzle.correct;
    }

    void ResetNodes()
    {
        for (int y = 0; y < puzzle.puzzleSize; y++)
            for (int x = 0; x < puzzle.puzzleSize; x++)
                displayPuzzle[x, y] = puzzle.incorrect;

        displayPuzzle[0, 0] = puzzle.selected;

        posX = 0;
        posY = 0;
    }

    /*public NodePuzzle puzzle;

    public bool isWorking = false;

    private void Update()
    {
        if(isWorking)
        {
            if(Input.GetMouseButtonDown(1))
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
    */
}
