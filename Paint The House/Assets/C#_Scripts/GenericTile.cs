using UnityEngine;

public class GenericTile : MonoBehaviour
{
    public enum TileType { Wall, Brush, DoorU, DoorL, Window, Wood, Mail, Power, Bush, Vent }
    public TileType tileType = TileType.Wall;
    private TileType prevTileType = TileType.Wall;

    //brush             Red
    //wallTile          White
    //windowTile        Bright Blue
    //doorUpperTile     Bright Brown
    //doorLowerTile     Dark Brown
    //woodTile          Yellow
    //mailTile          Magenta
    //bushTile          Green
    //ventTile          Black
    //powerTile         Grey

    Material tileMat;
    MeshRenderer tileRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spawnPrefab();

        Destroy(gameObject, 0.1f);
    }

    Material GetMaterial()
    {
        const string directory = "Materials/";
        string materialName = "";

        switch (tileType)
        {
            case TileType.Brush:
                materialName = "Brush";
                break;
            case TileType.DoorU:
                materialName = "DoorUpper";
                break;
            case TileType.DoorL:
                materialName = "DoorLower";
                break;
            case TileType.Window:
                materialName = "Glass";
                break;
            case TileType.Wood:
                materialName = "Wood";
                break;
            case TileType.Power:
                materialName = "Power";
                break;
            case TileType.Mail:
                materialName = "Mail";
                break;
            case TileType.Bush:
                materialName = "Bush";
                break;
            case TileType.Vent:
                materialName = "Vent";
                break;
            default:
                materialName = "Wall";
                break;
        }

        if (materialName != "")
            return Resources.Load<Material>(directory + materialName);
        else return null;

    }

    void spawnObject(GameObject prefab)
    {
        Transform parent = transform.parent;

        Instantiate(prefab, transform.position, transform.rotation, parent);
    }

    void spawnPrefab()
    {
        const string directory = "Prefabs/Tiles/";

        string objName = "";

        switch (tileType)
        {
            case TileType.Wall:
                objName = "Wall_Panel";
                break;
            case TileType.Brush:
                spawnObject(Resources.Load<GameObject>("Prefabs/Tiles/Brush"));
                objName = "Wall_Panel";
                break;
            case TileType.DoorU:
                objName = "DoorUpper_Panel";
                break;    
            case TileType.DoorL:
                objName = "DoorLower_Panel";
                break;
            case TileType.Window:
                objName = "Window_Panel";
                break;
            case TileType.Wood:
                objName = "Wood_Panel";
                break;
            case TileType.Power:
                objName = "Power_Panel";
                break;
            case TileType.Mail:
                objName = "Mail_Panel";
                break;
            case TileType.Bush:
                objName = "Bush_Panel";
                break;
            case TileType.Vent:
                objName = "Vent_Panel";
                break;
        }

        if(objName != "")
        {
            GameObject objToSpawn = Resources.Load<GameObject>(directory + objName);
            spawnObject(objToSpawn);
        }
    }

    void OnDrawGizmos()
    {
        if (tileType != prevTileType)
        {
            prevTileType = tileType;

            //Fetches the renderer
            if (tileRenderer == null) tileRenderer = GetComponentInChildren<MeshRenderer>();

            tileRenderer.material = GetMaterial();
        }
    }
}












        /*Color tileColor = Color.white;

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
            case TileType.Window:
                tileColor = Color.cyan;
                break;
            case TileType.Wood:
                tileColor = Color.yellow;
                break;
            case TileType.Power:
                tileColor = Color.grey;
                break;
            case TileType.Vent:
                tileColor = Color.black;
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
        }*/