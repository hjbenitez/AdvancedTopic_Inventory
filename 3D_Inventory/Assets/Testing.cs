using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private InteractiveGrid grid;
    private void Start()
    {
        grid = new InteractiveGrid(4, 2, 10f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //grid.setValue(GetMouseWorldPosition(), 56);
            grid.setValue(GetMouseWorldPosition(), 56);

        }
    }


    public Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.gameObject.transform.position.z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);
        Debug.Log(worldPosition);
        return worldPosition;
        
    }

    /*
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    */
}
