using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public class NormalCube : AbstractCube
    {
        [SerializeField] private Sprite[] iconSprites;
        [SerializeField] private SpriteRenderer imageRenderer;
        [SerializeField] private SpriteRenderer iconRenderer;

        public override void Init(int cubeColor)
        {
            this.cubeColor = cubeColor;

            gameObject.SetActive(true);
            imageRenderer.color = LevelManager.Instance.GetColor(cubeColor);
            iconRenderer.sprite = null;
            //iconRenderer.color = LevelManager.Instance.GetColor(cubeColor) - new Color(0.25f, 0.25f, 0.25f);
        }

        public override void Click(CubeIndex index)
        {
            LevelManager.Instance.ClickNormalCube(index, cubeColor);
        }

        public override void Remove()
        {
            // TODO: Return pool
            gameObject.SetActive(false);
        }
    }
}
