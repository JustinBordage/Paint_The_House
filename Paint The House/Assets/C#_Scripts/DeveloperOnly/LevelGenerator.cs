using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // The X and Y coordinates of the 
    public Vector3 CenterPoint = new Vector3(0, 0, 0);

    //Hex codes letters must be capitalized
    const string SHEEP_TILE = "FF00FF";     //Sheep
    const string DOG_TILE = "00FFFF";     //Dog
    const string LOG_TILE = "FFFF00";     //Log
    const string SHARK_TILE = "FF0000";     //PCS
    const string WALL_TILE = "000000";     //Wall
    const string WATER_TILE = "0000FF";     //Water
    const string END_TILE = "00FF00";     //Grass
    const string EMPTY_TILE = "FFFFFF";     //Empty

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
