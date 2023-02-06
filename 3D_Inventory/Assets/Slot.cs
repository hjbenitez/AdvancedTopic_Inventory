using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int slotID = 1;
    public string item;
    public Controller controller;
    public bool dropped;
    public bool dragging = false;
    GameObject temp;


    // Start is called before the first frame update
    void Start()
    {
        dropped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dropped)
        {
            if (controller.IsPointerOverUIElement() && dragging && item != null)
            {
                temp = Instantiate(Resources.Load(item, typeof(GameObject)) as GameObject);
                temp.name = item;
                temp.GetComponent<DragDrop>().dragging = true;

                item = null;
                dragging = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                dragging = true;
            }

            else if (Input.GetMouseButtonUp(0))
            {
                temp.GetComponent<DragDrop>().mouseUp = true;
                dropped = false;
                temp = null;
                dragging = false;
            }
        }

    }
}
