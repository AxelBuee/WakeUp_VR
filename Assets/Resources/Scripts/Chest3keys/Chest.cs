using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public static int NbKeys;
    private bool isOpened;

    private AudioSource audioData;

    void Awake() {
        NbKeys = 0;
        isOpened = false;
        audioData = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NbKeys >= 3 && !isOpened) {
            this.gameObject.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
            
            this.gameObject.transform.GetChild(0).GetComponent<HingeJoint>().useMotor = true;
            audioData.Play(0);
            StartCoroutine(OffMotor());
            isOpened = true;
        }
    }

    IEnumerator OffMotor() {
        yield return new WaitForSeconds(.5f);

        this.gameObject.transform.GetChild(0).GetComponent<HingeJoint>().useMotor = false;
    }
}
