using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour
{

    public Material material;

    public Vector3 origin;
    Vector3 offset = new Vector3(0, 0, 0);

    Vector3 offsetX = new Vector3(10, 0, 0);
    Vector3 offsetY = new Vector3(0, 10, 0);
    Vector3 offsetZ = new Vector3(0, 0, 10);


    Vector3[,,] points;

    public Testing test;
    InteractiveGrid grid;

    public Vector3 gridSize;

    float width;
    float height;
    float length;

    public float cellSize = 10;

    // Start is called before the first frame update
    void Start()
    {
        grid = test.getInteractiveGrid();

        width = gridSize.x;
        height = gridSize.y;
        length = gridSize.z;

        points = new Vector3[(int) length, (int) width, (int) height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < length; z++)
                {
                    points[x, y, z] = getWorldPosition(x, y, z);
                }   
            }
            Debug.Log("");
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPostRender()
    {
        RenderLines(points);
    }
    void RenderLines(Vector3[,,] gridPoints)
    {
        GL.Begin(GL.LINES);
        material.SetPass(0);
        GL.Color(material.color);

        for (int x = 0; x < gridPoints.GetLength(0); x++)
        {
            for (int y = 0; y < gridPoints.GetLength(1); y++)
            {
                for(int z = 0; z < gridPoints.GetLength(2); z++)
                {
                    if (x + 1 < gridPoints.GetLength(0) && y + 1 < gridPoints.GetLength(1) && z + 1 < gridPoints.GetLength(2))
                    {
                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, z + 1]);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y + 1, z]);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x + 1, y, z]);
                    }

                    if(x == gridPoints.GetLength(0) - 1 && !(y == gridPoints.GetLength(1) - 1 || z == gridPoints.GetLength(2) - 1))
                    {
                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, z] + offsetY);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, z] + offsetZ);
                    }

                    if (y == gridPoints.GetLength(1) - 1 && !(x == gridPoints.GetLength(0) - 1 || z == gridPoints.GetLength(2) - 1))
                    {
                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, z] + offsetX);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, z] + offsetZ);
                    }

                    if (z == gridPoints.GetLength(2) - 1 && !(x == gridPoints.GetLength(0) - 1 || y == gridPoints.GetLength(1) - 1))
                    {
                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, z] + offsetX);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, z] + offsetY);
                    }

                    if(x == gridPoints.GetLength(0) - 1 && y == gridPoints.GetLength(1) - 1 && z == gridPoints.GetLength(2) - 1)
                    {
                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[0, y, z]);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, 0, z]);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, 0]);
                    }
                }

            }
        }

        GL.End();
    }

    private Vector3 getWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize + origin;
    }

    private Vector3 getWorldPosition(Vector3 point)
    {
        return point * cellSize + origin;
    }
}
