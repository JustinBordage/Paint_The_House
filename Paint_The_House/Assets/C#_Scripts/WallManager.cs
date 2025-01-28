using UnityEngine;
using UnityEngine.UI;

public class WallManager : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject WinMenu;
    [SerializeField] private Transform house;
    private WallLevel[] wallLevels;
    int currentLevel = 0;
    bool rotating = false;
    float newEulerAngle;

    //Singleton Pattern
    public static WallManager mInstance = null;

    public void Initialize()
    {
        //Initializes the Singleton pattern
        if (mInstance == null) mInstance = this;

        //Fetches the Level Walls
        wallLevels = GetComponentsInChildren<WallLevel>();

        //Disables all the other levels
        for (int wallLevel = 1; wallLevel < wallLevels.Length; wallLevel++)
        {
            wallLevels[wallLevel].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        Initialize();
    }

    void FixedUpdate()
    {
        RotateLevel();
    }

    public void CheckWall(PaintBrush brush)
    {
        //Checks if the wall level is complete (Bug here Not sure what it was)
        if(wallLevels[currentLevel].VerifyWalls())
        {
            //Increases the level
            //and destroys the brush
            currentLevel++;
            Destroy(brush.gameObject);

            //If there's another level
            if (currentLevel < wallLevels.Length)
            {
                //Swaps the level through rotating the walls
                wallLevels[currentLevel].gameObject.SetActive(true);
                newEulerAngle = house.rotation.eulerAngles.y + 90f;
                rotating = true;
            }
            else
                //If it was the last level display the win menu
                WinMenu.SetActive(true);
        }
    }

    public void RotateLevel()
    {
        if(rotating)
        {
            //Gets the current rotation
            Vector3 currRot = house.rotation.eulerAngles;

            //Rotates the wall if the final angle hasn't been reached
            if (Mathf.Abs(newEulerAngle - currRot.y) > 0.2f)
                currRot.y = Mathf.Lerp(currRot.y, newEulerAngle, 10f * Time.deltaTime);
            else
            {
                //Stops rotating the wall
                rotating = false;
                currRot.y = newEulerAngle;
                wallLevels[currentLevel - 1].gameObject.SetActive(false);

                //Enables the next level playability (allows player input)
                wallLevels[currentLevel].IsLvlPlayable(true);
            }

            //Applies the new rotation
            house.rotation = Quaternion.Euler(currRot);
        }
    }

    //Need to manually reset any static variables
    //when their associated gameObjects get destroyed
    void OnDestroy()
    {
        mInstance = null;
    }
}
