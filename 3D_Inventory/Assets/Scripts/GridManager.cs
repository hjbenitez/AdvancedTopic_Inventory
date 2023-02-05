using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //parameters of the grid
    [Header("Grid Parameters")]
    public int width;
    public int height;
    public int length;
    public float cellSize;
    public Vector3 offset;

    [Space(10)]

    //the contents of the inventory (set in Inspector currently)
    [Header("Inventory Contents")]
    public GameObject[] items;

    public bool[,,] inventorySpace; // an array the same size as the grid that tracks if a space is occupied or not
    private InteractiveGrid grid; //the grid building system
    public bool spaceOccupied = false;

    private void Start()
    {
        grid = new InteractiveGrid(width, height, length, cellSize, offset); //creates the grid 
        inventorySpace = new bool[width, height, length]; //sets the space checker to the same size as the grid
        grid.CenterCamera(); //centers the camera on the grid
    }

    private void Update()
    {
        items = GameObject.FindGameObjectsWithTag("Item");

        //runs for every item in the inventory system
        foreach (GameObject item in items)
        {
            //gets the last positions of the item before moving the item
            Vector3[] lastPositions = item.GetComponent<DragDrop>().lastPositions; 

            //checks to see if the item is not moving
            if (!item.GetComponent<DragDrop>().dragging)
            {
                //gets the centers of the item 
                Vector3[] centers = item.GetComponent<DragDrop>().centers;

                //runs for every center in the item
                foreach(Vector3 center in centers)
                {
                    //caluclates the current position in the grid
                    Vector3 spacePos = new Vector3(Mathf.Round((center.x / cellSize) - 1), Mathf.Round((center.y / cellSize) - 1), Mathf.Round((center.z / cellSize) - 1));

                    //uses the grid position to say that the space is occupied
                    inventorySpace[(int)spacePos.x, (int)spacePos.y, (int)spacePos.z] = true;

                    //DEBUG --- Visual Aid
                    grid.setValue((int)spacePos.x, (int)spacePos.y, (int)spacePos.z, true);
                }
                spaceOccupied = true;
            }

            //checks if the item is moving
            if (item.GetComponent<DragDrop>().dragging == true)
            {
                //using all the last positions of each center, tell the game that their last position space is unoccupied now
                foreach(Vector3 lastPosition in lastPositions)
                {
                    Vector3 lp = new Vector3(Mathf.Round((lastPosition.x / cellSize) - 1), Mathf.Round((lastPosition.y / cellSize) - 1), Mathf.Round((lastPosition.z / cellSize) - 1));
                    inventorySpace[(int)lp.x, (int)lp.y, (int)lp.z] = false;
                    grid.setValue((int)lp.x, (int)lp.y, (int)lp.z, false);
                }
            }
        }
    }

    public InteractiveGrid getInteractiveGrid()
    {
        return grid;
    }
}
