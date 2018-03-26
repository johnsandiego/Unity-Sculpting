using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageVertexMesh : MonoBehaviour {

	Demo dm;
	MeshFilter mesh;
	public float intensity = 0.0f;
	public float radius = 1.0f;
	public float pull = 2.0f;
	public Vector3 sqrMagnitude;
	public float distance;
	float _fallOff = 0.0f;

	public enum FallOff{
		Gauss, 
		Linear, 
		Needle
	};

	public enum Tools
	{
		Sculpt,
		Smooth
	};

	private MeshFilter unappliedMesh;
	 MeshFilter filter;
	bool isTriggerOn = false;

	public FallOff fallOff = FallOff.Gauss;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter> ();
		//ApplyNormalNoise (mesh.mesh);
		dm = GetComponent<Demo>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (!Input.GetMouseButton (0)) {
			//Debug.Log ("left mouse click");
			ApplyMeshCollider ();
			return;
		}

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Debug.Log(fallOff);
		RaycastHit hit;
		//checks if the raycast hits a collider
		if(Physics.Raycast(ray, out hit)){
			//Debug.Log (hit.collider.name);
			if (hit.transform.gameObject.tag != "Non-Modifiable") {
				filter = hit.collider.GetComponent<MeshFilter> ();
				if (filter) {
					if (filter != unappliedMesh) {
						ApplyMeshCollider ();
						unappliedMesh = filter;
					}
					Vector3 relativePoint = filter.transform.InverseTransformPoint (hit.point);
					SmoothMesh (filter.mesh, relativePoint, pull * Time.deltaTime, radius);
					filter.gameObject.GetComponent<MeshCollider> ().sharedMesh = filter.mesh;
					//dm.HumphreySmooth ();
					}
				}
			}

			if (Input.GetMouseButtonUp (1)) {
				dm.HumphreySmooth ();
			}
		}

	Mesh ApplyNormalNoise (Mesh mesh) {

		var vertices = mesh.vertices;
		var normals = mesh.normals;
		for(int i = 0, n = mesh.vertexCount; i < n; i++) {
			vertices[i] = vertices[i] + normals[i] * Random.value * intensity;
		}
		mesh.vertices = vertices;

		return mesh;
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


	public void SmoothMesh(Mesh mesh, Vector3 position, float power, float inRadius){
		//Mesh meshShared = filter.sharedMesh;
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
