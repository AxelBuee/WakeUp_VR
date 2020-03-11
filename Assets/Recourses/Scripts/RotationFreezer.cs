using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Permet de fixer la rotation globale du GameObject
/// contenant ce script en Component.
public class RotationFreezer : MonoBehaviour 
{

	Quaternion mRotation;

	void Start () 
	{
		mRotation = this.transform.rotation;
	}
	
	void Update () 
	{
		this.transform.rotation = mRotation;			
	}
}
