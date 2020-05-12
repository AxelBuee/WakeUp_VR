using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public static int NbKeys;

    void Awake() {
        NbKeys = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (NbKeys >= 3) {
            this.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
        }*/
    }
}
