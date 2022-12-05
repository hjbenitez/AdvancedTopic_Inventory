using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop1x1 : MonoBehaviour
{
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
    }

    //updates the last position to the location the item was picked up
    private void OnMouseDown()
    {
        lastPosition = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), RoundToNearestGrid(targetPos.z));
        Debug.Log("last " + lastPosition);
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

        if(difference > (cellSize/2))
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

        if (transform.position.x >= gridWidth || transform.position.x <= offset.x || 
            transform.position.y >= gridHeight || transform.position.y <= offset.y ||
            transform.position.z >= gridLength || transform.position.z <= offset.z)

        {
            targetPos = lastPosition;
        }
    }

}

