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
    public GameObject Soclecount;
    //int lockedPuzzleCount = 0;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            GameObject puzzlepiece = other.gameObject;
            if (puzzlepiece.name == "piecePuzzle1")
            {
                Destroy(puzzlepiece);
                piecePuzzle1locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle2")
            {
                Destroy(puzzlepiece);
                piecePuzzle2locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle3")
            {
                Destroy(puzzlepiece);
                piecePuzzle3locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle4")
            {
                Destroy(puzzlepiece);
                piecePuzzle4locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle5")
            {
                Destroy(puzzlepiece);
                piecePuzzle5locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            if (Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount == 5)
            {
                OpenChest();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
