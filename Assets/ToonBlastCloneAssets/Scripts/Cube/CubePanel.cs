using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public struct CubeIndex
    {
        public int x;
        public int y;

        public CubeIndex(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class CubePanel : MonoBehaviour
    {
        private CubeIndex index;

        public void Init(CubeIndex index)
        {
            this.index = index;
        }

        public void Click()
        {
            Debug.Log($"Click on: {name}");
            GetCube().Click(index);
        }

        public AbstractCube GetCube()
        {
            return GetComponentInChildren<AbstractCube>();
        }
    }
}
