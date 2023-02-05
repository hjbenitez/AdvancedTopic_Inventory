using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    bool rotating;

    // the center of the grid
    Vector3 backpack = new Vector3(30, 30, 30);

    // the radius the camera rotates around the backpack 
    float radius = 100;

    //sensitivity for each axis 
    public float sensitivityX;
    public float sensitivityY;

    //offset of the camera height
    float elevationOffset = 0;

    //angle of the horizontal rotation
    public float angleX = 0;

    //horizontal position of the camera
    Vector3 horizontalPosition;

    //min and max elevation height
    float elevationMin;
    float elevationMax;

    bool initialize;

    // Start is called before the first frame update
    void Start()
    {
        //setting the min and maxes
        elevationMin = elevationOffset - 30;
        elevationMax = elevationOffset + 30;

        rotating = true;
        initialize = false;
    }

    // Update is called once per frame
    void Update()
    {
        //starts rotating on the right button down
        if (Input.GetMouseButtonDown(1))
        {
            rotating = true;
        }

        //stops on right mouse button up
        if (Input.GetMouseButtonUp(1))
        {
            rotating = false;
        }

        //checks if the elevation changes past the min and maxes
        if (elevationOffset < elevationMin)
        {
            elevationOffset = elevationMin;
        }

        if (elevationOffset > elevationMax)
        {
            elevationOffset = elevationMax;
        }
    }


    private void LateUpdate()
    {
        //when the right mouse button is held down
        if (rotating)
        {
            float horizontalDirection = Input.GetAxis("Mouse X"); //gets the mouse horizontal value between -1 and 1
            float verticalDirection = Input.GetAxis("Mouse Y"); //gets the mouse vertical value between -1 and 1

            //calculate the angle to use for the next few math caluclations
            //positive and negative horizontalDirection value to change direction
            angleX += Time.deltaTime * sensitivityX * -horizontalDirection;

            //sets the horizontal position 
            horizontalPosition.Set(
                Mathf.Cos(angleX) * radius,
                elevationOffset,
                Mathf.Sin(angleX) * radius
            );

            elevationOffset += (sensitivityY * -verticalDirection); //changes the elevation height based on mouse movement 

            transform.position = backpack + horizontalPosition; //sets the position of the camera 
            transform.LookAt(backpack); //looks at the inventory
        }

        if(!initialize)
        {
            rotating = false;
            initialize = true;
        }
    }
}

