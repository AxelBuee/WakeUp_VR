using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activator : MonoBehaviour
{
    private bool isActivated;
    private bool canDestroyTheKey;


    void Awake() {
        isActivated = false;
        canDestroyTheKey = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActivated) {
            Chest.NbKeys += 1;
            isActivated = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (canDestroyTheKey && other.gameObject.layer == 12) {      //si il a le layer "keyChest" et canDestroyTheKey
            canDestroyTheKey = false;
            other.gameObject.SetActive(false);
   
        }
    }
}
