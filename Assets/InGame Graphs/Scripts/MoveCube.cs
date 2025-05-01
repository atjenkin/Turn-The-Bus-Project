using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour {

	public bool autoMove=true;
	public CustomGraphic graph;

	public float DC;
	public float amplitude;

	// FixedUpdate is called every 0.02 seconds (default)
	void FixedUpdate () {

		//make cube move
		if(autoMove)
		transform.position = new Vector3 (DC + amplitude*Mathf.Cos(Time.time*2),Mathf.Sin(Time.time), -4);

		//add a point on line zero
		graph.AddPoint (transform.position.x,1);
	
		//add a point on line one
		graph.AddPoint (transform.position.y,0);

	}
}
