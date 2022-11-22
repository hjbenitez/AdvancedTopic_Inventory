using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    Vector3 lastPosition;
    Vector3 targetPos;
    bool dragging = false;

    public float cellSize = 10f;
    public int height = 5;
    public int width = 5;
    public int length = 5;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        lastPosition = targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(!dragging)
        {
            targetPos = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), RoundToNearestGrid(targetPos.z));
            transform.position = targetPos;
        }
    }

    private void OnMouseDown()
    {
        lastPosition = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), RoundToNearestGrid(targetPos.z));
        Debug.Log("last " + lastPosition);
    }

    private void OnMouseDrag()
    {
        dragging = true;
        float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
        transform.position = targetPos;
    }

    private void OnMouseUp()
    {
        dragging = false;
        checkBoundaries();
    }

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

    public void checkBoundaries()
    {
        float gridWidth = width * cellSize + offset.x;
        float gridHeight = height * cellSize + offset.y;
        float gridLength = length * cellSize + offset.z;

        if (transform.position.x >= gridWidth || transform.position.x <= offset.x || 
            transform.position.y >= gridHeight || transform.position.y <= offset.y)

        {
            targetPos = lastPosition;
            Debug.Log("HI");
        }
    }
}

