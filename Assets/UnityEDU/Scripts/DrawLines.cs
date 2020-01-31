using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace DimBoxes
{
    [ExecuteInEditMode]
public class DrawLines : MonoBehaviour {
	public Material lineMaterial;
	public Color lColor = Color.green;
	List<Vector3[,]> outlines;
    List<Vector3[,]> triangles;
	public List<Color> colors;

	void Awake () {
		outlines = new List<Vector3[,]>();
		colors = new List<Color>();
        triangles = new List<Vector3[,]>();
	}
	
	void Start () {
//		outlines = new List<Vector3[,]>();
//		colors = new List<Color>();
//		CreateLineMaterial();
	}

	void OnPostRender() {
		if(outlines==null) return;
	    lineMaterial.SetPass( 0 );
	    GL.Begin( GL.LINES );
		for (int j=0; j<outlines.Count; j++) {
			GL.Color(colors[j]);
			for (int i=0; i<outlines[j].GetLength(0); i++) {
				GL.Vertex(outlines[j][i,0]);
				GL.Vertex(outlines[j][i,1]);
			}
		}
		GL.End();

        GL.Begin(GL.TRIANGLES);

        for (int j = 0; j <triangles.Count; j++)
        {
            GL.Color(colors[j]);
            for (int i = 0; i < triangles[j].GetLength(0); i++)
            {
                GL.Vertex(triangles[j][i, 0]);
                GL.Vertex(triangles[j][i, 1]);
                GL.Vertex(triangles[j][i, 2]);
            }
        }

        GL.End();
	}
		
	public void setOutlines(Vector3[,] newOutlines, Color newcolor) {
        if (newOutlines == null) return;
        if (outlines == null) return;
		if(newOutlines.GetLength(0)>0)	{
			outlines.Add(newOutlines);
			colors.Add(newcolor);
		}
	}

    public void setOutlines(Vector3[,] newOutlines, Color newcolor, Vector3[,] newTriangles)
    {
        if (newOutlines == null) return;
        if (outlines == null) return;
        if (newOutlines.GetLength(0) > 0)
        {
            outlines.Add(newOutlines);
            colors.Add(newcolor);
            triangles.Add(newTriangles);
        }
    }	
	
	void Update () {
		outlines = new List<Vector3[,]>();
		colors = new List<Color>();
        triangles = new List<Vector3[,]>();
	}
}
}
