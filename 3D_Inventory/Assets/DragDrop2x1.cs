using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop2x1 : MonoBehaviour
{
    Vector3[] centers = new Vector3[2];

    public Vector3 size;
    public GridManager gridManager;

    float cellSize;
    int height;
    int width;
    int length;
    Vector3 offset;

    Vector3 lastPosition;
    Vector3 targetPos;
    bool dragging = false;

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
        lastPosition = targetPos;
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
        centers[0] = transform.position;
        centers[1] = centers[0] + new Vector3(cellSize, 0, 0);

        //Debug - Shows lines from the centers to the origin
        Debug.DrawLine(offset, centers[0], Color.red);
        Debug.DrawLine(offset, centers[1], Color.green);
    }

    //updates the last position to the location the item was picked up
    private void OnMouseDown()
    {
        lastPosition = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), RoundToNearestGrid(targetPos.z));
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

        //hardcoded check of the two centers
        if (centers[0].x >= gridWidth || centers[0].x <= offset.x ||
            centers[0].y >= gridHeight || centers[0].y <= offset.y ||
            centers[0].z >= gridLength || centers[0].z <= offset.z ||
            centers[1].x >= gridWidth || centers[1].x <= offset.x ||
            centers[1].y >= gridHeight || centers[1].y <= offset.y ||
            centers[1].z >= gridLength || centers[1].z <= offset.z)
        {
            targetPos = lastPosition;
        }
    }
}