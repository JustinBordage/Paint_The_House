using UnityEngine;
using UnityEngine.UI;

public class WallManager : MonoBehaviour
{
    public Text testTextBox;

    [SerializeField] private Transform roof;
    private WallLevel[] wallLvlList;
    int currentLevel = 0;
    bool rotating = false;
    float newEulerAngle;
    
    //Singleton Pattern
    public static WallManager mInstance = null;

    void Start()
    {
        wallLvlList = GetComponentsInChildren<WallLevel>();

        Debug.Log("Size: " + wallLvlList.Length);

        //Disables all the other levels
        for (int wallLevel = 1; wallLevel < wallLvlList.Length; wallLevel++)
        {
            wallLvlList[wallLevel].gameObject.SetActive(false);
        }

        if (mInstance == null) mInstance = this;
    }

    void Update()
    {
        rotateLevel();
    }

    public void checkWall()
    {
        //Checks if the wall level is complete (Bug here)
        if(wallLvlList[currentLevel].verifyWalls())
        {
            currentLevel++;

            if (currentLevel < wallLvlList.Length)
            {
                wallLvlList[currentLevel].gameObject.SetActive(true);
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
                wallLvlList[currentLevel - 1].gameObject.SetActive(false);
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
