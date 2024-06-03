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
            if (GetCube() == null)
                return;

            GetCube().Click(index);
        }

        public AbstractCube GetCube()
        {
            if(transform.childCount == 0)
                return null;

            return transform.GetChild(0).GetComponent<AbstractCube>();
        }

        public bool IsEqualIndex(CubeIndex index) 
        {
            return this.index.x == index.x && this.index.y == index.y;
        }
    }
}
