using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : Interactable
{
    public Door door;

    public NodePuzzle puzzle;
    
    public float gapSize;

    public Vector3 nodeSize;

    Transform startPos;

    GameObject[,] displayPuzzle;

    Material prevMat;

    private int posX, posY;


    private void Awake()
    {
        isWorking = false;
        canTake = false;

        startPos = transform.Find("startPos");
        camPos = transform.Find("camPos");
    }


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
        {
            CheckInput();

            if (Input.GetMouseButtonDown(1))
            {
                TerminalController.controllerChange = false;
                isWorking = false;

                if (isCompleted())
                    door.isLocked = false;
                else
                    door.isLocked = true;
            }
        }
    }

    public override void Interact(Transform playerCam)
    {
        StartCoroutine(WaitMouse(playerCam));
    }
    
    IEnumerator WaitMouse(Transform playerCam)
    {
        yield return new WaitForFixedUpdate();
        base.Interact(playerCam);
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            MovePointer(0, 1);
        else if (Input.GetKeyDown(KeyCode.A))
            MovePointer(-1, 0);
        else if (Input.GetKeyDown(KeyCode.S))
            MovePointer(0, -1);
        else if (Input.GetKeyDown(KeyCode.D))
            MovePointer(1, 0);

        if (Input.GetKeyDown(KeyCode.Space))
            AddPoint();
        else if (Input.GetKeyDown(KeyCode.R))
            ResetNodes();
    }

    int ConnectPuzzle(dynamic empty)
    {
        base.Interact(Camera.main.transform);
        TerminalController.controllerChange = true;
        return 0;
    }

    void MovePointer(int plusX, int plusY)
    {
        int x = posX + plusX;
        int y = posY + plusY;

        if ((x > -1 && x < puzzle.puzzleSize) && (y > -1 && y < puzzle.puzzleSize))
        {
            Renderer renderer = displayPuzzle[x, y].GetComponent<Renderer>();

            if (renderer.sharedMaterial == puzzle.incorrect)
            {
                Renderer oldRenderer = displayPuzzle[posX, posY].GetComponent<Renderer>();

                oldRenderer.sharedMaterial = prevMat;
                prevMat = renderer.sharedMaterial;
                renderer.sharedMaterial = puzzle.selected;

                posX = x;
                posY = y;
            }
        }
    }

    void CreatePuzzle()
    {
        displayPuzzle = new GameObject[puzzle.puzzleSize, puzzle.puzzleSize];

        for (int y = 0; y < puzzle.puzzleSize; y++)
        {
            for (int x = 0; x < puzzle.puzzleSize; x++)
            {
                Vector3 pos = new Vector3(startPos.position.x + (x * gapSize),
                    startPos.position.y + (y * gapSize),
                    startPos.position.z);

                GameObject clone = Instantiate(puzzle.puzzlePiece,
                    pos, startPos.rotation, null);

                clone.transform.parent = this.transform;

                clone.transform.localScale = nodeSize;

                Renderer cloneRenderer = clone.GetComponent<Renderer>();
                cloneRenderer.sharedMaterial = puzzle.incorrect;

                displayPuzzle[x, y] = clone;
            }
        }

        prevMat = puzzle.incorrect;
        ResetNodes();
    }
    
    void AddPoint()
    {
        displayPuzzle[posX, posY].GetComponent<Renderer>().sharedMaterial = puzzle.correct;
        prevMat = puzzle.correct;
    }

    bool isCompleted()
    {
        for (int y = 0; y < puzzle.puzzleSize; y++)
        {
            for (int x = 0; x < puzzle.puzzleSize; x++)
            {
                Material material = displayPuzzle[x, y].GetComponent<Renderer>().sharedMaterial;
                
                if (material != puzzle.nodes[x, y])
                {
                    if (puzzle.nodes[x, y] != prevMat)
                        return false;
                }
            }
        }
        return true;
    }

    void ResetNodes()
    {
        for (int y = 0; y < puzzle.puzzleSize; y++)
            for (int x = 0; x < puzzle.puzzleSize; x++)
                displayPuzzle[x, y].GetComponent<Renderer>().sharedMaterial = puzzle.incorrect;

        displayPuzzle[0, 0].GetComponent<Renderer>().sharedMaterial = puzzle.selected;
        prevMat = puzzle.incorrect;

        posX = 0;
        posY = 0;
    }
}
