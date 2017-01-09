using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class agent : MonoBehaviour
{
    public GameObject target;

    NavMeshAgent nav;
   
    
	// Use this for initialization
	void Start ()
    {

        nav = gameObject.GetComponent<NavMeshAgent>();

        
		
	}
	
	// Update is called once per frame
	void Update () {
        nav.SetDestination(target.transform.position);
    }
}
