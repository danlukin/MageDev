using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float xPos = target.transform.position.x;
        if (Math.Abs(xPos) > 7.5f)
            if (xPos < 0)
                xPos = -7.5f;
            else
                xPos = 7.5f;

        float yPos = target.transform.position.y;
        if (Math.Abs(yPos) > 5f)
            if (yPos < 0)
                yPos = -5f;
            else
                yPos = 5f;

        transform.position = new Vector3(xPos, yPos, -10);

    }
}
