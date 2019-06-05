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
    [SerializeField] private ParticleSystem paintFX;

    void Awake()
    {
        paintFX.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            translateBrush();
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchDelta = Input.GetTouch(0).deltaPosition;

                touchDelta.x = Mathf.Round(touchDelta.x / 10);
                touchDelta.y = Mathf.Round(touchDelta.y / 10);

                getBrushDestination(touchDelta);
            }
        }
    }

    private void getBrushDestination(Vector2 dir)
    {
        //if (isMoving)
        //    return;

        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            dir.y = 0f;
        else
            dir.x = 0f;

        dir = dir.normalized;

        RaycastHit hit;

        //Checks for obstacles and map edges
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
            paintFX.Play();
        }
    }

    private void translateBrush()
    {
        //Moves the brush towards the destination
        transform.position = Vector3.Lerp(transform.position, dest, Time.deltaTime * brushSpeed);

        //Calculates the distance to the objective
        float distToDest = VectorExt.vertDist(dest, transform.position);
        distTraveled += Mathf.Abs(prevDistance - distToDest);
        prevDistance = distToDest;

        //Checks if the brush has reached it's destination
        if (distToDest < 0.05f)
        {
            paintFX.Pause();
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
            hitList[hit].collider.GetComponent<WallTile>().paintWall();
        }
    }

    //Paints the walls
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            other.GetComponent<WallTile>().paintWall();
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
