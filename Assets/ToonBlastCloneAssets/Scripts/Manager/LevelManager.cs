using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    [System.Serializable]
    public class CubePrefabList
    {
        public GameObject NormalCubePref;
        public GameObject VerticalBombCubePref;
        public GameObject HorizontalBombCubePref;
        public GameObject SqureBombCubePref;
        public GameObject DiscoCubePref;
    }

    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public Vector2Int GridSize {  get => gridSize; }
        public int Row {  get => gridSize.x; }
        public int Column { get => gridSize.y; }

        [Header("Grid Setting")]
        [SerializeField] private Vector2Int gridSize;
        [Header("Panel Setting")]
        [SerializeField] private GameObject panelPrefab;
        [SerializeField] private bool isOverlap;
        [SerializeField] private Vector2 overlapOffset;
        [Header("Cube Setting")]
        [SerializeField] private CubePrefabList cubePrefabList;
        [SerializeField] [Range(0,4)] private int maxCubeColors;
        [SerializeField] private Color[] colors;

        private GameObject[,] gridArray;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start() 
        {
            SpawnGrid();
        }

        public Color GetColor(int cubeColor)
        {
            return colors[cubeColor];
        }

        private void SpawnGrid()
        {
            Debug.Log($"Row: {gridSize.x} | Column: {gridSize.y}");
            gridArray = new GameObject[gridSize.x, gridSize.y];

            GameObject parent = new GameObject("PanelParent");

            SpriteRenderer sr = panelPrefab.GetComponent<SpriteRenderer>();
            Vector3 size = sr.bounds.size;
            Vector2 position = Vector2.zero;
            Vector2 overlap = overlapOffset / sr.sprite.pixelsPerUnit;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    position.x = x * size.x;
                    position.y = y * size.y;

                    if (isOverlap)
                    {
                        position.x += overlap.x * x;
                        position.y += overlap.y * y;
                    }
                    
                    GameObject go = Instantiate(panelPrefab, position, Quaternion.identity, parent.transform);
                    go.name = $"Panel({x},{y})";
                    go.GetComponent<CubePanel>().Init(new CubeIndex(x, y));
                    gridArray[x, y] = go;
                }
            }

            Vector2 center = new Vector2(-gridSize.x / 2f + size.x / 2, -gridSize.y / 2f + size.y / 2);

            if (isOverlap)
            {
                center.x += overlap.x / 2;
                center.y += overlap.y / 2;
            }

            parent.transform.position = center;
            CameraManager.Instance.ZoomCameraFollowGridSize(gridSize);
            SpawnCube();
        }

        private void SpawnCube()
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    //TODO: Spawn cube object
                    NormalCube cube = Instantiate(cubePrefabList.NormalCubePref, gridArray[x, y].transform).GetComponent<NormalCube>();
                    cube.Init(Random.Range(1, maxCubeColors));
                }
            }
        }

        #region Cube Action
        public void ClickNormalCube(CubeIndex index, int color)
        {

        }

        public void ClickVerticalBombCube(CubeIndex index)
        {

        }

        public void ClickHorizontalBombCube(CubeIndex index)
        {

        }

        public void ClickSqureBombCube(CubeIndex index, int xRange = 1, int yRange = 1)
        {

        }

        public void ClickDiscoCubeCube(int color)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    
                }
            }
        }
        #endregion

        private void CheckCollideCube(int x, int y, int color)
        {

        }
    }
}
