using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace mattatz.MeshSmoothingSystem.Demo {

	[RequireComponent (typeof(MeshRenderer))]
	[RequireComponent (typeof(MeshFilter))]
	public class Demo : MonoBehaviour {

		public Dropdown SmoothAlgorithmValue;
		public Button ActivateSmooth;

		[System.Serializable] 
		enum FilterType {
			Laplacian, HC
		};

		MeshFilter filter {
			get {
				if(_filter == null) {
					_filter = GetComponent<MeshFilter>();
				}
				return _filter;
			}
		}

		MeshFilter _filter;

		[SerializeField, Range(0f, 1f)] float intensity = 0.0f;
		[SerializeField] FilterType type;
		[SerializeField, Range(0, 20)] int times = 3;
		[SerializeField, Range(0f, 1f)] float hcAlpha = 0.5f;
		[SerializeField, Range(0f, 1f)] float hcBeta = 0.5f;

		void Start () {
			var mesh = filter.mesh;
			filter.mesh = ApplyNormalNoise(mesh);
			ActivateSmooth.onClick.AddListener (LaplacianSmooth);
//			switch(type) {
//			case FilterType.Laplacian:
//				filter.mesh = MeshSmoothing.LaplacianFilter(filter.mesh, times);
//				break;
//			case FilterType.HC:
//				filter.mesh = MeshSmoothing.HCFilter(filter.mesh, times, hcAlpha, hcBeta);
//				break;
//			}
		}
		
		 void Update () {
			if(Input.GetKeyDown(KeyCode.L)){
				filter.mesh = MeshSmoothing.LaplacianFilter (filter.mesh, times);
				return;
			}
			if(Input.GetKeyDown(KeyCode.H)){
				filter.mesh = MeshSmoothing.HCFilter (filter.mesh, times);
				return;
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
		//drop down for smoother. assigns which smoother to use for the button
		public void SmoothChooser(){
			switch (SmoothAlgorithmValue.value) {
			case 0:
				ActivateSmooth.onClick.AddListener (LaplacianSmooth);
				break;
			case 1:
				ActivateSmooth.onClick.AddListener (HumphreySmooth);
				break;
			default:
				ActivateSmooth.onClick.AddListener (LaplacianSmooth);
				break;
			}
		}

		//function for the button to use
		public void LaplacianSmooth(){
			filter.mesh = MeshSmoothing.LaplacianFilter (filter.mesh, times);
		}

		public void HumphreySmooth(){
			filter.mesh = MeshSmoothing.HCFilter (filter.mesh, times);
		}

	}

}

