using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    bool rotating;

    Vector3 backpack = new Vector3(30, 30, 0);

    float radius = -45.1f;

    public float rotationSpeed = 25f;

    float elevationOffset = 15;

    Vector3 positionOffset;
    public float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    private void LateUpdate()
    {
        if (rotating)
        {
            float horizontalDirection = Input.GetAxis("Mouse X");
            //float verticalDirection = Input.GetAxis("Mouse Y");

            positionOffset.Set(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                elevationOffset,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius    
            );


            transform.position = backpack + positionOffset;
            angle += Time.deltaTime * rotationSpeed * horizontalDirection;
            transform.LookAt(backpack);
            
        }
    }
}
