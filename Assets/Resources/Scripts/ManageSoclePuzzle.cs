using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSoclePuzzle : MonoBehaviour
{
    public GameObject piecePuzzle1locked;
    public GameObject piecePuzzle2locked;
    public GameObject piecePuzzle3locked;
    public GameObject piecePuzzle4locked;
    public GameObject piecePuzzle5locked;
    public GameObject Couvercle;
    int lockedPuzzleCount = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenChest()
    {
        Destroy(piecePuzzle1locked);
        Destroy(piecePuzzle1locked);
        Destroy(piecePuzzle1locked);
        Destroy(piecePuzzle1locked);
        Destroy(piecePuzzle1locked);
        Destroy(Couvercle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 14)
        {
            GameObject puzzlepiece = collision.gameObject;
            Debug.Log(puzzlepiece.name);
            if (puzzlepiece.name == "piecePuzzle1")
            {
                Destroy(puzzlepiece);
                piecePuzzle1locked.SetActive(true);
                lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle2")
            {
                Destroy(puzzlepiece);
                piecePuzzle2locked.SetActive(true);
                lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle3")
            {
                Destroy(puzzlepiece);
                piecePuzzle3locked.SetActive(true);
                lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle4")
            {
                Destroy(puzzlepiece);
                piecePuzzle4locked.SetActive(true);
                lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle5")
            {
                Destroy(puzzlepiece);
                piecePuzzle5locked.SetActive(true);
                lockedPuzzleCount++;
                Debug.Log("GOOD");
            }
            if(lockedPuzzleCount == 5)
            {
                OpenChest();
            }
        }
    }
}
