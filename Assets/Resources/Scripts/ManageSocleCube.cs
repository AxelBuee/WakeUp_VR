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
    int lockedCubeCount = 0;
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
        Destroy(cubeWlocked);
        Destroy(cubeAlocked);
        Destroy(cubeKlocked);
        Destroy(cubeElocked);
        Destroy(cubeUlocked);
        Destroy(cubePlocked);
        Destroy(cubeIlocked);
        key.SetActive(true);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 13)
        {
            GameObject cube = collision.gameObject;
            if (cube.name == "cubeW")
            {
                Destroy(cube);
                cubeWlocked.SetActive(true);
                lockedCubeCount++;
            }
            else if (cube.name == "cubeA")
            {
                Destroy(cube);
                cubeAlocked.SetActive(true);
                lockedCubeCount++;
            }
            else if (cube.name == "cubeK")
            {
                Destroy(cube);
                cubeKlocked.SetActive(true);
                lockedCubeCount++;
            }
            else if (cube.name == "cubeE")
            {
                Destroy(cube);
                cubeElocked.SetActive(true);
                lockedCubeCount++;
            }
            else if (cube.name == "cubeU")
            {
                Destroy(cube);
                cubeUlocked.SetActive(true);
                lockedCubeCount++;
            }
            else if (cube.name == "cubeP")
            {
                Destroy(cube);
                cubePlocked.SetActive(true);
                lockedCubeCount++;
            }
            else if (cube.name == "cube!")
            {
                Destroy(cube);
                cubeIlocked.SetActive(true);
                lockedCubeCount++;
            }
            if(lockedCubeCount == 7)
            {
                KeyAppears();
            }
        }
    }
}
