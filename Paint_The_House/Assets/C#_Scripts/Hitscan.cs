using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitscan : MonoBehaviour
{
    public static Vector3 ScreenPos(Vector3 position)
    {
        Camera mainCam = Camera.main;

        Ray ray = mainCam.ScreenPointToRay(position);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 25f))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
