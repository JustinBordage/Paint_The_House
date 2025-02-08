using System;
using LvlEditor;

[Serializable]
public enum TileType
{
    Wall = "WALL",
    Brush = "WALL2",
    DoorU = "WALL3",
    DoorL = "WALL4",
    Window = "WALL5",
    Wood = "WALL6",
    Mail = "WALL7",
    Power = "WALL8",
    Bush = "WALL9",
    Vent= "WALL10"
}

public class TileConverter
{
    public static TileType convert(EditorType tile)
    {
        switch (tile)
        {
            case EditorType.Brush:
                return TileType.Brush;
            case EditorType.Door:
                return TileType.DoorL;
            case EditorType.Window:
                return TileType.Window;
            case EditorType.Wood:
                return TileType.Wood;
            case EditorType.Power:
                return TileType.Power;
            case EditorType.Vent:
                return TileType.Vent;
            case EditorType.Mail:
                return TileType.Mail;
            case EditorType.Bush:
                return TileType.Bush;
            case EditorType.Wall:
            default:
                return TileType.Wall;
        }
    }
}