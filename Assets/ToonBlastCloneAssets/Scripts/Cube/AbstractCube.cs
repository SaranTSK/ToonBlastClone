using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public abstract class AbstractCube : MonoBehaviour
    {
        public int CubeColor { get => cubeColor; }
        protected int cubeColor;
        public bool IsNormalCube { get => isNormalCube; }
        protected bool isNormalCube;

        public abstract void Init(int cubeColor);
        public abstract void Click(CubeIndex index);
        public abstract void Remove();

        public virtual void Drop(Vector2 position, float time)
        {
            transform.DOMove(position, time);
        }
    }
}
