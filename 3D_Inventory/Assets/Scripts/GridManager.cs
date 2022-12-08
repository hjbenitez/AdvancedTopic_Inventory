using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Parameters")]
    public int width;
    public int height;
    public int length;
    public float cellSize;
    public Vector3 offset;

    [Space(10)]

    [Header("Inventory Contents")]
    public GameObject[] items;

    public bool[,,] inventorySpace;
    private InteractiveGrid grid;
    
    private void Start()
    {
        grid = new InteractiveGrid(width, height, length, cellSize, offset);
        inventorySpace = new bool[width, height, length];
        grid.CenterCamera();
    }

    private void Update()
    {
        foreach (GameObject item in items)
        {
            Vector3[] lastPositions = item.GetComponent<DragDrop>().lastPositions;

            if (item.GetComponent<DragDrop>().dragging == false)
            {
                Vector3[] centers = item.GetComponent<DragDrop>().centers;

                foreach(Vector3 center in centers)
                {
                    Vector3 spacePos = new Vector3(Mathf.Round((center.x / cellSize) - 1), Mathf.Round((center.y / cellSize) - 1), Mathf.Round((center.z / cellSize) - 1));

                    inventorySpace[(int)spacePos.x, (int)spacePos.y, (int)spacePos.z] = true;
                    //grid.setValue((int)spacePos.x, (int)spacePos.y, (int)spacePos.z, true);
                }
            }

            if (item.GetComponent<DragDrop>().dragging == true)
            {
                foreach(Vector3 lastPosition in lastPositions)
                {
                    Vector3 lp = new Vector3(Mathf.Round((lastPosition.x / cellSize) - 1), Mathf.Round((lastPosition.y / cellSize) - 1), Mathf.Round((lastPosition.z / cellSize) - 1));
                    inventorySpace[(int)lp.x, (int)lp.y, (int)lp.z] = false;
                    //grid.setValue((int)lp.x, (int)lp.y, (int)lp.z, false);
                }
            }
        }
    }
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.gameObject.transform.position.z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);
        Debug.Log(worldPosition);
        return worldPosition;
        
    }

    public InteractiveGrid getInteractiveGrid()
    {
        return grid;
    }
}
