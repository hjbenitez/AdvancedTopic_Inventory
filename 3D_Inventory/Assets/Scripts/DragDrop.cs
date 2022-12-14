using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public Vector3[] centers; //array for all the centers of the object (every 1x1 it is composed of)

    public GridManager gridManager; //the grid manager game object, set in the Inspector
    public int numberOfCenters; //essentially tells the object how many 1x1s are inside of it (set in the Inspector)
    public List<Transform> blocks = new List<Transform>(); //a list of all the blocks composed in the item

    //Grid values
    float cellSize;
    int height;
    int width;
    int length;
    Vector3 offset;

    public Vector3[] lastPositions; //array that stores the last position for each block before moving
    Vector3 targetPos; //the target position of where the item will go when dropped
    public bool dragging = false; //tells the game if the block is dragging or not
    Vector3 lastRotation; //stores the last rotation of the item

    // Start is called before the first frame update
    void Start()
    {
        //grabs values from the grid created
        width = gridManager.width;
        height = gridManager.height;
        length = gridManager.length;
        cellSize = gridManager.cellSize;
        offset = gridManager.offset;

        targetPos = transform.position; //sets target location to current location

        centers = new Vector3[numberOfCenters]; //creates an array that is the length of the number of blocks inside the item, stores the center points
        lastPositions = new Vector3[numberOfCenters]; //creates an array that is the length of the number of blocks inside the item, stores the last positions
        lastRotation = transform.eulerAngles; //sets the last rotation of the item to its current rotation

        //for each block that is inside the item, add it to the list of blocks
        foreach(Transform child in this.gameObject.transform)
        {
            blocks.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the item is not being moved
        if (!dragging)
        {
            //continiually updates the target position to the current position
            targetPos = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), RoundToNearestGrid(targetPos.z));
            transform.position = targetPos;
        }

        //updates all the centers of the item
        for (int i = 0; i < centers.Length; i++)
        {
            centers[i] = blocks[i].position;

            //Debug - Shows lines from the centers to the origin
            //Debug.DrawLine(offset, centers[i], Color.red);
        }
    }
    //updates the last position to the location the item was picked up AND the last rotation of the item
    private void OnMouseDown()
    {
        for(int i = 0; i < centers.Length; i++)
        {
            lastPositions[i] = new Vector3(RoundToNearestGrid(centers[i].x), RoundToNearestGrid(centers[i].y), RoundToNearestGrid(centers[i].z));
            lastRotation = transform.eulerAngles;
        }
    }

    //runs when the item is being dragged
    private void OnMouseDrag()
    {
        //rotates the item when a specific key is pressed
        //WS - UP/DOWN
        //AD - RIGHT/LEFT
        if(Input.GetKeyDown(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.eulerAngles += new Vector3(0, -90, 0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.localEulerAngles += new Vector3(-90, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.localEulerAngles += new Vector3(90 ,0, 0);
        }

        dragging = true;

        //weird set of calculations that ensures that the screen position and the world position are 1:1 when dragging
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

    //checks all centers to ensure they are inside the grid also checks if the space is currently occupied
    //drops it at the last known correct location when dropped out of bounds or the space is occupied
    public void checkBoundaries()
    {
        //calulates the max dimensions of the grid
        float gridWidth = width * cellSize + offset.x;
        float gridHeight = height * cellSize + offset.y;
        float gridLength = length * cellSize + offset.z;

        //runs for every block inside the item
        for (int i = 0; i < centers.Length; i++)
        {
            //gets the index of where the item is inside the grid
            Vector3 index = new Vector3(Mathf.Round(centers[i].x / cellSize - 1), Mathf.Round(centers[i].y / cellSize - 1), Mathf.Round(centers[i].z / cellSize - 1));

            //checks if the item is inbounds AND if the space is free using the index (returns the inverse of this due to the ! at the beginning)
            if (!(centers[i].x <= gridWidth && centers[i].x >= offset.x &&
            centers[i].y <= gridHeight && centers[i].y >= offset.y &&
            centers[i].z <= gridLength && centers[i].z >= offset.z &&
            gridManager.inventorySpace[(int)index.x, (int)index.y, (int)index.z] == false))
            {
                targetPos = lastPositions[lastPositions.Length-1]; //sets the position to the last position
                transform.eulerAngles = lastRotation; //sets the last rotation to the current rotation
            }          
        }
    }
}
