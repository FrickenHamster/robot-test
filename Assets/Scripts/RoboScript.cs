using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float hAxis = Input.GetAxis("Horizontal");
		this.transform.Translate(hAxis * 5 * Time.deltaTime, 0, 0, Space.World);
	}
}
