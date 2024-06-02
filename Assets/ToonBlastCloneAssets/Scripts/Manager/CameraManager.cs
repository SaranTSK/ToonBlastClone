using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        public Camera MainCamera { get => mainCamera; }
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void ZoomCameraFollowGridSize(Vector2Int girdSize)
        {
            mainCamera.orthographicSize = Mathf.Max(girdSize.x, girdSize.y / 2f);
        }
    }
}
