using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    public Vector3[] centers; //array for all the centers of the object (every 1x1 it is composed of)

    public Sprite inventoryIcon;
    public Controller controller;
    public MeshRenderer mesh;
    public bool overUI;
    public bool equipped;
    public bool instantiated;

    public bool mouseDown;
    public bool mouseUp;

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

    float gridWidth;
    float gridLength;
    float gridHeight;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
        mesh = GetComponentInChildren<MeshRenderer>();

        //for each block that is inside the item, add it to the list of blocks
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.tag == "Collider")
            {
                blocks.Add(child);

            }
        }

        numberOfCenters = blocks.Count;

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

        gridWidth = width * cellSize + offset.x;
        gridHeight = height * cellSize + offset.y;
        gridLength = length * cellSize + offset.z;

        overUI = false;
        instantiated = false;
        mouseDown = false;
        mouseUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        overUI = controller.IsPointerOverUIElement();
        //updates all the centers of the item
        for (int i = 0; i < centers.Length; i++)
        {
            centers[i] = blocks[i].position;

            //Debug - Shows lines from the centers to the origin
            //Debug.DrawLine(offset, centers[i], Color.red);
        }

        //OnMouseDown() ------------------------
        if (mouseDown)
        {
            mouseDown = false;
            //loops for e
            for (int i = 0; i < centers.Length; i++)
            {
                lastPositions[i] = new Vector3(RoundToNearestGrid(centers[i].x), RoundToNearestGrid(centers[i].y), RoundToNearestGrid(centers[i].z));
                lastRotation = transform.eulerAngles;
            }
            dragging = true;
        }

        //OnMouseUp() -------------------------
        else if (mouseUp)
        {
            mouseUp = false;
            dragging = false;

            if (!overUI)
            {
                transform.position = checkBoundaries();

            }

            else
            {
                controller.store(this.gameObject);
                controller.droppedItem(true);
                Destroy(this.gameObject);
            }

        }

        //OnMouseDrag() ----------------------------
        if (dragging)
        {
            //weird set of calculations that ensures that the screen position and the world position are 1:1 when dragging
            float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            transform.position = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), RoundToNearestGrid(targetPos.z));
        }
    }

    //updates the last position to the location the item was picked up AND the last rotation of the item
    private void OnMouseDown()
    {
        /*/loops for e
        for (int i = 0; i < centers.Length; i++)
        {
            lastPositions[i] = new Vector3(RoundToNearestGrid(centers[i].x), RoundToNearestGrid(centers[i].y), RoundToNearestGrid(centers[i].z));
            lastRotation = transform.eulerAngles;
        }

        dragging = true;
        */
        mouseDown = true;
    }


    //checks boundaries when dropped
    private void OnMouseUp()
    {
        //dragging = false;
        //transform.position = checkBoundaries();
        mouseUp = true;
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
    //Snaps it to the closet grid edge when dropped out of bounds
    //drops it at the location it was moved from when the space is occupied
    public Vector3 checkBoundaries()
    {
        //used if the item is dropped out of bound 
        //1 in the x, y, or z position means it was dropped out of bounds along that axis
        Vector3 maxBounds = new Vector3(0, 0, 0); //the far edges
        Vector3 minBounds = new Vector3(0, 0, 0); //the close edges

        bool outOfBounds = false; //tracks if the item was dropped out of bounds
        Vector3 newPosition = new Vector3(RoundToNearestGrid(targetPos.x), RoundToNearestGrid(targetPos.y), RoundToNearestGrid(targetPos.z)); //the position the item was dropped at 

        //runs for every block inside the item
        for (int i = 0; i < centers.Length; i++)
        {
            //gets the index of where the item is inside the grid
            Vector3 index = new Vector3(Mathf.Round(centers[i].x / cellSize - 1), Mathf.Round(centers[i].y / cellSize - 1), Mathf.Round(centers[i].z / cellSize - 1));

            //checks if the item was dropped out of bounds
            if (!(centers[i].x <= gridWidth && centers[i].x >= offset.x &&
            centers[i].y <= gridHeight && centers[i].y >= offset.y &&
            centers[i].z <= gridLength && centers[i].z >= offset.z))
            {
                //Check which side the object was dropped out of bounds
                //checks the far x bounds
                if (centers[i].x > gridWidth)
                {
                    maxBounds.x = 1;
                }

                //checks the close x bounds
                else if (centers[i].x < offset.x)
                {
                    minBounds.x = 1;
                }

                //checks the far y bounds
                if (centers[i].y > gridHeight)
                {
                    maxBounds.y = 1;
                }

                //checks the close y bounds
                else if (centers[i].y < offset.y)
                {
                    minBounds.y = 1;
                }

                //checks the far z bounds
                if (centers[i].z > gridLength)
                {
                    maxBounds.z = 1;
                }

                //checks the close z bounds
                else if (centers[i].z < offset.z)
                {
                    minBounds.z = 1;
                }

                outOfBounds = true; //the item was dropped out of bounds
            }

            //checks if it was dropped overlapping another item
            else if (gridManager.inventorySpace[(int)index.x, (int)index.y, (int)index.z] == true && !outOfBounds)
            {
                newPosition = lastPositions[lastPositions.Length - 1]; //sets the position to the position it was moved from
            }
        }

        //when the item is dropped out of bounds
        if (outOfBounds)
        {
            //snaps the x position along the far edge of the grid
            if (maxBounds.x == 1)
            {
                GameObject tempMax = furthestCenterMax(0);
                float distanceToPivot = RoundToNearestGrid(tempMax.transform.position.x) - RoundToNearestGrid(newPosition.x);
                newPosition.x = gridWidth - distanceToPivot - 5;

            }

            //snaps the x position along the close edge of the grid
            else if (minBounds.x == 1)
            {
                GameObject tempMin = furthestCenterMin(0);
                float distanceToPivot = RoundToNearestGrid(newPosition.x) - RoundToNearestGrid(tempMin.transform.position.x);
                newPosition.x = offset.x + distanceToPivot + 5;
            }

            //snaps the y position along the far edge of the grid
            if (maxBounds.y == 1)
            {
                GameObject tempMax = furthestCenterMax(1);
                float distanceToPivot = RoundToNearestGrid(tempMax.transform.position.y) - RoundToNearestGrid(newPosition.y);
                newPosition.y = gridHeight - distanceToPivot - 5;

            }

            //snaps the y position along the close edge of the grid
            else if (minBounds.y == 1)
            {
                GameObject tempMin = furthestCenterMin(1);
                float distanceToPivot = RoundToNearestGrid(newPosition.y) - RoundToNearestGrid(tempMin.transform.position.y);
                newPosition.y = offset.y + distanceToPivot + 5;
            }

            //snaps the z position along the far edge of the grid
            if (maxBounds.z == 1)
            {
                GameObject tempMax = furthestCenterMax(2);
                float distanceToPivot = RoundToNearestGrid(tempMax.transform.position.z) - RoundToNearestGrid(newPosition.z);
                newPosition.z = gridLength - distanceToPivot - 5;

            }

            //snaps the z position along the close edge of the grid
            else if (minBounds.z == 1)
            {
                GameObject tempMin = furthestCenterMin(2);
                float distanceToPivot = RoundToNearestGrid(newPosition.z) - RoundToNearestGrid(tempMin.transform.position.z);
                newPosition.z = offset.z + distanceToPivot + 5;
            }
        }

        //Loops through all the blocks and sees if the new position is currently occupied 
        for (int i = 0; i < blocks.Count; i++)
        {
            Vector3 distanceFromPivot = blocks[blocks.Count - 1].position - blocks[i].position; //calulates the relative position this block is to the pivot point

            Vector3 index = new Vector3(
                Mathf.Round((newPosition.x - distanceFromPivot.x) / cellSize - 1),
                Mathf.Round((newPosition.y - distanceFromPivot.y) / cellSize - 1),
                Mathf.Round((newPosition.z - distanceFromPivot.z) / cellSize - 1)); //calulates the index for the position of this block to check if it is occupied 

            //using the index, check if this space is occupied 
            if (gridManager.inventorySpace[(int)index.x, (int)index.y, (int)index.z] == true)
            {
                newPosition = lastPositions[lastPositions.Length - 1]; //sets the position to the position it was moved from

            }
        }

        return newPosition;
    }

    //finds the block furthest to the far edge
    GameObject furthestCenterMax(int axis)
    {
        Transform furthest = null; //stores the furthest block

        //loops for every block in the item
        for (int i = 0; i < blocks.Count; i++)
        {
            //first block is set as the furthest and is then check amongst the other blocks
            if (i == 0)
            {
                furthest = blocks[i];
            }

            //checks the furthest block from the far x bounds
            else if (axis == 0)
            {
                if (furthest.position.x < blocks[i].position.x)
                {
                    furthest = blocks[i];
                }
            }

            //checks the furthest block from far y bounds
            else if (axis == 1)
            {
                if (furthest.position.y < blocks[i].position.y)
                {
                    furthest = blocks[i];
                }
            }

            //checks the furthest block from far z bounds
            else if (axis == 2)
            {
                if (furthest.position.z < blocks[i].position.z)
                {
                    furthest = blocks[i];
                }
            }
        }

        return furthest.gameObject;
    }

    //finds the block furthest to the close edge
    GameObject furthestCenterMin(float axis)
    {
        Transform furthest = null; //stores the furthest block

        //runs for every block in the item
        for (int i = 0; i < blocks.Count; i++)
        {
            //first block is set as the furthest and is then check amongst the other blocks
            if (i == 0)
            {
                furthest = blocks[i];
            }

            //checks the furthest block from close x bounds
            else if (axis == 0)
            {
                if (furthest.position.x > blocks[i].position.x)
                {
                    furthest = blocks[i];
                }
            }

            //checks the furthest block from close y bounds
            else if (axis == 1)
            {
                if (furthest.position.y > blocks[i].position.y)
                {
                    furthest = blocks[i];
                }
            }

            //checks the furthest block from close z bounds
            else if (axis == 2)
            {
                if (furthest.position.z > blocks[i].position.z)
                {
                    furthest = blocks[i];
                }
            }
        }

        return furthest.gameObject;
    }

    public Sprite getInventoryIcon()
    {
        return inventoryIcon;
    }

}
