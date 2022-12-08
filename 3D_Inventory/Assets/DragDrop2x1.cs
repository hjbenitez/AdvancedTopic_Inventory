using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop2x1 : MonoBehaviour
{
    public Vector3[] centers;

    public Vector3 size;
    public GridManager gridManager;
    public int numberOfCenters;
    public List<Transform> blocks = new List<Transform>();

    float cellSize;
    int height;
    int width;
    int length;
    Vector3 offset;

    public Vector3[] lastPositions;
    Vector3 targetPos;
    public bool dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        //grabs values from the grid created
        width = gridManager.width;
        height = gridManager.height;
        length = gridManager.length;
        cellSize = gridManager.cellSize;
        offset = gridManager.offset;

        targetPos = transform.position;

        centers = new Vector3[numberOfCenters];

        foreach(Transform child in this.gameObject.transform)
        {
            blocks.Add(child);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //enters when the right mouse is not being held down
        if (!dragging)
        {
            targetPos = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), RoundToNearestGrid(targetPos.z));
            transform.position = targetPos;
        }

        //updates the centers of the 2x1 block
        for(int i = 0; i < centers.Length; i++)
        {
            centers[i] = blocks[i].position;
        }

        //centers[0] = transform.position;
        //centers[1] = centers[0] + new Vector3(cellSize, 0, 0);

        //Debug - Shows lines from the centers to the origin
        Debug.DrawLine(offset, centers[0], Color.red);
        Debug.DrawLine(offset, centers[1], Color.green);
    }

    //updates the last position to the location the item was picked up
    private void OnMouseDown()
    {
        for(int i = 0; i < centers.Length; i++)
        {
            lastPositions[i] = new Vector3(RoundToNearestGrid(centers[i].x), RoundToNearestGrid(centers[i].y), RoundToNearestGrid(centers[i].z));
        }
    }

    //runs when left-clicked dragging on the object to move it around
    private void OnMouseDrag()
    {
        dragging = true;
        float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
        transform.position = targetPos;
    }

    //checks boundaries when dropped
    private void OnMouseUp()
    {
        dragging = false;
        checkBoundaries();
    }

    //rounds to the nearest grid cell
    public float RoundToNearestGrid(float pos)
    {
        float difference = pos % cellSize;
        pos -= difference;

        if (difference > (cellSize / 2))
        {
            pos += cellSize;
        }

        return pos;
    }
    //checks all centers to ensure they are inside the grid
    //drops it at the last known correct location when dropped out of bounds
    public void checkBoundaries()
    {
        float gridWidth = width * cellSize + offset.x;
        float gridHeight = height * cellSize + offset.y;
        float gridLength = length * cellSize + offset.z;

        for (int i = 0; i < centers.Length; i++)
        {
            Vector3 index = new Vector3(Mathf.Round(transform.position.x / cellSize - 1), Mathf.Round(transform.position.y / cellSize - 1), Mathf.Round(transform.position.z / cellSize - 1));

            if (!(centers[i].x <= gridWidth && centers[i].x >= offset.x &&
            centers[i].y <= gridHeight && centers[i].y >= offset.y &&
            centers[i].z <= gridLength && centers[i].z >= offset.z &&
            gridManager.inventorySpace[(int)index.x, (int)index.y, (int)index.z] == false))
            {
                targetPos = lastPositions[0];
            }          
        }



        /*hardcoded check of the two centers
        if (centers[0].x >= gridWidth || centers[0].x <= offset.x ||
            centers[0].y >= gridHeight || centers[0].y <= offset.y ||
            centers[0].z >= gridLength || centers[0].z <= offset.z ||
            centers[1].x >= gridWidth || centers[1].x <= offset.x ||
            centers[1].y >= gridHeight || centers[1].y <= offset.y ||
            centers[1].z >= gridLength || centers[1].z <= offset.z)
        {
            targetPos = lastPosition;
        }
        */
    }
}
