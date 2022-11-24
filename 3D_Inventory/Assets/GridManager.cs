using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width;
    public int height;
    public int length;

    public float cellSize;
    public Vector3 offset;

    private InteractiveGrid grid;
    private void Start()
    {
        grid = new InteractiveGrid(width, height, length, cellSize, offset);
        grid.CenterCamera();
    }

    private void Update()
    {

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
