using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour
{
    public Material material;

    public Vector3 origin;

    Vector3 offsetX;
    Vector3 offsetY;
    Vector3 offsetZ;


    Vector3[,,] points;

    public GridManager gridManager;

    float width;
    float height;
    float length;

    float cellSize;

    // Start is called before the first frame update
    void Start()
    {
        width = gridManager.width+1;
        height = gridManager.height+1;
        length = gridManager.length+1;
        cellSize = gridManager.cellSize;

        //used when the grid is close to completion
        offsetX = new Vector3(cellSize, 0, 0);
        offsetY = new Vector3(0, cellSize, 0);
        offsetZ = new Vector3(0, 0, cellSize);


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

    //built in function - runs after update
    private void OnPostRender()
    {
        RenderLines(points);
    }
    //draws the grid lines
    void RenderLines(Vector3[,,] gridPoints)
    {
        //begins the method to draw the lines
        GL.Begin(GL.LINES);
        material.SetPass(0);
        GL.Color(material.color);

        //draws the lines on the x axis
        for (int x = 0; x < gridPoints.GetLength(0); x++)
        {
            //draws the lines on the y axis
            for (int y = 0; y < gridPoints.GetLength(1); y++)
            {
                //draws the lines on the z axis
                for(int z = 0; z < gridPoints.GetLength(2); z++)
                {
                    //checks to make sure the lines being drawn are NOT the lines at the maximum edges
                    if (x + 1 < gridPoints.GetLength(0) && y + 1 < gridPoints.GetLength(1) && z + 1 < gridPoints.GetLength(2))
                    {
                        //draws  a line up, right, and in from the selected point
                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y, z + 1]);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x, y + 1, z]);

                        GL.Vertex(gridPoints[x, y, z]);
                        GL.Vertex(gridPoints[x + 1, y, z]);
                    }

                    //For when the last few lines need to get drawn (when x, y, or ) are at their maxes
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
}
