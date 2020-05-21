using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSocleCube : MonoBehaviour
{
    public GameObject key;
    public GameObject cubeWlocked;
    public GameObject cubeAlocked;
    public GameObject cubeKlocked;
    public GameObject cubeElocked;
    public GameObject cubeUlocked;
    public GameObject cubePlocked;
    public GameObject cubeIlocked;
    public GameObject socleCube;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void KeyAppears()
    {
        /*
        cubeWlocked.SetActive(false);
        cubeAlocked.SetActive(false);
        cubeKlocked.SetActive(false);
        cubeElocked.SetActive(false);
        cubeUlocked.SetActive(false);
        cubePlocked.SetActive(false);
        cubeIlocked.SetActive(false);*/
        socleCube.SetActive(false);
        key.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            GameObject cube = other.gameObject;
            if (cube.name == "cubeW")
            {
                cube.SetActive(false);
                cubeWlocked.SetActive(true);
                socleCube.GetComponent<ObjectCount>().lockedPuzzleCount++;
            }
            else if (cube.name == "cubeA")
            {
                cube.SetActive(false);
                cubeAlocked.SetActive(true);
                socleCube.GetComponent<ObjectCount>().lockedPuzzleCount++;
            }
            else if (cube.name == "cubeK")
            {
                cube.SetActive(false);
                cubeKlocked.SetActive(true);
                socleCube.GetComponent<ObjectCount>().lockedPuzzleCount++;
            }
            else if (cube.name == "cubeE")
            {
                cube.SetActive(false);
                cubeElocked.SetActive(true);
                socleCube.GetComponent<ObjectCount>().lockedPuzzleCount++;
            }
            else if (cube.name == "cubeU")
            {
                cube.SetActive(false);
                cubeUlocked.SetActive(true);
                socleCube.GetComponent<ObjectCount>().lockedPuzzleCount++;
            }
            else if (cube.name == "cubeP")
            {
                cube.SetActive(false);
                cubePlocked.SetActive(true);
                socleCube.GetComponent<ObjectCount>().lockedPuzzleCount++;
            }
            else if (cube.name == "cube!")
            {
                cube.SetActive(false);
                cubeIlocked.SetActive(true);
                socleCube.GetComponent<ObjectCount>().lockedPuzzleCount++;
            }
            if(socleCube.GetComponent<ObjectCount>().lockedPuzzleCount == 7)
            {
                KeyAppears();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
