using System;
using System.Linq;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    [SerializeField] private float maxScanDistance = 10f;
    
    public float brushSpeed;
    public bool isMoving { get; private set; } = false;
    private Vector3 dest;
    private float distTraveled;
    private float prevDistance;
    public int deadzone = 5;
    public Color paintColor { get; private set; }

    private Vector2 lastDir = Vector2.zero;
    
    private static AudioPlayer audioPlayer;
    private LayerMask obstacleLayer;
    private LayerMask wallLayer;
    
    
    
    // Debugging
    private Vector3 prevRayCastOrigin = Vector3.zero;
    private Vector3 prevRayCastDest = Vector3.zero;

    void Awake()
    {
        //Fetches the paintColor
        paintColor = GetComponentInChildren<MeshRenderer>().material.color + new Color(0.3f, 0.3f, 0.3f, 1f);

        //Fetches the audioPlayer
        if(audioPlayer == null)
            audioPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioPlayer>();
        
        obstacleLayer = LayerMask.GetMask("Obstacle");
        wallLayer = LayerMask.GetMask("Wall");
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            //Performs the object translation
            TranslateBrush();
        }
        else
        {
            //Translates the brush if the swipe is long enough
            if (Input.touchCount > 0)
            {
                GetBrushDestination(Input.GetTouch(0).deltaPosition);
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) ||
                    Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow))
                {
                    var xAxis = Input.GetAxis("Horizontal");
                    var yAxis = Input.GetAxis("Vertical");
                    var dir = new Vector2(xAxis, yAxis).normalized * deadzone;
                    
                    if (lastDir.x != dir.x || lastDir.y != dir.y)
                    {
                        var didMove = GetBrushDestination(dir);

                        if (didMove)
                        {
                            lastDir = dir;
                        }
                        else
                        {
                            lastDir = Vector2.zero;
                        }
                    }
                }
            }
        }
    }

    private bool GetBrushDestination(Vector2 dir)
    {
        //Calculates the absolute value of the
        Vector2 absDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
        
        //Exits if no movement will occur or if not above the deadzone
        if ((absDir.x < deadzone && absDir.y < deadzone) || absDir.x == absDir.y)
        {
            return false;
        }

        //Checks for the larger axis, if
        //both the same(dir == Vector2.Zero)
        if (absDir.x > absDir.y)
        {
            dir.y = 0;
        }
        if (absDir.x < absDir.y)
        {
            dir.x = 0;
        }

        Debug.Log("[BEFORE] Direction: " + dir);
        
        //Normalizes the vector
        dir.Normalize();
        
        //Checks for obstacles and map edges (Each tile has an invisible BoxCollider behind it, this is what the hit scan triggers on)
        bool hitWall = false;
        if (!Physics.Raycast(transform.position, dir, out var hit, maxScanDistance, obstacleLayer))
        {
            // if (!FindFurthestWall(transform.position, dir, out hit, maxScanDistance, wallLayer))
            // {
            //     Debug.Log("Did not pick anything up");
            //     return false;
            // }
            // else
            // {
            //     hitWall = true;
            // }
            return false;
        }

        prevRayCastOrigin = transform.position;
        prevRayCastDest = hit.point;

        Debug.Log("[AFTER] Direction: " + dir);
        
        //Fetches the current and obstacle positions
        Vector2 currPos = transform.position;

        var hitObject = hit.collider.transform;
        // Vector3 dimensions = hitObject.lossyScale; // Perhaps use the brush dimensions instead
        // Debug.Log("[BEFORE] Size: " + dimensions);
        // dimensions.Scale(hitWall ? dir : Vector2.zero);
        // Debug.Log("[AFTER] Size: " + dimensions);
        Vector2 obstaclePos = hitObject.position;// + dimensions;

        //Calculates the distance to travel
        Vector3 translateBy = (obstaclePos - currPos) - dir;
        translateBy.x *= Mathf.Abs(dir.x);
        translateBy.y *= Mathf.Abs(dir.y);

        //Calculates the distance to the destination
        dest = transform.position + translateBy;
        float distance = VectorExt.VertDist(dest, transform.position);
        
        if(distance <= 0f)
        {
            return false;
        }
        
        //Triggers the Translation
        isMoving = true;
        audioPlayer.PlaySFX("BrushSwoosh");
        prevDistance = distance;
        distTraveled = 0f;
        
        return true;
    }

    private bool FindFurthestWall(
        Vector3 origin,
        Vector3 direction,
        out RaycastHit hitInfo,
        float maxDistance,
        int layerMask
    ) {
        var allHits = Physics.RaycastAll(origin, direction, maxDistance, layerMask);
        if (allHits.Length == 0)
        {
            hitInfo = new RaycastHit();
            return false;
        }

        var furthestHit = new RaycastHit();
        hitInfo = allHits.Aggregate(furthestHit, (furthest, hit) => furthest.distance < hit.distance ? hit : furthest);
        return true;
    }

    private void TranslateBrush()
    {
        //Moves the brush towards the destination
        transform.position = Vector3.Lerp(transform.position, dest, Time.fixedDeltaTime * brushSpeed);

        //Calculates the distance to the objective
        float distToDest = VectorExt.VertDist(dest, transform.position);
        distTraveled += Mathf.Abs(prevDistance - distToDest);
        prevDistance = distToDest;

        //Checks if the brush has reached it's destination
        if (distToDest < 0.05f)
        {
            //Stops the translation
            isMoving = false;
            distTraveled += 0.05f;
            transform.position = dest;
            lastDir = Vector2.zero;
            
            // The Wall Manager is not
            // required on the Level Editor
            if (WallManager.mInstance)
            {
                WallManager.mInstance.CheckWall(this);
            }
        }

        //Sometimes the brush moves too fast
        //this will paint those missed walls
        PaintTheWall();
    }

    //Catches any tiles that were missed by the 
    private void PaintTheWall()
    {
        //Variables
        RaycastHit[] hitList;
        Vector3 dir = transform.position - dest;

        //Checks all the tiles in the path that it has travelled so far
        hitList = Physics.RaycastAll(transform.position, dir.normalized, distTraveled, 1 << 10);

        //Paints those tiles
        for(int hit = 0; hit < hitList.Length; hit++)
        {
            hitList[hit].collider.GetComponent<WallTile>().PaintWall(this);
        }
    }

    //Paints the walls
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            other.GetComponent<WallTile>().PaintWall(this);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(prevRayCastOrigin, prevRayCastDest, Color.red);
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
