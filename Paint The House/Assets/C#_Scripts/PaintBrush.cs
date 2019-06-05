using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public float brushSpeed;
    public bool isMoving { get; private set; } = false;
    private Vector3 dest;
    private float distTraveled;
    private float prevDistance;
    public int deadzone = 5;
    public Color paintColor { get; private set; }


    public Vector2 testValues;
    public bool deadzoneTestTrigger = false;

    void Awake()
    {
        paintColor = GetComponentInChildren<MeshRenderer>().material.color + new Color(0.3f, 0.3f, 0.3f, 1f);
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            translateBrush();
        }
        else
        {
            if (Input.touchCount > 0)
                getBrushDestination(Input.GetTouch(0).deltaPosition);
        }

        if (deadzoneTestTrigger)
        {
            getBrushDestination(testValues);
            deadzoneTestTrigger = false;
        }
    }

    private void getBrushDestination(Vector2 dir)
    {
        //Calculates the absolute value of the
        Vector2 absDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

        //Exits if no movement will occur or if not above the deadzone
        if ((absDir.x < deadzone && absDir.y < deadzone)
            || absDir.x == absDir.y)
            return;

        //Checks for the larger axis, if
        //both the same(dir == Vector2.Zero)
        dir.x *= (absDir.x > absDir.y).GetHashCode();
        dir.y *= (absDir.x < absDir.y).GetHashCode();
        ///"GetHashCode" gets bool value (F == 0, T == 1)

        //Normalizes the vector
        dir.Normalize();

        //Checks for obstacles and map edges
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 10f, 1 << 11))
        {
            Vector2 currPos = transform.position;
            Vector2 obstaclePos = hit.collider.transform.position;

            Vector2 translateBy = (obstaclePos - currPos) - dir;
            translateBy.x *= Mathf.Abs(dir.x);
            translateBy.y *= Mathf.Abs(dir.y);

            distTraveled = 0f;
            dest = transform.position + new Vector3(translateBy.x, translateBy.y);
            prevDistance = VectorExt.vertDist(dest, transform.position);
            isMoving = true;
        }
    }

    private void translateBrush()
    {
        //Moves the brush towards the destination
        transform.position = Vector3.Lerp(transform.position, dest, Time.fixedDeltaTime * brushSpeed);

        //Calculates the distance to the objective
        float distToDest = VectorExt.vertDist(dest, transform.position);
        distTraveled += Mathf.Abs(prevDistance - distToDest);
        prevDistance = distToDest;

        //Checks if the brush has reached it's destination
        if (distToDest < 0.05f)
        {
            isMoving = false;
            distTraveled += 0.05f;
            transform.position = dest;
            WallManager.mInstance.checkWall(this);
        }

        //Sometimes the brush moves too fast
        //this will paint those missed walls
        paintTheWall();
    }

    //Paints the tiles between the beginning and the end
    private void paintTheWall()
    {
        RaycastHit[] hitList;
        Vector3 dir = transform.position - dest;

        hitList = Physics.RaycastAll(transform.position, dir.normalized, distTraveled, 1 << 10);

        for(int hit = 0; hit < hitList.Length; hit++)
        {
            hitList[hit].collider.GetComponent<WallTile>().paintWall(this);
        }
    }

    //Paints the walls
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            other.GetComponent<WallTile>().paintWall(this);
        }
    }
}













///Old Testing Input

/*if (Input.GetMouseButtonDown(0))
{
    Vector3 wallTilePos = Hitscan.screenPos(Input.mousePosition);

    if (wallTilePos == Vector3.zero)
        return;

    Vector2 dir = wallTilePos - transform.position;

    getBrushDestination(dir);
}*/
