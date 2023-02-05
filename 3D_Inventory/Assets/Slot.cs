using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int slotID = 1;
    public GameObject item;
    public Controller controller;
    public bool dropped;

    public bool dragging = false;
    // Start is called before the first frame update
    void Start()
    {
        dropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(dropped)
        {
            if(controller.IsPointerOverUIElement() && dragging && item != null)
            {
                item.SetActive(true);
                item.GetComponent<DragDrop>().dragging = true;
                item = null;
                dropped = false;
            }

            if(Input.GetMouseButtonDown(0))
            {
                dragging = true;
            }

            else if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
        }

    }
}
