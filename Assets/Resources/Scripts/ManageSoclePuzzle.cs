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
        /*
        Destroy(piecePuzzle1locked);
        Destroy(piecePuzzle2locked);
        Destroy(piecePuzzle3locked);
        Destroy(piecePuzzle4locked);
        Destroy(piecePuzzle5locked);
        Destroy(Couvercle);
        */
        piecePuzzle1locked.SetActive(false);
        piecePuzzle2locked.SetActive(false);
        piecePuzzle3locked.SetActive(false);
        piecePuzzle4locked.SetActive(false);
        piecePuzzle5locked.SetActive(false);
        Couvercle.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            GameObject puzzlepiece = other.gameObject;
            if (puzzlepiece.name == "piecePuzzle1")
            {
                //Destroy(puzzlepiece);
                puzzlepiece.SetActive(false);
                piecePuzzle1locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle2")
            {
                //Destroy(puzzlepiece);
                puzzlepiece.SetActive(false);
                piecePuzzle2locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle3")
            {
                //Destroy(puzzlepiece);
                puzzlepiece.SetActive(false);
                piecePuzzle3locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle4")
            {
                //Destroy(puzzlepiece);
                puzzlepiece.SetActive(false);
                piecePuzzle4locked.SetActive(true);
                Soclecount.GetComponent<ObjectCount>().lockedPuzzleCount++;
                //lockedPuzzleCount++;
            }
            else if (puzzlepiece.name == "piecePuzzle5")
            {
                //Destroy(puzzlepiece);
                puzzlepiece.SetActive(false);
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
