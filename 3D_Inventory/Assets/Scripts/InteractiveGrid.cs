using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGrid
{
    int width;
    int height;
    int length;
    
    float cellSize;
    Vector3 originPosition;

    int[,,] gridArray;
    
    
    TextMesh[,,] debugTextArray; //used for debugging | displays true/false text if an item is occupying a space

    public bool showText = true;
    public bool[,,] inventorySpace;

    public InteractiveGrid(int width, int height, int length, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.length = length;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height, length];
        inventorySpace = new bool[width, height, length];
        debugTextArray = new TextMesh[width, height, length];


        //used to track if a space was registering being taken up by a space
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    debugTextArray[x, y, z] = CreateWorldText(null, inventorySpace[x, y, z].ToString(), getWorldPosition(x, y, z) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.blue, TextAnchor.MiddleCenter);
                }

            }
        }


    }
    private Vector3 getWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize + originPosition;
    }

    private void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public void setValue(int x, int y, int z, int value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y, z] = value;
            debugTextArray[x, y, z].text = gridArray[x, y, z].ToString();
        }
    }

    public void setValue(int x, int y, int z, bool value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            inventorySpace[x, y, z] = value;
            debugTextArray[x, y, z].text = inventorySpace[x, y, z].ToString();
        }
    }

    public void setValue(Vector3 worldPosition, int value)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        setValue(x, y, z, value);
    }

    public int getValue(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && x < width && y < height && z >= 0 && z <= length)
        {
            return gridArray[x, y, z];
        }

        else
        {
            return -1;
        }
    }


    public int getValue(Vector3 worldPosition)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        return getValue(x, y, z);

    }

    public void CenterCamera()
    {
        Camera camera = Camera.main;
        camera.transform.position = new Vector3((width * cellSize) / 2, (height * cellSize) / 2, (length * cellSize) / 2) + originPosition;
    }

    public Vector3 getOrigin()
    {
        return originPosition;
    }

    public Vector3 getSize()
    {
        return new Vector3(width, height, length);
    }

    public TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = TextAlignment.Center;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = 5000;
        return textMesh;
    }
}
