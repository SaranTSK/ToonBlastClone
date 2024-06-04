using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public class DiscoCube : AbstractCube
    {
        [SerializeField] private SpriteRenderer imageRenderer;

        public override void Init(int cubeColor)
        {
            this.cubeColor = cubeColor;
            this.isNormalCube = false;

            imageRenderer.color = LevelManager.Instance.GetColor(cubeColor);
        }

        public override void Click(CubeIndex index)
        {
            LevelManager.Instance.ClickDiscoCubeCube(index, cubeColor);
        }

        public override void Remove()
        {
            EffectManager.Instance.SpawnEffect(Effect.ExplodeVFX, transform.position, transform.rotation);
            ObjectPoolManager.Instance.ReturnToPool(PoolTag.DiscoCube, gameObject);
        }
    }
}
