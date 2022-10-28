using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Liner : MonoBehaviour {

public GameObject OtherGameObject;
LineRenderer line;


public float RopeWidth=0.5f;
	void Start () {
		if(OtherGameObject == null)
		{
			Debug.LogWarning("Please Attach Other GameObject in inspector");
			return;
		}
		line = GetComponent<LineRenderer>();


		line.SetWidth(RopeWidth,RopeWidth);

		line.useWorldSpace = true;

		line.positionCount = 2;

		line.SetPosition(0,gameObject.transform.position);
		line.SetPosition(1,OtherGameObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {

		line.SetPosition(0,gameObject.transform.position);
		line.SetPosition(1,OtherGameObject.transform.position);
		
	}
}
