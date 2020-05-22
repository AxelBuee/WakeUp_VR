using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveScript : MonoBehaviour
{
    public Material newMat;
    string DissolveState;//Vector1_E7C781E4
    // Start is called before the first frame update
    void Start()
    {
        DissolveState = "Vector1_E7C781E4";
        this.gameObject.GetComponent<MeshRenderer>().material = newMat;
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<MeshRenderer>().material.SetFloat(DissolveState, (0.1f * Time.deltaTime) + this.gameObject.GetComponent<MeshRenderer>().material.GetFloat(DissolveState));
        if (this.gameObject.GetComponent<MeshRenderer>().material.GetFloat(DissolveState) > 1f)
            this.gameObject.SetActive(false);
    }
}
