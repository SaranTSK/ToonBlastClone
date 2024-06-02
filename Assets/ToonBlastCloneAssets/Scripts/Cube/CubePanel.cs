using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public struct CubeIndex
    {
        public int x;
        public int y;

        public CubeIndex(int x, int y)
        {
            this.x = 0;
            this.y = 0;
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
            AbstractCube cube = GetComponentInChildren<AbstractCube>();
            cube.Click(index);
        }
    }
}
