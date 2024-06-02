using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public static class CubeType
    {
        public static int NormalCube = 1;
        public static int VerticalBombCube = 2;
        public static int HorizontalBombCube = 3;
        public static int SqureBombCube = 4;
        public static int DiscoCube = 5;
    }

    public static class CubeColor
    {
        public static int None = 0;
        public static int Red = 1;
        public static int Green = 2;
        public static int Blue = 3;
        public static int Yellow = 4;
    }

    public abstract class AbstractCube : MonoBehaviour
    {
        public abstract void Init(int cubeColor);
        public abstract void Click(CubeIndex index);
    }
}
