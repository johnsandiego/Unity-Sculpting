using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace sandiegoJohn.VRsculpting{
	
	public class UndoAndRedo : MonoBehaviour {

		Stack<Vector3[]> UndoStack = new Stack<Vector3[]>(); //a vector3 array stack
		Mesh CurrentMesh; 

		// Use this for initialization
		void Start () {
			CurrentMesh = GetComponent<MeshFilter> ().mesh;
			UndoStack.Push (CurrentMesh.vertices);
		}
		
		// Update is called once per frame
		void Update () {
			//push the current mesh to stack once the mousebutton is pressed up
			if (Input.GetMouseButtonUp (0)) {
				PushToStack ();
			}
			//if keyboard key p is pressed pop the current mesh from the stack
			if (Input.GetKeyDown (KeyCode.P)) {
				UndoPaint ();
			}
		}
		//Push the current mesh vertices in the stack
		void PushToStack(){
			UndoStack.Push (CurrentMesh.vertices);
		}
		//checks to make sure their is something in the stack if not return
		//otherwise, pop the top of the stack and assign value to current mesh vertices
		//recalculate normals and bounds.
		void UndoPaint(){
			if (UndoStack.Count != 0) {
				CurrentMesh.vertices = UndoStack.Pop ();
				CurrentMesh.RecalculateNormals ();
				CurrentMesh.RecalculateBounds ();
			} else
				return;

		

		}
	}
}
