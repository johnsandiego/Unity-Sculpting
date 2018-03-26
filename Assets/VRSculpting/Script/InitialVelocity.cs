using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVelocity : MonoBehaviour {

	public float speed = 10.0f;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody> ().velocity = transform.TransformDirection (Vector3.forward) * speed;
	}

}
