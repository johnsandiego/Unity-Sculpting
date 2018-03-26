using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CallingExporterScript : MonoBehaviour {

	EditorObjExporter exporterObj = new EditorObjExporter();
	GameObject mesh;
	// Use this for initialization
	void Start () {
		mesh = GameObject.FindGameObjectWithTag ("Mesh");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void exportToMesh(){
		
	}
}
