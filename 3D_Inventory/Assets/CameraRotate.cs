using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    bool rotating;

    Vector3 backpack = new Vector3(30, 30, 30);

    float radius = -100;

    public float sensitivityX;
    public float sensitivityY = 0.5f;

    float elevationOffset = 0;

    public float angleX = 0;

    Vector3 horizontalPosition;

    float elevationMin;
    float elevationMax;

    // Start is called before the first frame update
    void Start()
    {
        elevationMin = elevationOffset - 30;
        elevationMax = elevationOffset + 30;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            rotating = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rotating = false;
        }

        if(elevationOffset < elevationMin)
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
        if (rotating)
        {
            
            float horizontalDirection = Input.GetAxis("Mouse X");
            float verticalDirection = Input.GetAxis("Mouse Y");

            angleX += Time.deltaTime * sensitivityX * horizontalDirection;

            horizontalPosition.Set(
                Mathf.Cos(angleX) * radius,
                elevationOffset,
                Mathf.Sin(angleX) * radius    
            );

            elevationOffset += (sensitivityY * verticalDirection);

            transform.position = backpack + horizontalPosition;
            transform.LookAt(backpack);     
        }
    }

    private void OnDrawGizmos()
    {

    }
}

