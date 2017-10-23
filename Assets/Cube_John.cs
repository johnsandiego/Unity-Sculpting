using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube_John: MonoBehaviour {

	//dimension for the cube and number of vertices?
	public int xSize, ySize, zSize;

	private Mesh mesh;
	private Vector3[] vertices;

	private void Awake(){
		Generate();
	}

	private void Generate(){
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedural Cube";
        //WaitForSeconds wait = new WaitForSeconds (0.05f);
        CreateVertices();
        CreateTriangles();
		



    }

    //Creates the vertices for the cube
    private void CreateVertices()
    {
        int cornerVertices = 8;
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;
        int faceVertices = (
                               (xSize - 1) * (ySize - 1) +
                               (xSize - 1) * (zSize - 1) +
                               (ySize - 1) * (zSize - 1)) * 2;
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int v = 0;
        //draws on y axis
        for (int y = 0; y <= ySize; y++)
        {
            //draws the on positive x
            for (int x = 0; x <= xSize; x++)
            {
                vertices[v++] = new Vector3(x, y, 0);
                //yield return wait;
            }
            //draws on the positive z
            for (int z = 1; z <= zSize; z++)
            {
                vertices[v++] = new Vector3(xSize, y, z);
                //yield return wait;
            }
            //draws on the negative x
            for (int x = xSize - 1; x >= 0; x--)
            {
                vertices[v++] = new Vector3(x, y, zSize);
                //yield return wait;
            }
            //draws on the negative z
            for (int z = zSize - 1; z > 0; z--)
            {
                vertices[v++] = new Vector3(0, y, z);
                //yield return wait;
            }


        }
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, ySize, z);
                //yield return wait;
            }
        }
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, 0, z);
                //yield return wait;
            }
        }
        mesh.vertices = vertices;
    }

    //creates the triangle face or polygon. 
    private void CreateTriangles()
    {
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads * 6];
        mesh.triangles = triangles;
        int ring = (xSize + zSize) * 2;
        int t = 0, v = 0;

        //wraps the triangle around the cube.
        for (int y = 0; y < ySize; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);

            }
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
            t = CreateTopFace(triangles, t, ring);
            mesh.triangles = triangles;
        }

    }
    //creates the top face
    private int CreateTopFace(int[] triangles, int t, int ring)
    {
        
        int v = ring * ySize;   //no of rings that goes fully around the cube times the height
        for(int x = 0; x < xSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }
        int vMin = ring * (ySize + 1) -1;
        int vMid = vMin + 1;
        int vMax = v + 2;
        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
            for (int x = 1; x < xSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
            }
            t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMid + 1);
        }
        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMin - 2);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }
        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);
        return t;
        
    }

    //creates a quad of the triangle face
    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }


    //draws vertices on the screen
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
