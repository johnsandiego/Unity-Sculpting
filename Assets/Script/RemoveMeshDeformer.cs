using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class RemoveMeshDeformer : MonoBehaviour {

	public float radius = 1.0f;
	public float pull = 10.0f;
	public Vector3 sqrMagnitude;
	public float distance;
	//public float fallOff;


	public enum FallOff{
		Gauss, 
		Linear, 
		Needle
	};

	private MeshFilter unappliedMesh;

    FallOff fallOff = FallOff.Gauss;
    float _fallOff = 0.0f;

    // Update is called once per frame
    void Update () {
		if (!Input.GetMouseButton (0)) {
			//ApplyMeshCollider ();
			return;

		}
        //Debug.Log(fallOff);
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if(Physics.Raycast(ray, out hit)){
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
		return Mathf.Clamp01 (1.0f - dis / inRadius);
	}

	public float GaussFallOff(float diss, float inRadius){

		return Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(diss/inRadius,2.5f)-0.01f));
	}

	public float NeedleFalloff(float dist, float inRadius){

		return -(dist*dist)/(inRadius*inRadius)+1.0f;
	}

	public void DeformMesh(Mesh mesh, Vector3 position, float power, float inRadius){
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		float sqrRadius = inRadius * inRadius;

		Vector3 averageNormal = Vector3.zero;
		for (int i = 0; i < vertices.Length; i++) {
			float sqrMagnitude = (vertices[i] - position).sqrMagnitude;
            //Debug.Log(sqrMagnitude);
			if (sqrMagnitude > sqrRadius)
				continue;

			distance = Mathf.Sqrt (sqrMagnitude);
			float fallOff = LinearFallOff (distance, inRadius);
			averageNormal += fallOff * normals [i];

		}
		averageNormal = averageNormal.normalized;

		for (int j = 0; j < vertices.Length; j++) {
			float sqrMagnitude = (vertices [j] - position).sqrMagnitude;

			if (sqrMagnitude > sqrRadius)
				continue;

			distance = Mathf.Sqrt (sqrMagnitude);

			switch (fallOff) {
				
			case FallOff.Gauss:
				_fallOff = GaussFallOff (distance, inRadius);
				break;
			case FallOff.Needle:
				_fallOff = NeedleFalloff (distance, inRadius);
				break;
			default:
				_fallOff = LinearFallOff (distance, inRadius);
				break;
			
			}
			vertices [j] += averageNormal * _fallOff * power;

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
