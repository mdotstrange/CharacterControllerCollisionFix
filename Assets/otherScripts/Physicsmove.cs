using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physicsmove : MonoBehaviour {

    public float mult;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.right * mult * Time.deltaTime, ForceMode.Force);
		
	}
}
