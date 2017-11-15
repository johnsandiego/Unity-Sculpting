using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undo : MonoBehaviour {

	Stack UndoStack = new Stack();

	PaintMeshDeformer CurrentMesh;
	MeshFilter TempMesh;

	// Use this for initialization
	void Start () {
		CurrentMesh = GetComponent<PaintMeshDeformer> ();
		TempMesh = CurrentMesh.gameObject.GetComponent<MeshFilter> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
