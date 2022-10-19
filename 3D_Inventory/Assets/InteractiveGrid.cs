using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGrid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;

    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public InteractiveGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateWorldText(null, gridArray[x, y].ToString(), getWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x, y + 1), Color.red, 100f);
                Debug.DrawLine(getWorldPosition(x, y), getWorldPosition(x + 1, y), Color.red, 100f);
            }
            Debug.DrawLine(getWorldPosition(0, height), getWorldPosition(width, height), Color.red, 100f);
            Debug.DrawLine(getWorldPosition(width, 0), getWorldPosition(width, height), Color.red, 100f);
        }


    }
    private Vector3 getWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void setValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void setValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        setValue(x, y, value);
    }

    public int getValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }

        else
        {
            return -1;
        }
    }


    public int getValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return getValue(x, y);

    }

    public void CenterCamera()
    {
        Camera camera = Camera.main;
        camera.transform.position = new Vector3((width * cellSize) / 2, (height * cellSize) / 2, camera.transform.position.z) + originPosition;
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
