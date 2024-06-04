using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public static class PoolTag
    {
        public static string NormalCube = "NormalCube";
        public static string VerticalBombCube = "VerticalBombCube";
        public static string HorizontalBombCube = "HorizontalBombCube";
        public static string SqureBombCube = "SqureBombCube";
        public static string CrossBombCube = "CrossBombCube";
        public static string DiscoCube = "DiscoCube";
    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance;

        [SerializeField] private List<Pool> pools;

        private Dictionary<string, Queue<GameObject>> poolDict;

        public bool IsInit {  get => isInit; }
        private bool isInit;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            poolDict = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject go = Instantiate(pool.prefab, transform);
                    go.SetActive(false);
                    objectPool.Enqueue(go);
                }

                poolDict.Add(pool.tag, objectPool);
            }

            isInit = true;
        }

        private Pool GetPool(string tag) 
        {
            return pools.Find(s => s.tag == tag);
        }

        public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation, Transform parent = null)
        {
            if(!poolDict.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool [{tag}] doesn't exist!");
                return null;
            }

            GameObject spawnObject = (poolDict[tag].Count > 0) ? poolDict[tag].Dequeue() : Instantiate(GetPool(tag).prefab);

            spawnObject.SetActive(true);
            spawnObject.transform.position = position;
            spawnObject.transform.rotation = rotation;
            spawnObject.transform.parent = parent;

            return spawnObject;
        }

        public void ReturnToPool(string tag, GameObject spawnObject)
        {
            if (!poolDict.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool [{tag}] doesn't exist!");
                return;
            }

            poolDict[tag].Enqueue(spawnObject);

            spawnObject.SetActive(false);
            spawnObject.transform.position = transform.position;
            spawnObject.transform.rotation = Quaternion.identity;
            spawnObject.transform.parent = transform;
        }
    }
}
