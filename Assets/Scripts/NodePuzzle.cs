//*******************
// Generate puzzle according to the size of the puzzle
// TODO: Sometimes puzzle doesn't have any correct node
//       Add CheckPuzzle method or randomly add a correct node
//*******************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePuzzle : MonoBehaviour
{
    public Material[,] nodes { get; private set; }

    public Material selected, correct, incorrect;

    public int puzzleSize;

    public Transform startPos;
    
    public float gapSize;

    public Vector3 nodeSize;

    public GameObject puzzlePiece;

    //Set puzzle ready
    private void Awake()
    {
        nodes = new Material[puzzleSize, puzzleSize];

        for (int y = 0; y < puzzleSize; y++)
        {
            for (int x = 0; x < puzzleSize; x++)
            {
                Vector3 pos = new Vector3(startPos.position.x + (x * gapSize), 
                    startPos.position.y + (y * gapSize), 
                    startPos.position.z);

                GameObject clone = Instantiate(puzzlePiece,
                    pos, startPos.rotation, null);

                clone.transform.localScale = nodeSize;

                clone.transform.parent = this.transform;

                Renderer cloneRenderer = clone.GetComponent<Renderer>();

                // %25 chance for get correct material
                if (Random.Range(0, 4) == 0)
                    cloneRenderer.material = correct;
                else
                    cloneRenderer.material = incorrect;

                nodes[x, y] = clone.GetComponent<Renderer>().sharedMaterial;
            }
        }
    }
}
