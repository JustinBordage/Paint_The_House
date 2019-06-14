using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LvlEditor;

public class EditorTile : MonoBehaviour
{
    public Color TileColor { get { return getTileColor(); } }
    public EditorType type { get; private set; } = EditorType.Wall;
    Image tileImage = null;

    void Awake()
    {
        tileImage = GetComponent<Image>();
        PaintTile();
    }
    
    void OnMouseOver()
    {
        if(Input.GetMouseButton(0))
        {
            SwapTile();
        }
    }

    Color getTileColor()
    {
        Color tileColor = Color.white;

        switch (type)
        {
            case EditorType.Brush:
                tileColor = Color.red;
                break;
            case EditorType.Door:
                tileColor = ColorExt.brown;
                break;
            case EditorType.Window:
                tileColor = Color.cyan;
                break;
            case EditorType.Wood:
                tileColor = Color.yellow;
                break;
            case EditorType.Power:
                tileColor = Color.grey;
                break;
            case EditorType.Vent:
                tileColor = Color.black;
                break;
            case EditorType.Mail:
                tileColor = Color.magenta;
                break;
            case EditorType.Bush:
                tileColor = Color.green;
                break;
            default:
                if (type != EditorType.Wall)
                    Debug.Log("Unknown TileType Index - \'" + type.ToString() + "\'");
                break;
        }

        return tileColor;
    }

    void SwapTile()
    {
        type = EditorManager.BrushTile;

        PaintTile();
    }

    void PaintTile()
    {
        Color tColor = getTileColor();

        tileImage.color = tColor;
    }
}
