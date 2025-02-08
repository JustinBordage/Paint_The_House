using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace LvlEditor
{
    public enum EditorType { Wall, Brush, Wood, Window, Vent, Bush, Mail, Power, Door }

    public class EditorManager : MonoBehaviour
    {
        //Editor Scene Object Management
        public static EditorType BrushTile { get; private set; } = EditorType.Wall;
        public static EditorManager Instance = null;
        private GameObject editorSetup = null;

        //Wall Object Array
        private Transform[] wallList = null;
        private int currWall = 0;

        //Level Dimensions
        public static int widthFB, widthLR, height;

        //File Saving
        private const string filename = "default";
        private const string path = "Assets\\";

        GameObject playtestLvl = null;

        void Awake()
        {
            if (Instance == null)
            {
                //Sets the intial tile
                GameObject selector = GameObject.FindGameObjectWithTag("TileSelector");
                Dropdown tileSelector = selector.GetComponent<Dropdown>();
                BrushTile = (EditorType)tileSelector.value;

                //Fetches a reference to the setup manager
                editorSetup = GameObject.FindGameObjectWithTag("EditorSetup");

                //Singleton Pattern
                Instance = this;
            }
        }

        public Transform[] GetWallList()
        {
            if (wallList == null)
            {
                wallList = new Transform[4];

                //Sets up a reference to each of the wall parents
                GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

                int wallIndex = 0;
                for (int i = 0; i < 4; i++)
                {
                    switch (walls[i].name)
                    {
                        case "FrontWall":
                            wallIndex = 0;
                            break;
                        case "RightWall":
                            wallIndex = 1;
                            break;
                        case "RearWall":
                            wallIndex = 2;
                            break;
                        case "LeftWall":
                            wallIndex = 3;
                            break;
                    }

                    wallList[wallIndex] = walls[i].transform;
                }
            }

            return wallList;
        }

        public void SwapTile(int dropDownIndex)
        {
            //Sets the tile type
            BrushTile = (EditorType)dropDownIndex;
        }

        public void WipeEditor()
        {
            foreach (Transform wall in wallList)
            {
                for (int tile = 0; tile < wall.childCount; tile++)
                {
                    Destroy(wall.GetChild(tile).gameObject);
                }
            }

            editorSetup.SetActive(true);
        }

        public void SwapWall(int indexMod)
        {
            //Disables the previous wall
            wallList[currWall].gameObject.SetActive(false);

            //Changes the index
            currWall += indexMod;

            //Wrap around
            if (currWall >= 4)
                currWall = 0;
            else if (currWall < 0)
                currWall = 3;

            //Enables the new wall
            wallList[currWall].gameObject.SetActive(true);
        }

        /** Parses the coordinates from the tile game object's name.
         *
         *  @remark "tileName" is formatted as follows: "x_y_Tile" */
        private Vector2Int ParseTileCoords(string tileName)
        {
            var items = tileName.Split('_');
            if (items.Length != 3 || items[2] != "Tile")
            {
                Debug.LogErrorFormat("Invalid tile coords for tile '{0}'", tileName);
                return new Vector2Int(-1, -1);
            }

            int xPos = int.Parse(items[0]);
            int yPos = int.Parse(items[1]);
            return new Vector2Int(xPos, yPos);
        }
        
        private static T[,] FillArray<T>(T[,] array, T fillValue)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = fillValue;
                }
            }

            return array;
        }
        
        private static void PrintArray<T>(T[,] array)
        {
            var map = "";
            for (int i = 0; i < array.GetLength(0); i++)
            {
                var line = "";
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    line += array[i, j] + ", ";
                }
                
                map += line.TrimEnd(',', ' ') + "\n";
            }

            Debug.Log(map);
        }
        
        // private static TileType[,] FillTileArray(TileType[,] array, TileType fillValue)
        // {
        //     var length = array.GetLength(0);
        //     for (int i = 0; i < length; i++)
        //     {
        //         var row = new int[length];
        //         array.SetValue(i, row);
        //         for (int j = 0; j < array.GetLength(1); j++)
        //         {
        //             array[i, j] = fillValue;
        //         }
        //     }
        //
        //     return array;
        // }
        //
        // private static int[,] FillIntArray(int[,] array, int fillValue)
        // {
        //     for (int i = 0; i < array.GetLength(0); i++)
        //     {
        //         array.SetValue(i, new int[array.GetLength(0)]);
        //         for (int j = 0; j < array.GetLength(1); j++)
        //         {
        //             array[i, j] = fillValue;
        //         }
        //     }
        //
        //     return array;
        // }
        
        public HouseTileInfo[] WallToTileMap(Transform wall)
        {
            int childIndex;
            Vector2Int tileCoords;
            EditorTile wallTile;
            List<HouseTileInfo> tileList = new List<HouseTileInfo>();
        
            // Populates the wall tile list
            for (childIndex = 0; childIndex < wall.childCount; childIndex++)
            {
                wallTile = wall.GetChild(childIndex).GetComponent<EditorTile>();
                var tileType = TileConverter.convert(wallTile.type);

                if (tileType == TileType.Wall) continue;
                
                tileCoords = ParseTileCoords(wallTile.name);
                tileList.Add(new HouseTileInfo(tileCoords, tileType));
            }
            
            return tileList.ToArray();
        }
        
        // public int[,] WallToTileMapAlt(Transform wall, int width, int height)
        // {
        //     int childIndex;
        //     Vector2Int tileCoords;
        //     EditorTile wallTile;
        //     int[,] tileMap = FillArray(new int[width,height], (int) TileType.Wall);
        //
        //     // Populates the wall tile map
        //     for (childIndex = 0; childIndex < wall.childCount; childIndex++)
        //     {
        //         wallTile = wall.GetChild(childIndex).GetComponent<EditorTile>();
        //         var tileType = TileConverter.convert(wallTile.type);
        //
        //         if (tileType == TileType.Wall) continue;
        //         
        //         tileCoords = ParseTileCoords(wallTile.name);
        //         tileMap[tileCoords.x, tileCoords.y] = (int) tileType;
        //     }
        //
        //     return tileMap;
        // }

        public Texture2D WallToPNG(Transform wall, int offset, Texture2D levelTexture)
        {
            int childIndex;
            Vector2Int tileCoords;
            EditorTile wallTile;

            //Populates the wall tile colors
            for (childIndex = 0; childIndex < wall.childCount; childIndex++)
            {
                wallTile = wall.GetChild(childIndex).GetComponent<EditorTile>();

                tileCoords = ParseTileCoords(wallTile.name);

                levelTexture.SetPixel(tileCoords.x + offset, tileCoords.y, wallTile.TileColor);
            }

            //Applies the pixel changes
            levelTexture.Apply();

            return levelTexture;
        }

        private void formatLevel()
        {
            HouseLevel houseLevel = new HouseLevel();
            houseLevel.height = height;
            houseLevel.width = widthFB;
            houseLevel.depth = widthLR;
            
            

        }
        
        public void SaveLevel()
        {
            //Indexing Variables
            int y;
            int offset = 0;
            int wallWidth;
            int wallWidthAlt;
            
            HouseLevel houseLevel = new HouseLevel
            {
                width = widthFB,
                height = height,
                depth = widthLR
            };

            //Texture Creation
            Texture2D levelTexture = new Texture2D(widthFB * 2 + widthLR * 2 + 3, height);
            levelTexture.alphaIsTransparency = true;

            //Scene Objects
            Transform wall;

            for (int wallIndex = 0; wallIndex < 4; wallIndex++)
            {
                //Retrieves the wall
                wall = wallList[wallIndex];

                levelTexture = WallToPNG(wall, offset, levelTexture);
                
                wallWidth = wallIndex % 2 == 0 ? widthFB : widthLR;
                
                var tileMap = WallToTileMap(wall);
                
                switch (wallIndex)
                {
                    case 0:
                        Debug.Log("Setting Front Tiles");
                        houseLevel.frontTiles = tileMap;
                        break;
                    case 1:
                        Debug.Log("Setting Right Tiles");
                        houseLevel.rightTiles = tileMap;
                        break;
                    case 2:
                        Debug.Log("Setting Rear Tiles");
                        houseLevel.backTiles = tileMap;
                        break;
                    case 3:
                        Debug.Log("Setting Left Tiles");
                        houseLevel.leftTiles = tileMap;
                        break;
                }

                //Offsets by the wall's width (I get the feeling that this is a bug, shouldn't it use a modulus?)
                if (wallIndex != 1)
                {
                    wallWidthAlt = widthFB;
                    offset += widthFB;
                }
                else
                {
                    wallWidthAlt = widthLR;
                    offset += widthLR;
                }

                if (wallWidthAlt != wallWidth)
                {
                    Debug.LogErrorFormat("[{0}] Wall Width Mismatch: {1} | {2}", wallIndex, wallWidth, wallWidthAlt);
                }

                //Adds a transparent spacer
                for (y = 0; y < height; y++)
                {
                    levelTexture.SetPixel(offset, y, Color.clear);
                }

                //Accounts for spacer
                offset += 1;
            }

            //Applies the pixel changes
            levelTexture.Apply();

            //Saves the Texture as a PNG
            SaveLevelAsPNG(levelTexture, path + filename + ".png");
            
            string timeNow = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HHmm'Z'");
            SaveLevelAsJSON(houseLevel, path + filename + "_" + timeNow + ".json");
        }

        private void SaveLevelAsPNG(Texture2D _texture, string _fullPath)
        {
            byte[] _bytes = _texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(_fullPath, _bytes);
            Debug.Log(_bytes.Length / 1024 + "Kb was saved as: \'" + _fullPath + "\'");
        }
        
        private void SaveLevelAsJSON(HouseLevel levelData, string _fullPath)
        {
            var serializedLevelData = JsonUtility.ToJson(levelData);
            Debug.Log(serializedLevelData);
            
            byte[] _bytes = Encoding.ASCII.GetBytes(serializedLevelData);
            System.IO.File.WriteAllBytes(_fullPath, _bytes);
            Debug.Log(_bytes.Length / 1024 + "Kb was saved as: \'" + _fullPath + "\'");
        }

        public void StartPlaytest()
        {
            //Determines the width of the test wall
            int width;
            if (currWall == 0 || currWall == 2)
                width = widthFB;
            else
                width = widthLR;

            //Converts the current wall to a PNG
            Texture2D levelTexture = new Texture2D(width, height);
            levelTexture = WallToPNG(wallList[currWall], 0, levelTexture);

            //Creates a level parent object (to make destruction easier later)
            playtestLvl = new GameObject("Playtest_Level");

            //Creates the playtest level
            GameObject generator = GameObject.FindGameObjectWithTag("LevelGenerator");
            LevelGenerator lvlGen = generator.GetComponent<LevelGenerator>();
            lvlGen.SpawnTiles(levelTexture, playtestLvl.transform);

            wallList[currWall].gameObject.SetActive(false);
        }

        public void StopPlaytest()
        {
            if(playtestLvl != null)
            {
                Destroy(playtestLvl);
                playtestLvl = null;

                wallList[currWall].gameObject.SetActive(true);
            }
        }

        void OnDestroy()
        {
            Instance = null;
        }
    }
}


