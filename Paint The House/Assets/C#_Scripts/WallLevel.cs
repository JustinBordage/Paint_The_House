using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLevel : MonoBehaviour
{
    WallTile[] wallList;

    // Start is called before the first frame update
    void Start()
    {
        //Basically Generate Level Before Loading the scene
        Invoke("populateList", 0.01f);
    }

    public void populateList()
    {
        wallList = GetComponentsInChildren<WallTile>();
    }

    public bool verifyWalls()
    {
        //Exits if a wall is not painted
        foreach (WallTile wall in wallList)
        {
            if (!wall.isPainted)
                return false;
        }

        //Execute level End Code
        return true;
    }
}
