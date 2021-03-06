﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BugAIDebug : MonoBehaviour {
	private List<GameObject> _attractants;
	private List<GameObject> _deterrents;
	public float attractantRadius;
	public float speed;
	
	// Use this for initialization
	void Start () {
		_attractants = new List<GameObject> ();
		_deterrents = new List<GameObject> ();
		gameObject.rigidbody.velocity = new Vector3 (Random.value * 10, 0, Random.value * 10);
	}
	
	// Update is called once per frame
	void Update () {
		getStimuli ();
		Vector3 direction = GetAverageStimuliDirection ();
		moveOnGround (direction);
	}
	
	void moveOnGround(Vector3 direction){
		gameObject.rigidbody.velocity += new Vector3 (direction.x, 0, direction.z) * speed;
	}
	
	void moveAnywhere(Vector3 direction){
		gameObject.rigidbody.velocity = direction;
	}
	
	void getStimuli(){
		Collider[] colliders = Physics.OverlapSphere (transform.position, attractantRadius);
		foreach (Collider c in colliders) {
			if(c.gameObject.CompareTag("Attractant"))
				_attractants.Add(c.gameObject);
			else if(c.gameObject.CompareTag("Deterrent"))
				_deterrents.Add(c.gameObject);
		}
	}
	
	
	
	Vector3 GetAverageStimuliDirection(){
		Vector3 sum = new Vector3 (0, 0, 0);
		if (_attractants.Count == 0 && _deterrents.Count == 0)
			return sum;
		//for each attraact, add the vector from you to it.
		foreach (GameObject attractant in _attractants) {
			Vector3 temp = attractant.transform.position - gameObject.transform.position;;
			temp = Vector3.ClampMagnitude(temp, attractantRadius/temp.magnitude);
			sum += temp;
			print (temp.magnitude);
		}
		//for each deterrent, add the vector away from it.
		foreach (GameObject deterrent in _deterrents) {
			Vector3 temp = deterrent.transform.position - gameObject.transform.position;;
			temp = Vector3.ClampMagnitude(temp, attractantRadius/temp.magnitude);
			sum -= temp;
		}
		sum.Normalize();
		return sum;
	}
	
	
	Vector3 GetDirectionTo(Vector3 target)
	{
		Vector3 desired = target - gameObject.transform.position;
		desired.Normalize ();
		return desired;
	}
}

