using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public class HorizontalBombCube : AbstractCube
    {
        public override void Init(int cubeColor)
        {
            this.cubeColor = cubeColor;
            this.isNormalCube = false;
        }

        public override void Click(CubeIndex index)
        {
            LevelManager.Instance.ClickHorizontalBombCube(index);
        }

        public override void Remove()
        {
            ObjectPoolManager.Instance.ReturnToPool(PoolTag.HorizontalBombCube, gameObject);
        }
    }
}
