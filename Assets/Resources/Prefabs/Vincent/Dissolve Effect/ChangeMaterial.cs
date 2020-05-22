using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    float timer = 0;
    public Material newMat;
    bool isDissolving;
    string DissolveState;//Vector1_E7C781E4
    // Start is called before the first frame update
    void Start()
    {
        isDissolving = false;
        DissolveState = "Vector1_E7C781E4";
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDissolving && timer <= 3)
            timer += Time.deltaTime;
        else if (timer >= 3 && !isDissolving)
        {
            Debug.LogError("diss state " + this.gameObject.GetComponent<MeshRenderer>().material.HasProperty(DissolveState));
            this.gameObject.GetComponent<MeshRenderer>().material = newMat;
           isDissolving = true;
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().material.SetFloat(DissolveState, (0.1f * Time.deltaTime) + this.gameObject.GetComponent<MeshRenderer>().material.GetFloat(DissolveState));
        }
    }
}
