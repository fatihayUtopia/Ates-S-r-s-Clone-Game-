using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMove : MonoBehaviour
{
    public Transform targetUpperWall, targetlowerWall,mainCamera;
    public CollocateWalls wallsScrpt;
    Vector3 baseRotation;
    float upBound, downBound, centerPoint, positionY;
    float[] positionsY = new float[3];
    float[] rotationsX = new float[2];
    void Start()
    {

        rotationsX[0] = 6;
        rotationsX[1] = -30;
        baseRotation = new Vector3(6, 90, 0);
    }

    void OnEnable()
    {

        upBound = (targetUpperWall.position.y - ((transform.localScale.y + targetUpperWall.localScale.y) * 0.5f));
        downBound = (targetlowerWall.position.y + ((transform.localScale.y + targetlowerWall.localScale.y) * 0.5f));
        centerPoint = (upBound + downBound) * 0.5f;
        positionY = centerPoint;

        if (wallsScrpt.level == 3 || wallsScrpt.level == 4)
        {
            transform.rotation = Quaternion.Euler(rotationsX[Random.Range(0, 2)], baseRotation.y, baseRotation.z);
        }

        transform.position = new Vector3(targetUpperWall.position.x, positionY, transform.position.z);

    }
}
