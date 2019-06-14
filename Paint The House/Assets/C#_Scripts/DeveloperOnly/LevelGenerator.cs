using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // The X and Y coordinates of the
    public Vector3 CenterPoint = new Vector3(0, 0, 0);
    public Vector2 tileSize = Vector2.one * 1f;

    const string mDirectory = "Prefabs/Tiles/";

    //Hex codes letters must be capitalized
    const string WALL_TILE = "FFFFFF";      //Wall
    const string WOOD_TILE = "FFEB04";      //Wood
    const string WINDOW_TILE = "00FFFF";    //Window
    const string BRUSH_TILE = "FF0000";     //Brush
    const string VENT_TILE = "000000";      //Vent X
    const string BUSH_TILE = "00FF00";      //Bush X
    const string MAIL_TILE = "FF00FF";      //Mail 
    const string POWER_TILE = "808080";     //Power
    const string DOOR_TILE = "654321";      //Door X
    //FF0000
    //654321
     //00FFFF
     //FFEB04
     //808080
     //000000
     //FF00FF
    //00FF00

    public void generateWall()
    {
        SpawnWall("Level1", WallDir.Front);
    }

    void applyWallOffset(Transform wall, WallDir direction)
    {
        float angle = 0f;
        Vector3 offset = Vector3.zero;

        switch(direction)
        {
            case WallDir.Front:
                angle = 0f;
                offset = new Vector3(0f, 0f, 0f);
                break;
            case WallDir.Right:
                angle = 0f;
                offset = new Vector3(0f, 0f, 0f);
                break;
            case WallDir.Rear:
                angle = 0f;
                offset = new Vector3(0f, 0f, 0f);
                break;
            case WallDir.Left:
                angle = 0f;
                offset = new Vector3(0f, 0f, 0f);
                break;
        }

        wall.rotation = Quaternion.Euler(Vector3.up * angle);
        wall.position = CenterPoint + offset;

    }

    void SpawnWall(string directory, WallDir direction)
    {
        Texture2D level = Resources.Load<Texture2D>(directory);// + "/" + direction.ToString());

        //Creates the wall parent object and adjusts it's rotation/offset
        Transform wall = new GameObject(direction.ToString()).transform;
        applyWallOffset(wall, direction);

        spawnTiles(level, wall);
    }

    void spawnTiles(Texture2D levelImg, Transform parent)
    {
        float width = levelImg.width;
        float height = levelImg.height;

        Vector2 centerPos = new Vector2((tileSize.x * width - tileSize.x) * 0.5f, (tileSize.y * height - tileSize.y) * 0.5f);
        Vector3 tilePos = Vector3.zero;
        GameObject paintObj = null;

        int x, y;
        for (x = 0; x < width; x++)
        {
            for (y = 0; y < height; y++)
            {
                List<GameObject> objList = getTilePrefab(levelImg.GetPixel(x, y));

                foreach (GameObject prefab in objList)
                {
                    tilePos = new Vector3(-centerPos.x + x * tileSize.x, -centerPos.y + y * tileSize.y, 0f);

                    paintObj = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);

                    paintObj.name = prefab.name;

                    if (paintObj.name != "Brush")
                        paintObj.name = x.ToString() + "_" + y.ToString() + "_" + paintObj.name;

                    paintObj.transform.localPosition = tilePos;
                }
            }
        }
    }

    List<GameObject> getTilePrefab(Color pixelColor)
    {
        string Hexcode = ColorUtility.ToHtmlStringRGB(pixelColor);
        List<GameObject> objList = new List<GameObject>();
        
        switch (Hexcode)
        {
            case WALL_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "Wall_Panel"));
                break;
            case BRUSH_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "Brush"));
                objList.Add(Resources.Load<GameObject>(mDirectory + "Wall_Panel"));
                break;
            case DOOR_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "DoorLower_Panel"));
                break;
            case WINDOW_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "Window_Panel"));
                break;
            case WOOD_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "Wood_Panel"));
                break;
            case POWER_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "Power_Panel"));
                break;
            case MAIL_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "Mail_Panel"));
                break;
            case BUSH_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "Bush_Panel"));
                break;
            case VENT_TILE:
                objList.Add(Resources.Load<GameObject>(mDirectory + "Vent_Panel"));
                break;
            default:
                Debug.LogWarning("ColorHex(" + Hexcode + ") - Maps may be improperly read if, 1) Image's Compression setting isn't set to \'None\',\n" +
                                                     "2) Generate Mip Maps is turned off, 3) \'Non Power of 2\' is not set to \'None\', or 4) HexColor letters are not capitalized");

                break;
        }

        return objList;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
