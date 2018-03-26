using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

/*
 * JOhn San Diego
 * Able
 * Create afunction to recalculate normals in a set interval not everyframe. 
 */

namespace sandiegoJohn.VRsculpting{
	
	public class PaintMeshDeformer : MonoBehaviour {

		//private SteamVR_TrackedController _controller;

		public GameObject rightController;
		public Demo dm;

		public float radius = 1.0f;
		public float pull = 2.0f;
		public Vector3 sqrMagnitude;
		public float distance;

		public Vector3[] halfVert;
		public Vector3[] otherHalf;
		public Vector3 offset;
		public Vector3 inverse;
		public bool symmetryEnabled = false;


	    //public float fallOff;
		//UI chooser
	    public Dropdown PaintChooser;
	    public Dropdown AddorSubtract;
	    public Slider strengthLevel;
		public Slider radiusLevel;
		public Toggle MeshSymmetry;

		public Vector3 sculptBrushPoint;

		//break the blocks into two
		Mesh SphereA;
		Mesh SphereB;

	    public GameObject[] SculptTools;

		[Tooltip("Raycast Point object")]
		public Transform rayPos;
		public float range;

		public enum FallOff{
			Gauss, 
			Linear, 
			Needle
		};

		private MeshFilter unappliedMesh;
		public MeshFilter filter;
		bool isTriggerOn = false;

	    public FallOff fallOff = FallOff.Gauss;
	    float _fallOff = 0.0f;


	    // Update is called once per frame
	    void Update () {
			Vector3 forward = rayPos.transform.TransformDirection(Vector3.forward) * range;
			Debug.DrawRay (rayPos.transform.position, forward, Color.green);

//			if (!Input.GetMouseButton (0)) {
//				//ApplyMeshCollider ();
//				return;
//
//			}



	        //Debug.Log(fallOff);
			RaycastHit hit;
			//checks if the raycast hits a collider
			if(Physics.Raycast(rayPos.transform.position, forward, out hit, range)){
				//Debug.Log (hit.collider.name);
				if (hit.transform.gameObject.tag != "Non-Modifiable") {
					filter = hit.collider.GetComponent<MeshFilter> ();
					if (filter) {
						if (filter != unappliedMesh) {
							ApplyMeshCollider ();
							unappliedMesh = filter;
						}


						//Mesh Symmetry. simple but it works
						Vector3 relativePoint = filter.transform.InverseTransformPoint (hit.point);
						Vector3 inversePoint = relativePoint;
						inversePoint.x = -relativePoint.x;
						if (isTriggerOn) {
							DeformMesh (filter.mesh, relativePoint, pull * Time.deltaTime, radius);
							if (MeshSymmetry.isOn) {
								DeformMesh (filter.mesh, inversePoint, pull * Time.deltaTime, radius);
							}
						}
					}
				}
			}




		}


//		void OnEnable(){
//			_controller = rightController.GetComponent<SteamVR_TrackedController> ();
//			_controller.TriggerClicked += _controller_TriggerClicked;
//			_controller.TriggerUnclicked += _controller_TriggerClickedDisabled;
//		}

//		void OnDisable(){
//			_controller.TriggerClicked -= _controller_TriggerClicked;
//			_controller.TriggerUnclicked -= _controller_TriggerClickedDisabled;
//		}

//		void _controller_TriggerClicked (object sender, ClickedEventArgs e)
//		{
//			isTriggerOn = true;
//			Debug.Log ("trigger is Pressed");
//
//
//		}

//		void _controller_TriggerClickedDisabled(object sender, ClickedEventArgs e){
//			filter.gameObject.GetComponent<MeshCollider> ().sharedMesh = filter.mesh;
//			isTriggerOn = false;
//			dm.LaplacianSmooth ();
//		}
			



		public float LinearFallOff(float dis, float inRadius){
			return Mathf.Clamp01 (1.0f - dis / inRadius);
		}

		public float GaussFallOff(float diss, float inRadius){
			return Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(diss/inRadius,2.5f)-0.01f));
		}

		public float NeedleFalloff(float dist, float inRadius){
			return -(dist*dist)/(inRadius*inRadius)+1.0f;
		}



	    //deforms mesh
		public void DeformMesh(Mesh mesh, Vector3 position, float power, float inRadius){
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

				switch (PaintChooser.value) {
					
				case 0:
					_fallOff = GaussFallOff (distance, inRadius);
					break;
				case 1:
					_fallOff = NeedleFalloff (distance, inRadius);
	                break;
	            case 2:
	                _fallOff = LinearFallOff(distance, inRadius);
	                break;
				default:
					_fallOff = LinearFallOff (distance, inRadius);
	                break;
				
				}
	            switch (AddorSubtract.value)
	            {
					case 0:
						pull = strengthLevel.value;
						radius = radiusLevel.value;
	                    break;
	                case 1:
	                    pull = -(strengthLevel.value);
						radius = radiusLevel.value;
	                    break;
	                default:
	                    pull = 2.0f;
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
}
