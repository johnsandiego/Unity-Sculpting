using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshEditor: MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		int i = 0;
		while (i < vertices.Length) {
			vertices[i] += Vector3.up * Time.deltaTime;
			i++;
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();

	}
}
