using UnityEngine;

public class GenericTile : MonoBehaviour
{
    public enum TileType { Wall, Brush, DoorU, DoorL, WindowU, WindowL, Wood, Mail, Power, Bush, Vent }
    public TileType tileType = TileType.Wall;

    public static GameObject brush = null;                  //Red
    public static GameObject wallTile = null;               //White
    public static GameObject windowUpperTile = null;        //Bright Blue
    public static GameObject windowLowerTile = null;        //Dark Blue
    public static GameObject doorUpperTile = null;          //Bright Brown
    public static GameObject doorLowerTile = null;          //Dark Brown
    public static GameObject woodTile = null;               //Yellow
    public static GameObject mailTile = null;               //Magenta
    public static GameObject bushTile = null;               //Green
    public static GameObject powerTile = null;              //Grey

    // Start is called before the first frame update
    void Awake()
    {
        loadPrefabAssets();

        spawnPrefab();

        Destroy(gameObject, 0.1f);
    }

    void loadPrefabAssets()
    {
        //loads the Assets from the resources
        if (brush == null) brush = Resources.Load<GameObject>("Prefabs/Tiles/Brush");
        if (wallTile == null) wallTile = Resources.Load<GameObject>("Prefabs/Tiles/Wall_Panel");
        if (windowUpperTile == null) windowUpperTile = Resources.Load<GameObject>("Prefabs/Tiles/WindowUpper_Panel");
        if (windowLowerTile == null) windowLowerTile = Resources.Load<GameObject>("Prefabs/Tiles/WindowLower_Panel");
        if (doorUpperTile == null) doorUpperTile = Resources.Load<GameObject>("Prefabs/Tiles/DoorUpper_Panel");
        if (doorLowerTile == null) doorLowerTile = Resources.Load<GameObject>("Prefabs/Tiles/DoorLower_Panel");
        if (woodTile == null) woodTile = Resources.Load<GameObject>("Prefabs/Tiles/Wood_Panel");
        if (mailTile == null) mailTile = Resources.Load<GameObject>("Prefabs/Tiles/Mail_Panel");
        if (powerTile == null) powerTile = Resources.Load<GameObject>("Prefabs/Tiles/Power_Panel");
        if (bushTile == null) bushTile = Resources.Load<GameObject>("Prefabs/Tiles/Bush_Panel");
    }

    void spawnObject(GameObject prefab)
    {
        Transform parent = transform.parent;

        Instantiate(prefab, transform.position, transform.rotation, parent);
    }

    void spawnPrefab()
    {
        GameObject objToSpawn = null;

        switch (tileType)
        {
            case TileType.Wall:
                objToSpawn = wallTile;
                break;
            case TileType.Brush:
                spawnObject(brush);
                objToSpawn = wallTile;
                break;
            case TileType.DoorU:
                objToSpawn = doorUpperTile;
                break;
            case TileType.DoorL:
                objToSpawn = doorLowerTile;
                break;
            case TileType.WindowU:
                objToSpawn = windowUpperTile;
                break;
            case TileType.WindowL:
                objToSpawn = windowLowerTile;
                break;
            case TileType.Wood:
                objToSpawn = woodTile;
                break;
            case TileType.Power:
                objToSpawn = powerTile;
                break;
            case TileType.Mail:
                objToSpawn = mailTile;
                break;
            case TileType.Bush:
                objToSpawn = bushTile;
                break;
        }

        spawnObject(objToSpawn);
    }

    void OnDrawGizmos()
    {
        Color tileColor = Color.white;

        switch(tileType)
        {
            case TileType.Brush:
                tileColor = Color.red;
                break;
            case TileType.DoorU:
                tileColor = ColorExt.lightBrown;
                break;
            case TileType.DoorL:
                tileColor = ColorExt.brown;
                break;
            case TileType.WindowU:
                tileColor = Color.cyan;
                break;
            case TileType.WindowL:
                tileColor = Color.blue;
                break;
            case TileType.Wood:
                tileColor = Color.yellow;
                break;
            case TileType.Power:
                tileColor = Color.grey;
                break;
            case TileType.Mail:
                tileColor = Color.magenta;
                break;
            case TileType.Bush:
                tileColor = Color.green;
                break;
            default:
                if(tileType != TileType.Wall)
                    Debug.Log("Unknown TileType Index - \'" + tileType + "\'");
                break;
        }

        Gizmos.color = tileColor;
        Gizmos.DrawCube(transform.position, transform.lossyScale * 1.0001f);
    }
}
