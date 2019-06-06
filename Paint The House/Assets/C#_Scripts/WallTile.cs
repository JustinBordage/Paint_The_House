using UnityEngine;

public class WallTile : MonoBehaviour
{
    public bool isPainted { get; private set; } = false;

    public void paintWall(PaintBrush brush)
    {
        if(!isPainted)
        {
            //Checks that it's in the brush is the same level
            if(brush.transform.parent == transform.parent)
            {
                MeshRenderer wallMesh = GetComponentInChildren<MeshRenderer>();
                Material wallMat = wallMesh.material;
                wallMat.color = brush.paintColor;
                wallMesh.material = wallMat;

                isPainted = true;
            }
        }
    }
}
