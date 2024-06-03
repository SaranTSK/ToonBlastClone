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

        private CubePanel[,] gridArray;

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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                ResetCubes();
            }
        }

        public Color GetColor(int cubeColor)
        {
            return colors[cubeColor];
        }

        private void SpawnGrid()
        {
            Debug.Log($"Row: {gridSize.x} | Column: {gridSize.y}");
            gridArray = new CubePanel[gridSize.x, gridSize.y];

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
                    
                    GameObject go = Instantiate(panelPrefab, position, Quaternion.identity, transform);
                    go.name = $"Panel({x},{y})";
                    go.GetComponent<CubePanel>().Init(new CubeIndex(x, y));
                    gridArray[x, y] = go.GetComponent<CubePanel>();
                }
            }

            Vector2 center = new Vector2(-gridSize.x / 2f + size.x / 2, -gridSize.y / 2f + size.y / 2);

            if (isOverlap)
            {
                center.x += overlap.x / 2;
                center.y += overlap.y / 2;
            }

            transform.position = center;
            CameraManager.Instance.ZoomCameraFollowGridSize(gridSize);
            SpawnCube();
        }

        public void ResetCubes()
        {
            foreach(Transform child in transform)
            {
                Destroy(child.GetComponent<CubePanel>().GetCube().gameObject);
            }

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
            cubes.Clear();
            queue.Clear();
            queue.Enqueue(index);

            while (queue.Count > 0)
            {
                CubeIndex current = queue.Dequeue();
                CheckCollideCube(current, color);
            }

            Debug.Log($"Cube[{color}] amount: {cubes.Count}");
            if(cubes.Count > 1)
            {
                foreach (CubePanel cube in cubes)
                {
                    cube.GetCube().Remove();
                }
            }
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

        private List<CubePanel> cubes = new List<CubePanel>();
        private Queue<CubeIndex> queue = new Queue<CubeIndex>();
        private void CheckCollideCube(CubeIndex index, int color)
        {
            Debug.Log($"Check Index[{index.x},{index.y}]");

            CubeIndex targetIndex = new CubeIndex();
            // Check Left Cube
            if (index.x - 1 >= 0)
            {
                if (!cubes.Contains(gridArray[index.x - 1, index.y]))
                {
                    targetIndex = new CubeIndex(index.x - 1, index.y);
                    if (IsColorMatch(targetIndex, color))
                    {
                        queue.Enqueue(targetIndex);
                    }
                    else
                    {
                        Debug.LogWarning($"Left cube [{targetIndex.x},{targetIndex.y}] doesn't match");
                    }
                }
                else
                {
                    Debug.LogWarning($"Left cube [{targetIndex.x},{targetIndex.y}] is in list");
                }
            }

            // Check Right Cube
            if (index.x + 1 < gridSize.x)
            {
                if (!cubes.Contains(gridArray[index.x + 1, index.y]))
                {
                    targetIndex = new CubeIndex(index.x + 1, index.y);
                    if (IsColorMatch(targetIndex, color))
                    {
                        queue.Enqueue(targetIndex);
                    }
                    else
                    {
                        Debug.LogWarning($"Right cube [{targetIndex.x},{targetIndex.y}] doesn't match");
                    }
                }
                else
                {
                    Debug.LogWarning($"Right cube [{targetIndex.x} , {targetIndex.y}] is in list");
                }
            }

            // Check Top Cube
            if (index.y - 1 >= 0)
            {
                if (!cubes.Contains(gridArray[index.x, index.y - 1]))
                {
                    targetIndex = new CubeIndex(index.x, index.y - 1);
                    if (IsColorMatch(targetIndex, color))
                    {
                        queue.Enqueue(targetIndex);
                    }
                    else
                    {
                        Debug.LogWarning($"Top cube [{targetIndex.x} , {targetIndex.y}] doesn't match");
                    }
                }
                else
                {
                    Debug.LogWarning($"Top cube [{targetIndex.x} , {targetIndex.y}] is in list");
                }
            }

            // Check Down Cube
            if (index.y + 1 < gridSize.y)
            {
                if (!cubes.Contains(gridArray[index.x, index.y + 1]))
                {
                    targetIndex = new CubeIndex(index.x, index.y + 1);
                    if (IsColorMatch(targetIndex, color))
                    {
                        queue.Enqueue(targetIndex);
                    }
                    else
                    {
                        Debug.LogWarning($"Down cube [{targetIndex.x} , {targetIndex.y}] doesn't match");
                    }
                }
                else
                {
                    Debug.LogWarning($"Down cube [{targetIndex.x} , {targetIndex.y}] is in list");
                }
            }

            // Add origin index
            cubes.Add(gridArray[index.x, index.y]);
        }

        private bool IsColorMatch(CubeIndex index, int color)
        {
            if(gridArray[index.x, index.y].GetCube() == null)
                return false;

            return gridArray[index.x, index.y].GetCube().CubeColor == color;
        }
    }
}
