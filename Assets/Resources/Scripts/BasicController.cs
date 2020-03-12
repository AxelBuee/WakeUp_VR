using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> De quoi faire deplacer la camera avec les touches flechees.
/// envoie un raycast depuis la camera avec clique gauche
/// </summary>
[RequireComponent(typeof(Camera))]
public class BasicController : MonoBehaviour 
{
    private Camera mCamera;
    
	private float mRotAroundX = 0f;
    private float mRotAroundY = 0f;
    private float mRrotAroundX = 0f;
    private float mMinRotationX = 0f;
    private float mMaxRotationX = 0f;
    public float mSensitivityY = 1f;
    public float mSensitivityX = 1f;
    private float mRrotAroundY = 0f;

    private float mHorizontal;
    private float mVertical;
    private float mJump;
    public float mSpeed = 1f;
    private RaycastHit mHit;

    // Use this for initialization
    void Start () 
	{
        mCamera = this.GetComponent<Camera>();
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


        CameraRotation();
    	CameraTranslation();

		// raycast au clique gauche
		if( Input.GetKey(KeyCode.Mouse0) )
		{
			//CameraRaycast();
		}
    }

    private void CameraRotation()
    {
		if( transform.parent != null )
		{
        	transform.parent.rotation = Quaternion.Euler( 0, mRotAroundY, 0 ); // rotation of parent (player body) ssi on control un perso
		}
        mCamera.transform.rotation = Quaternion.Euler( -mRotAroundX, mRotAroundY, 0 ); // rotation of Camera
    }

	
    private void CameraTranslation()
    {
		mCamera.transform.Translate(Vector3.right * mHorizontal * mSpeed * Time.deltaTime); // lateraux
		mCamera.transform.Translate(Vector3.forward * mVertical * mSpeed * Time.deltaTime); // frontaux
		mCamera.transform.Translate(Vector3.up * mJump * mSpeed * Time.deltaTime); // verticaux
    }

	private void CameraRaycast()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.SphereCast(transform.position, 1f, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {       
            if(hit.transform != null)
            {
                mHit = hit;
            }
            Debug.Log("Did Hit : " + hit.collider.name);
            if( hit.collider.name.StartsWith("Marble") )
			{
                Vector3 lPullForce = (this.transform.position - hit.transform.position);
                lPullForce.y = 0f;
                lPullForce = lPullForce.normalized * 0.1f;
                hit.rigidbody.AddForce( lPullForce , ForceMode.VelocityChange);
                //hit.rigidbody.angularVelocity = Vector3
                //hit.rigidbody.AddForceAtPosition( hit.transform.forward, ForceMode.Impulse );
                //hit.transform.Rotate( hit.transform.forward, Space.Self );
                //hit.transform.Translate( hit.transform.forward, Space.Self );
                /*
				var lMarbleAnimation = hit.transform.GetComponent<Animation>();
				lMarbleAnimation.Play(); 
                */
			}
            
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000,  Color.magenta, 1f);
            //Debug.Log("Did not Hit");
        }
    }

        void OnDrawGizmos()
        {
            if( mHit.transform != null )
            {
                // Draw a yellow sphere at the transform's position
                Gizmos.color = new Color(0.5f, 0f, 0.5f, 0.5f);
                Gizmos.DrawSphere( mHit.transform.position, 1f );
            }
        }
}
