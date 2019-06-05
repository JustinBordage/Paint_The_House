using UnityEngine;
using UnityEngine.UI;

public class WallManager : MonoBehaviour
{
    public Text testTextBox;

    public bool debugTrigger = false;
    [SerializeField] private Transform roof;
    private WallLevel[] wallLevels;
    int currentLevel = 0;
    bool rotating = false;
    float newEulerAngle;
    
    //Singleton Pattern (Set up by the loading Manager)
    public static WallManager mInstance = null;

    public void Initialize()
    {
        if (mInstance == null) mInstance = this;

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

    void Update()
    {
        if(debugTrigger)
        {
            //Disables the previous level
            wallLevels[currentLevel].isLvlPlayable(false);

            //Increases the level index
            currentLevel++;

            if (currentLevel < wallLevels.Length)
            {
                //Enables the next level
                wallLevels[currentLevel].isLvlPlayable(true);

                wallLevels[currentLevel].gameObject.SetActive(true);
                newEulerAngle = roof.rotation.eulerAngles.y + 90f;
                rotating = true;
            }

            debugTrigger = false;
        }


        rotateLevel();
    }

    public void checkWall(PaintBrush brush)
    {
        //Checks if the wall level is complete (Bug here Not sure what it was)
        if(wallLevels[currentLevel].verifyWalls())
        {
            currentLevel++;
            Destroy(brush.gameObject);

            if (currentLevel < wallLevels.Length)
            {
                wallLevels[currentLevel].gameObject.SetActive(true);
                newEulerAngle = roof.rotation.eulerAngles.y + 90f;
                rotating = true;
                testTextBox.text = "You Won Level " + currentLevel + "!";
            }
            else
                testTextBox.text = "You Painted the House!";
        }
    }

    public void rotateLevel()
    {
        if(rotating)
        {
            Vector3 currRot = roof.rotation.eulerAngles;

            if (Mathf.Abs(newEulerAngle - currRot.y) > 0.2f)
                currRot.y = Mathf.Lerp(currRot.y, newEulerAngle, 10f * Time.deltaTime);
            else
            {
                currRot.y = newEulerAngle;
                rotating = false;
                wallLevels[currentLevel - 1].gameObject.SetActive(false);

                //Enables the next level
                wallLevels[currentLevel].isLvlPlayable(true);
            }

            roof.rotation = Quaternion.Euler(currRot);
        }
    }

    //Need to manually reset any static variables
    //when their associated gameObjects get destroyed
    void OnDestroy()
    {
        mInstance = null;
    }
}
