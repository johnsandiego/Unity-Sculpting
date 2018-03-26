using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardOrbit : MonoBehaviour {

	public float speed = 50f;

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (Vector3.up * Time.deltaTime * speed);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (Vector3.down * Time.deltaTime * speed);
		}
		if (Input.GetKey (KeyCode.W)) {
			transform.Rotate (Vector3.left * Time.deltaTime * speed);
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.Rotate (Vector3.right * Time.deltaTime * speed);
		}
		if (Input.GetKey (KeyCode.Q)) {
			transform.Rotate (Vector3.forward * Time.deltaTime * speed);
		}
		if (Input.GetKey (KeyCode.E)) {
			transform.Rotate (Vector3.back * Time.deltaTime * speed);
		}
	}
}
