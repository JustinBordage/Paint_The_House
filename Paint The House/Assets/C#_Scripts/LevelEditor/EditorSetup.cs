using UnityEngine;
using UnityEngine.UI;

namespace LvlEditor
{
    public enum WallDir { Front, Right, Rear, Left }

    public class EditorSetup : MonoBehaviour
    {
        GameObject tilePrefab = null;
        public Vector2 tileSize = new Vector2(75f, 75f);

        [SerializeField] private Slider heightSlider;
        [SerializeField] private Slider FBwidthSlider;
        [SerializeField] private Slider LRwidthSlider;

        void Awake()
        {
            tilePrefab = Resources.Load<GameObject>("EditorAssets/x_y_Tile");
        }

        public void SetupLevel()
        {
            //Retrieves the level dimensions
            int widthFB = EditorManager.widthFB = (int)FBwidthSlider.value;
            int widthLR = EditorManager.widthLR = (int)LRwidthSlider.value;
            int height = EditorManager.height = (int)heightSlider.value;

            //Fetches a list of the walls
            Transform[] wallList = EditorManager.Instance.getWallList();

            //Creates the walls
            spawnWall(widthFB, height, wallList[(int)WallDir.Front]);
            spawnWall(widthLR, height, wallList[(int)WallDir.Right]);
            spawnWall(widthFB, height, wallList[(int)WallDir.Rear]);
            spawnWall(widthLR, height, wallList[(int)WallDir.Left]);

            //Disables the other walls
            for (int i = 1; i < 4; i++)
                wallList[i].gameObject.SetActive(false);

            //Disables the level setup menu
            gameObject.SetActive(false);

            //Enables the editor tools
            EditorManager.Instance.gameObject.SetActive(true);
        }

        void spawnWall(int width, int height, Transform parent)
        {
            Vector2 centerPos = new Vector2((tileSize.x * width - tileSize.x) * 0.5f, (tileSize.y * height - tileSize.y) * 0.5f);
            Vector3 editorTilePos = Vector3.zero;
            GameObject editorTile = null;

            int x, y;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    editorTilePos = new Vector3(-centerPos.x + x * tileSize.x, -centerPos.y + y * tileSize.y, 0f);

                    editorTile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity, parent);

                    editorTile.name = x.ToString() + "_" + y.ToString() + "_Tile";
                    editorTile.transform.localPosition = editorTilePos;
                }
            }
        }
    }
}
