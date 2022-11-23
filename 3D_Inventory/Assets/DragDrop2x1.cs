using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop2x1 : MonoBehaviour
{
    public Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        size = new Vector3(Mathf.RoundToInt(transform.GetComponent<BoxCollider>().bounds.size.x),
            Mathf.RoundToInt(transform.GetComponent<BoxCollider>().bounds.size.y),
            Mathf.RoundToInt(transform.GetComponent<BoxCollider>().bounds.size.z)); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
