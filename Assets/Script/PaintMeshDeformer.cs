using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
public class PaintMeshDeformer : MonoBehaviour {

	public float radius = 1.0f;
	public float pull = 10.0;
	public Vector3 sqrMagnitude;
	public float distance;
	//public float fallOff;


	enum FallOff{
		Gauss, 
		Linear, 
		Needle
	};

	private MeshFilter unappliedMesh;

	public FallOff fallOff = FallOff.Gauss;
	
	// Update is called once per frame
	void Update () {
		if (!Input.GetMouseButton (0)) {
			//ApplyMeshCollider ();
			return;

		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if(Physics.Raycast(ray, out hit){
			MeshFilter filter = hit.collider.GetComponent<MeshFilter>();
			if(filter){
				if(filter != unappliedMesh){
					ApplyMeshCollider();
					unappliedMesh = filter;

				}

				Vector3 relativePoint = filter.transform.InverseTransformPoint(hit.point);
				DeformMesh(filter.mesh, relativePoint, pull*Time.deltaTime, radius);

			}
		}
	}

	public float LinearFallOff(float dis, float inRadius){
		return Mathf.Clamp01 (1.0 - dis / inRadius);
	}

	public float GaussFallOff(float diss, float inRadius){

		return Mathf.Clamp01(Mathf.Pow(360.0, -Mathf.Pow(diss/inRadius,2.5)-0.01));
	}

	public float NeedleFalloff(float dist, float inRadius){

		return -(dist*dist)/(inRadius*inRadius)+1.0;
	}

	public float DeformMesh(Mesh mesh, Vector3 position, float power, float inRadius){
		Mesh vertices = mesh.vertices;
		Mesh normals = mesh.normals;
		float sqrRadius = inRadius * inRadius;

		Vector3 averageNormal = Vector3.zero;
		for (int i = 0; i < vertices.vertexCount; i++) {
			sqrMagnitude = (vertices [i] - position).sqrMagnitude;

			if (sqrMagnitude > sqrRadius)
				continue;

			distance = Mathf.Sqrt (sqrMagnitude);
			fallOff = LinearFallOff (distance, inRadius);
			averageNormal += fallOff * normals [i];

		}
		averageNormal = averageNormal.normalized;

		for (int j = 0; j < vertices.vertexCount; j++) {
			sqrMagnitude = (vertices [j] - position).sqrMagnitude;

			if (sqrMagnitude > sqrRadius)
				continue;

			distance = Mathf.Sqrt (sqrMagnitude);

			switch (fallOff) {
				
			case FallOff.Gauss:
				fallOff = GaussFallOff (distance, inRadius);
				break;
			case FallOff.Needle:
				fallOff = NeedleFalloff (distance, inRadius);
				break;
			default:
				fallOff = LinearFallOff (distance, inRadius);
				break;
			
			}
			vertices [j] += averageNormal * fallOff * power;

		}
		mesh.vertices = vertices;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
			
	}

	void ApplyMeshCollider(){
			if (unappliedMesh && unappliedMesh.GetComponent<MeshFilter> ()) {
					unappliedMesh.GetComponent<MeshFilter> ().mesh = unappliedMesh.mesh;		
		}
		unappliedMesh = null;
	}


}
