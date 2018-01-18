using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshExtrude : MonoBehaviour {

	public MeshExtrusion Extrusion;
	public Mesh srcMesh;
	public MeshExtrusion.Edge[] precomputedEdges;
	List<ExtrudedTrailSection> sections = new List<ExtrudedTrailSection>();

	public float time = 1.0f;
	public bool autoCalculateOrientation = true;
	public float minDistance = 0.1f;
	public bool invertFaces = false;

	Matrix4x4 worldToLocal;
	Matrix4x4[] finalSections;
	Quaternion previousRotation;
	Vector3 direction;
	Quaternion rotation;
	public float angle;

	public class ExtrudedTrailSection
	{
		public Vector3 point;
		public Matrix4x4 matrix;
		public float time;
	}
	// Use this for initialization
	void Start () {
		srcMesh = GetComponent<MeshFilter> ().sharedMesh;
		precomputedEdges = MeshExtrusion.BuildManifoldEdges (srcMesh);

	}
	
	// Update is called once per frame
	void LateUpdate () {
		var position = transform.position;
		var now = Time.time;

		while (sections.Count > 0 && now > sections [sections.Count - 1].time + time) {
			sections.RemoveAt (0);
		}

		// Add a new trail section to beginning of array
		if (sections.Count == 0 || (sections[0].point - position).sqrMagnitude > minDistance * minDistance)
		{
			ExtrudedTrailSection section = new ExtrudedTrailSection ();
			section.point = position;
			section.matrix = transform.localToWorldMatrix;
			section.time = now;
			sections.Insert(0, section);
		}

		if (sections.Count < 2) 
			return;

		worldToLocal = transform.worldToLocalMatrix;
		finalSections = new Matrix4x4[sections.Count];

	
		for (int i=0; i<sections.Count; i++)
		{
			if (autoCalculateOrientation)
			{
				if (i == 0)
				{
					direction = sections[0].point - sections[1].point;
					rotation = Quaternion.LookRotation(direction, Vector3.up);
					previousRotation = rotation;
					finalSections[i] = worldToLocal * Matrix4x4.TRS(position, rotation, Vector3.one);	
				}
				// all elements get the direction by looking up the next section
				else if (i != sections.Count - 1)
				{	
					direction = sections[i].point - sections[i+1].point;
					rotation = Quaternion.LookRotation(direction, Vector3.up);

					angle = Quaternion.Angle (previousRotation, rotation);
					// When the angle of the rotation compared to the last segment is too high
					// smooth the rotation a little bit. Optimally we would smooth the entire sections array.
					if (angle > 20) {
						rotation = Quaternion.Slerp (previousRotation, rotation, .5f);
					}

					previousRotation = rotation;
					finalSections[i] = worldToLocal * Matrix4x4.TRS(sections[i].point, rotation, Vector3.one);
				}
				// except the last one, which just copies the previous one
				else
				{
					finalSections[i] = finalSections[i-1];
				}
			}
			else
			{
				if (i == 0)
				{
					finalSections[i] = Matrix4x4.identity;
				}
				else
				{
					finalSections[i] = worldToLocal * sections[i].matrix;
				}
			}
		}
			
		MeshExtrusion.ExtrudeMesh (srcMesh, GetComponent<MeshFilter> ().mesh, finalSections, precomputedEdges, invertFaces);
	}
}
