using UnityEngine;

public class WallTile : MonoBehaviour
{
    public static GameObject obstacle = null;
    public bool isObstacle = false;
    private static Material paintedMat = null;
    public bool isPainted { get; private set; } = false;

    void Awake()
    {
        if (obstacle == null) obstacle = Resources.Load<GameObject>("Prefabs/Obstacle_Panel");

        if (isObstacle)
        {
            Instantiate(obstacle, transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject);
        }


        if(paintedMat == null) paintedMat = Resources.Load<Material>("Materials/Painted_Panel");
    }

    public void paintWall()
    {
        if(!isPainted)
        {
            MeshRenderer wallMesh = GetComponent<MeshRenderer>();

            wallMesh.material = paintedMat;

            isPainted = true;
        }
    }
}
