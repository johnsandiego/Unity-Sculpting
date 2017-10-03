using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour {

	//dimension for the cube and number of vertices?
	public int xSize, ySize, zSize;

	private Mesh mesh;
	private Vector3[] vertices;

	private void Awake(){
		StartCoroutine(Generate());
	}

	private IEnumerator Generate(){
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedural Cube";
		WaitForSeconds wait = new WaitForSeconds (0.05f);

		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;
		int faceVertices = (
		                       (xSize - 1) * (ySize - 1) +
		                       (xSize - 1) * (zSize - 1) +
		                       (ySize - 1) * (zSize - 1)) * 2;
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

		int v = 0;
		for (int x = 0; x <= xSize; x++) {
			vertices [v++] = new Vector3 (x, 0, 0);
			yield return wait;
		}
		for (int z = 0; z <= zSize; z++) {
			vertices [v++] = new Vector3 (xSize, 0, z);
			yield return wait;
		}




	}

	private void OnDrawGizmos(){
		if (vertices == null) {
			return;
		}
		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Length; i++) {
			Gizmos.DrawSphere (vertices [i], 0.1f);
		}
	}
}
