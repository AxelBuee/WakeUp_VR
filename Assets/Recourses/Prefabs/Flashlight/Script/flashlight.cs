using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour {
	private Light myLight;
	public GameObject sourceLamp;

	// Use this for initialization
	void Start () {
		 myLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space))
        {
            myLight.enabled = !myLight.enabled;
			
			if(myLight.enabled == true) {
				sourceLamp.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
			}

			if(myLight.enabled == false) {
				sourceLamp.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
			}
        }
	}
}
