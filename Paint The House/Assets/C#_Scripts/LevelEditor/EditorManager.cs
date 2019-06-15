using System.Collections;
using System.Collections.Generic;
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

        public Transform[] getWallList()
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

        public void wipeEditor()
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

        public void swapWall(int indexMod)
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

        private Vector2Int parseTileCoords(string tileName)
        {
            int x = -1, y = -1;
            Vector2Int tileCoords = Vector2Int.zero;

            for (x = 0; x < tileName.Length; x++)
            {
                if (tileName[x] == '_')
                {
                    tileCoords.x = int.Parse(tileName.Substring(0, x));
                    break;
                }
            }

            for (y = x + 1; y < tileName.Length; y++)
            {
                if (tileName[y] == '_')
                {
                    tileCoords.y = int.Parse(tileName.Substring(x + 1, y - (x + 1)));
                    break;
                }
            }

            return tileCoords;
        }

        public Texture2D wallToPNG(Transform wall, int offset, Texture2D levelTexture)
        {
            int tile;
            Vector2Int tileCoords;
            EditorTile wallTile;

            //Populates the wall tile colors
            for (tile = 0; tile < wall.childCount; tile++)
            {
                wallTile = wall.GetChild(tile).GetComponent<EditorTile>();

                tileCoords = parseTileCoords(wallTile.name);

                levelTexture.SetPixel(tileCoords.x + offset, tileCoords.y, wallTile.TileColor);
            }

            //Applies the pixel changes
            levelTexture.Apply();

            return levelTexture;
        }

        public void SaveLevel()
        {
            //Indexing Variables
            int y;
            int offset = 0;

            //Texture Creation
            Texture2D levelTexture = new Texture2D(widthFB * 2 + widthLR * 2 + 3, height);
            levelTexture.alphaIsTransparency = true;

            //Scene Objects
            Transform wall;

            for (int wallIndex = 0; wallIndex < 4; wallIndex++)
            {
                //Retrieves the wall
                wall = wallList[wallIndex];

                levelTexture = wallToPNG(wall, offset, levelTexture);

                //Offsets by the wall's width
                if (wallIndex != 1)
                {
                    offset += widthFB;
                }
                else
                    offset += widthLR;

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
        }

        private void SaveLevelAsPNG(Texture2D _texture, string _fullPath)
        {
            byte[] _bytes = _texture.EncodeToPNG();
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
            levelTexture = wallToPNG(wallList[currWall], 0, levelTexture);

            //Creates a level parent object (to make destruction easier later)
            playtestLvl = new GameObject("Playtest_Level");

            //Creates the playtest level
            GameObject generator = GameObject.FindGameObjectWithTag("LevelGenerator");
            LevelGenerator lvlGen = generator.GetComponent<LevelGenerator>();
            lvlGen.spawnTiles(levelTexture, playtestLvl.transform);

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


