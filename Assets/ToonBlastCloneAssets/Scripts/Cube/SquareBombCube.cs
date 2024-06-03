using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public class SquareBombCube : AbstractCube
    {
        [SerializeField] private int range;
        public override void Init(int cubeColor)
        {
            this.cubeColor = cubeColor;
            this.isNormalCube = false;
        }

        public override void Click(CubeIndex index)
        {
            LevelManager.Instance.ClickSqureBombCube(index, range);
        }

        public override void Remove()
        {
            ObjectPoolManager.Instance.ReturnToPool(PoolTag.SqureBombCube, gameObject);
        }
    }
}
