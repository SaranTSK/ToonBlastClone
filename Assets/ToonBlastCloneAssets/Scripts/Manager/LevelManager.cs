using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
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
            SpawnNormalCube();
        }

        public void ResetCubes()
        {
            foreach(Transform child in transform)
            {
                AbstractCube cube = child.GetComponent<CubePanel>().GetCube();
                if(cube != null)
                {
                    cube.Remove();
                }
            }

            SpawnNormalCube();
        }

        #region Cube Spawning
        private void SpawnNormalCube()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    CubePanel panel = gridArray[x, y];
                    GameObject cube = ObjectPoolManager.Instance.SpawnFromPool(PoolTag.NormalCube, panel.transform.position, Quaternion.identity, panel.transform);
                    cube.GetComponent<NormalCube>().Init(Random.Range(1, colors.Length));
                }
            }
        }

        private void SpawnSpecialCube(string tag, CubeIndex index, int color = 0)
        {
            CubePanel panel = gridArray[index.x, index.y];
            GameObject cube = ObjectPoolManager.Instance.SpawnFromPool(tag, panel.transform.position, Quaternion.identity, panel.transform);
            
            if(tag == PoolTag.DiscoCube)
            {
                cube.GetComponent<DiscoCube>().Init(color);
            }
        }
        #endregion

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

            int amount = cubes.Count;
            if (amount > 1)
            {
                foreach (CubePanel cube in cubes)
                {
                    cube.GetCube().Remove();
                }

                CheckSpecialCubeSpawn(index, amount, color);
            }
        }

        public void ClickVerticalBombCube(CubeIndex index)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                CubePanel panel = gridArray[index.x, y];

                if (panel.GetCube() == null)
                    continue;

                if (panel.GetCube().IsNormalCube || panel.IsEqualIndex(index))
                {
                    panel.GetCube().Remove();
                }
                else
                {
                    panel.GetCube().Click(new CubeIndex(index.x, y));
                }
            }
        }

        public void ClickHorizontalBombCube(CubeIndex index)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                CubePanel panel = gridArray[x, index.y];

                if (panel.GetCube() == null)
                    continue;

                if (panel.GetCube().IsNormalCube || panel.IsEqualIndex(index))
                {
                    panel.GetCube().Remove();
                }
                else
                {
                    panel.GetCube().Click(new CubeIndex(x, index.y));
                }
            }
        }

        public void ClickSqureBombCube(CubeIndex index, int range)
        {
            int xMin = (index.x - range < 0) ? 0 : index.x - range;
            int xMax = (index.x + range > gridSize.x) ? gridSize.x : index.x + range + 1;
            int yMin = (index.y - range < 0) ? 0 : index.y - range;
            int yMax = (index.y + range > gridSize.y) ? gridSize.y : index.y + range + 1;

            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    CubePanel panel = gridArray[x, y];

                    if (panel.GetCube() == null)
                        continue;

                    if (panel.GetCube().IsNormalCube || panel.IsEqualIndex(index))
                    {
                        panel.GetCube().Remove();
                    }
                    else
                    {
                        panel.GetCube().Click(new CubeIndex(x, y));
                    }
                }
            }
        }

        public void ClickDiscoCubeCube(int color)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    if (IsColorMatch(new CubeIndex(x, y), color))
                    {
                        gridArray[x, y].GetCube().Remove();
                    }
                }
            }
        }
        #endregion

        #region Cube Checker
        private List<CubePanel> cubes = new List<CubePanel>();
        private Queue<CubeIndex> queue = new Queue<CubeIndex>();
        private void CheckCollideCube(CubeIndex index, int color)
        {
            Debug.Log($"Check Index[{index.x},{index.y}]");

            // Add origin index
            if (!cubes.Contains(gridArray[index.x, index.y]))
            {
                cubes.Add(gridArray[index.x, index.y]);
            }
            else
            {
                Debug.LogWarning($"Self cube [{index.x} , {index.y}] is in list");
            }

            CubeIndex targetIndex = new CubeIndex();
            // Check Left Cube
            if (index.x - 1 >= 0)
            {
                if (!cubes.Contains(gridArray[index.x - 1, index.y]))
                {
                    targetIndex = new CubeIndex(index.x - 1, index.y);
                    if (IsColorMatch(targetIndex, color))
                    {
                        cubes.Add(gridArray[index.x - 1, index.y]);
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
                        cubes.Add(gridArray[index.x + 1, index.y]);
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
                        cubes.Add(gridArray[index.x, index.y - 1]);
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
                        cubes.Add(gridArray[index.x, index.y + 1]);
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
        }

        private bool IsColorMatch(CubeIndex index, int color)
        {
            if(gridArray[index.x, index.y].GetCube() == null)
                return false;

            return gridArray[index.x, index.y].GetCube().CubeColor == color;
        }

        private void CheckSpecialCubeSpawn(CubeIndex index, int amount, int color)
        {
            if(amount >= 10)
            {
                SpawnSpecialCube(PoolTag.DiscoCube, index, color);
            }
            else if(amount >= 6)
            {
                SpawnSpecialCube(PoolTag.SqureBombCube, index);
            }
            else if (amount >= 4)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                    SpawnSpecialCube(PoolTag.VerticalBombCube, index);
                else
                    SpawnSpecialCube(PoolTag.HorizontalBombCube, index);
            }
        }
        #endregion
    }
}
