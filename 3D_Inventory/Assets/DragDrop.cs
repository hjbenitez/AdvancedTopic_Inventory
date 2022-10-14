using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    float startX;
    float startY;

    bool dragging = false;
    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
        float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
    }
    /*
    public void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 10;
        startX = mousePosition.x - transform.localPosition.x;
        startY = mousePosition.y - transform.localPosition.y;

        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    public void DragObject()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 10;
        transform.localPosition = new Vector3(mousePosition.x - startX, mousePosition.y - startY, transform.localPosition.z);
    }
    */
}
