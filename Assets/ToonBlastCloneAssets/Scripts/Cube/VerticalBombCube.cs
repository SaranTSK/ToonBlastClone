using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public class VerticalBombCube : AbstractCube
    {
        public override void Init(int cubeColor)
        {
            this.cubeColor = cubeColor;
            this.isNormalCube = false;
        }

        public override void Click(CubeIndex index)
        {
            LevelManager.Instance.ClickVerticalBombCube(index);
        }

        public override void Remove()
        {
            EffectManager.Instance.SpawnEffect(Effect.ExplodeVFX, transform.position, transform.rotation);
            ObjectPoolManager.Instance.ReturnToPool(PoolTag.VerticalBombCube, gameObject);
        }
    }
}
