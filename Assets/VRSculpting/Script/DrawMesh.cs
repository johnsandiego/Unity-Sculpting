using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sandiegoJohn.VRsculpting{
	
public class DrawMesh : MonoBehaviour {

		PaintMeshDeformer halfMesh;

	// Use this for initialization
	void Start () {
			halfMesh = GameObject.FindGameObjectWithTag ("mesh").GetComponent<PaintMeshDeformer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
}
