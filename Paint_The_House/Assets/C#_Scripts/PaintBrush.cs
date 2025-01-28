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

    private Vector2 lastDir = Vector2.zero;
    
    private static AudioPlayer audioPlayer;

    void Awake()
    {
        //Fetches the paintColor
        paintColor = GetComponentInChildren<MeshRenderer>().material.color + new Color(0.3f, 0.3f, 0.3f, 1f);

        //Fetches the audioPlayer
        if(audioPlayer == null)
            audioPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioPlayer>();
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
        dir.x *= (absDir.x > absDir.y).GetHashCode();
        dir.y *= (absDir.x < absDir.y).GetHashCode();
        ///"GetHashCode" gets bool value (F == 0, T == 1)

        //Normalizes the vector
        dir.Normalize();

        //Checks for obstacles and map edges
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, dir, out hit, 10f, 1 << 11))
        {
            return false;
        }
        
        //Fetches the current and obstacle positions
        Vector2 currPos = transform.position;
        Vector2 obstaclePos = hit.collider.transform.position;

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
            WallManager.mInstance.CheckWall(this);
            lastDir = Vector2.zero;
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
