using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    GameObject wallManager;
    GameObject[] wallLevels;

    public static LoadingManager mInstance = null;

    // Start is called before the first frame update
    void Awake()
    {
        LoadingSequence();

        //Destroy Self After 1 second
        Destroy(this, 1f);
    }

    void LoadingSequence()
    {
        CreateManagers();
        //Perform Initialization here
    }

    //This creates the gameObjects in which the managers
    //will be attached to without initializing the script
    void CreateManagers()
    {
        wallManager = Instantiate(new GameObject("WallManager"), transform.position, Quaternion.identity);

        wallLevels = new GameObject[4];
        for (int level = 0; level < 4; level++)
        {
            wallLevels[level] = Instantiate(new GameObject("Level_" + level.ToString()), transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)), wallManager.transform);
        }
    }

    void GenerateLevels()
    {

    }

    void InitializeScripts()
    {

    }
    


}
