﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorExt : MonoBehaviour
{
    public static float vertDist(Vector2 a, Vector2 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
    }
}
