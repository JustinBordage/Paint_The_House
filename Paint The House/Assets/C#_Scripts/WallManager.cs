using UnityEngine;

public class WallManager : MonoBehaviour
{
    WallTile[] wallList;

    //Singleton Pattern
    public static WallManager mInstance = null;

    void Start()
    {
        if (mInstance == null) mInstance = this;

        wallList = GetComponentsInChildren<WallTile>();
    }

    public void verifyWalls()
    {

    }

    //Need to manually reset any static variables
    //when their associated gameObjects get destroyed
    void OnDestroy()
    {
        mInstance = null;
    }
}
