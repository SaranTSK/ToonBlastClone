using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    [System.Serializable]
    public class NormalCubeIcon
    {
        public Sprite FireworkIcon;
        public Sprite BombIcon;
        public Sprite CrossIcon;
        public Sprite DiscoIcon;
    }
    public class NormalCube : AbstractCube
    {
        [SerializeField] private NormalCubeIcon iconSprites;
        [SerializeField] private SpriteRenderer imageRenderer;
        [SerializeField] private SpriteRenderer iconRenderer;

        public override void Init(int cubeColor)
        {
            this.cubeColor = cubeColor;
            this.isNormalCube = true;

            imageRenderer.color = LevelManager.Instance.GetColor(cubeColor);
            iconRenderer.sprite = null;
        }

        public void SetCubeIcon(int amount)
        {
            if (amount >= CollideCondition.DiscoCount)
            {
                iconRenderer.sprite = iconSprites.DiscoIcon;
            }
            else if (amount >= CollideCondition.CrossCount)
            {
                iconRenderer.sprite = iconSprites.CrossIcon;
            }
            else if (amount >= CollideCondition.BombCount)
            {
                iconRenderer.sprite = iconSprites.BombIcon;
            }
            else if (amount >= CollideCondition.FireWorkCount)
            {
                iconRenderer.sprite = iconSprites.FireworkIcon;
            }
            else
            {
                iconRenderer.sprite = null;
            }

            iconRenderer.color = LevelManager.Instance.GetColor(cubeColor) - new Color(0.25f, 0.25f, 0.25f, 0f);
        }

        public override void Click(CubeIndex index)
        {
            LevelManager.Instance.ClickNormalCube(index, cubeColor);
        }

        public override void Remove()
        {
            EffectManager.Instance.SpawnEffect(Effect.ExplodeVFX, transform.position, transform.rotation);
            ObjectPoolManager.Instance.ReturnToPool(PoolTag.NormalCube, gameObject);
        }
    }
}
