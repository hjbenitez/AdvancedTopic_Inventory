using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    Vector3 targetPos;
    bool dragging = false;

    public float gridSize = 10f;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        offset = new Vector3(-5, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!dragging)
        {
            targetPos = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), targetPos.z);
            transform.position = targetPos;
        }
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
        
    }

    public float RoundToNearestGrid(float pos)
    {
        float difference = pos % gridSize;
        pos -= difference;

        if(difference > (gridSize/2))
        {
            pos += gridSize;
        }

        return pos;
    }
}

