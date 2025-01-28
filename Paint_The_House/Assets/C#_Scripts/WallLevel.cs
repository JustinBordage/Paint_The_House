using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLevel : MonoBehaviour
{
    WallTile[] wallList;
    GameObject levelBrush;

    // Start is called before the first frame update
    void Start()
    {
        //Basically Generate Level Before Loading the scene
        Invoke("registerLevel", 0.001f);
    }

    //Registers all the level's components
    //(which are children of this object)
    public void registerLevel()
    {
        wallList = GetComponentsInChildren<WallTile>();
        levelBrush = GetComponentInChildren<PaintBrush>().gameObject;
        isLvlPlayable(name[5] == '0');
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

    public void isLvlPlayable(bool playable)
    {
        levelBrush.gameObject.SetActive(playable);
    }
}
