using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public float brushSpeed;
    bool isMoving = false;
    Vector3 dest;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            getBrushDestination();

        translateBrush();
    }

    private void getBrushDestination()
    {
        if (isMoving)
            return;

        Debug.Log("Click 0");
        Vector3 wallTilePos = Hitscan.screenPos(Input.mousePosition);

        if (wallTilePos != Vector3.zero)
        {
            Debug.Log("Click 1");
            Vector2 dir = wallTilePos - transform.position;

            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
                dir.y = 0f;
            else
                dir.x = 0f;

            dir = dir.normalized;

            RaycastHit hit;

            //Checks for obstacles and map edges
            if (Physics.Raycast(transform.position, dir, out hit, 10f, (1 << 11) + (1 << 12)))
            {
                Debug.Log("Click 2");

                Vector2 currPos = transform.position;
                Vector2 obstaclePos = hit.collider.transform.position;

                Vector2 translateBy = (obstaclePos - currPos) - dir;
                translateBy.x *= Mathf.Abs(dir.x);
                translateBy.y *= Mathf.Abs(dir.y);

                dest = transform.position + new Vector3(translateBy.x, translateBy.y);
                isMoving = true;
            }
        }
    }

    private void translateBrush()
    {
        if(isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, dest, Time.deltaTime * brushSpeed);

            if(VectorExt.vertDist(dest, transform.position) < 0.05f)
            {
                transform.position = dest;
                isMoving = false;
            }
        }
    }
}
