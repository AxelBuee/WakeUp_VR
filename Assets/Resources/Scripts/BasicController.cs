using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// De quoi faire deplacer l'objet qui detient le script avec les touches flechees.
/// Concu pour le rig contenant les peripheriques VR/AR, mais detournable.
/// Ne gere plus d'interactions (plus de raycasts).
/// </summary>
public class BasicController : MonoBehaviour
{
    private float mRotAroundX = 0f;
    private float mRotAroundY = 0f;
    public float mSensitivityY = 1f;
    public float mSensitivityX = 1f;

    private float mHorizontal;
    private float mVertical;
    private float mJump;
    public float mSpeed = 1f;

    // Use this for initialization
    void Start()
    {
        mRotAroundX = transform.eulerAngles.x;
        mRotAroundY = transform.eulerAngles.y;
    }

    // Update is called once per frame
    private void Update()
    {
        // rotation
        mRotAroundX += Input.GetAxis("Mouse Y") * mSensitivityX;
        mRotAroundY += Input.GetAxis("Mouse X") * mSensitivityY;

        //translation 
        mHorizontal = Input.GetAxis("Horizontal");
        mVertical = Input.GetAxis("Vertical");
        mJump = Input.GetAxis("Jump");


        RotationXrRig();
        TranslationXrRig();
    }

    private void RotationXrRig()
    {
        transform.rotation = Quaternion.Euler(-mRotAroundX, mRotAroundY, 0); // rotation of Camera
    }


    private void TranslationXrRig()
    {
        transform.Translate(Vector3.right * mHorizontal * mSpeed * Time.deltaTime); // lateraux
        transform.Translate(Vector3.forward * mVertical * mSpeed * Time.deltaTime); // frontaux
        transform.Translate(Vector3.up * mJump * mSpeed * Time.deltaTime); // verticaux
    }

}