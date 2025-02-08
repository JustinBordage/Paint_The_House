using System;
using UnityEngine;


public class HouseLevel
{
    public int height;
    /** Front & back faces */
    public int width;
    /** Left & right faces */
    public int depth;
    
    public HouseTileInfo[] frontTiles;
    public HouseTileInfo[] rightTiles;
    public HouseTileInfo[] backTiles;
    public HouseTileInfo[] leftTiles;
}

[Serializable]
public struct HouseTileInfo
{
    public int x;
    public int y;
    public TileType tileType;
    
    public HouseTileInfo(Vector2Int pos, TileType tileType)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.tileType = tileType;
    }
}