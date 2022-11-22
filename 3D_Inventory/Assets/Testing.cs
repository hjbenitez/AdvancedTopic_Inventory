using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private InteractiveGrid grid;
    private void Start()
    {
        grid = new InteractiveGrid(5, 5, 5, 10f, new Vector3(5, 5, 5));
        grid.CenterCamera();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //grid.setValue(GetMouseWorldPosition(), 56);
            grid.setValue(GetMouseWorldPosition(), 56);

        }

        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.getValue(GetMouseWorldPosition()));
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
