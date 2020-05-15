using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereAnimation : MonoBehaviour
{
    public GameObject sphereInGauntlet;
    public GameObject sphereOut;
    Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TriggerSphere()
    {
        MeshRenderer m = sphereInGauntlet.GetComponent<MeshRenderer>();
        m_Animator = sphereInGauntlet.GetComponent<Animator>();
        m.enabled = true;
        sphereOut.SetActive(false);
        m_Animator.SetTrigger("SphereDrop");
    }
}
