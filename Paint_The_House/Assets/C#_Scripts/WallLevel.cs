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
        Invoke("RegisterLevel", 0.001f);
    }

    //Registers all the level's components
    //(which are children of this object)
    public void RegisterLevel()
    {
        wallList = GetComponentsInChildren<WallTile>();
        levelBrush = GetComponentInChildren<PaintBrush>().gameObject;
        IsLvlPlayable(name[5] == '0');
    }

    public bool VerifyWalls()
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

    public void IsLvlPlayable(bool playable)
    {
        levelBrush.gameObject.SetActive(playable);
    }
}
