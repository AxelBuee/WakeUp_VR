using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class Globe : MonoBehaviour
{
    [FormerlySerializedAs("smashedObject")]
    public GameObject SmashedObject;
    [FormerlySerializedAs("HiddenKey")]
    public GameObject HiddenKey;

    [Header("Audio")]
    public AudioClip[] BreakingAudioClips;

    Rigidbody m_RbPotion;
    bool m_Breakable;
    int m_UniqueId;
    static int NextFreeUniqueId = 3000;

    void OnEnable()
    {
        m_RbPotion = GetComponent<Rigidbody>();
        m_Breakable = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_UniqueId = NextFreeUniqueId++;
    }

    public void ToggleBreakable(bool breakable)
    {
        m_Breakable = breakable;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_Breakable && m_RbPotion.velocity.magnitude > 1.15)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            HiddenKey.SetActive(true);
            HiddenKey.GetComponentInChildren<BoxCollider>().enabled = true;
            HiddenKey.GetComponent<Rigidbody>().isKinematic = false;
            HiddenKey.GetComponent<Rigidbody>().useGravity = true;
            SmashedObject.SetActive(true);

            SFXPlayer.Instance.PlaySFX(BreakingAudioClips[Random.Range(0, BreakingAudioClips.Length)], transform.position, new SFXPlayer.PlayParameters()
            {
                Pitch = Random.Range(0.8f, 1.2f),
                SourceID = m_UniqueId,
                Volume = 1.0f
            });

            Rigidbody[] rbs = SmashedObject.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.AddExplosionForce(100.0f, SmashedObject.transform.position, 2.0f, 15.0F);
            }

            Destroy(SmashedObject, 4.0f);
            Destroy(this);
        }
    }
}
