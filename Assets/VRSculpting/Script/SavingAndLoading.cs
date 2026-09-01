
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>Editor-only menu commands for the sculpting project.</summary>
public static class SavingAndLoading
{
	[MenuItem("Saving/Save Mesh")]
	private static void SaveMesh()
	{
		var mesh = new Mesh { name = "MyMesh" };
		AssetDatabase.CreateAsset(mesh, "Assets/MyMesh.asset");
		AssetDatabase.SaveAssets();
	}
}
#endif

