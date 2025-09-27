using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float xLimit;
    [SerializeField] private float yLimit;
    
    void Update()
    {

        float xPos = target.transform.position.x;
        if (Math.Abs(xPos) > xLimit)
            if (xPos < 0)
                xPos = -xLimit;
            else
                xPos = xLimit;

        float yPos = target.transform.position.y;
        if (Math.Abs(yPos) > yLimit)
            if (yPos < 0)
                yPos = -yLimit;
            else
                yPos = yLimit;

        transform.position = new Vector3(xPos, yPos, -10);

    }
}
