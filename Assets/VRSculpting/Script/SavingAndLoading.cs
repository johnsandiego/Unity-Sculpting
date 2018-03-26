
using UnityEngine;
using UnityEditor;

public class SavingAndLoading : MonoBehaviour {

		[MenuItem("Saving/save Mesh")]
		static void SaveMesh(){
			GameObject mesh = new GameObject();
			AssetDatabase.CreateAsset (mesh, "Assets/MyMesh.asset");
			AssetDatabase.SaveAssets ();
		}
}

