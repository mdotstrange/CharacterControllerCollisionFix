using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class translatePlatform : MonoBehaviour {

    public float daX;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {


       transform.Translate(Vector3.right* daX * Time.deltaTime);
    }
}
