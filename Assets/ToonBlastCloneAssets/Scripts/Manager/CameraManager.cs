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

        private Vector2Int gridSize;
        private Vector2Int screenSize;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            screenSize = new Vector2Int(Screen.width, Screen.height);
        }

        private void Update()
        {
            UpdateScreenSizeChange();
        }

        public void ZoomCameraFollowGridSize(Vector2Int gridSize)
        {
            this.gridSize = gridSize;

            float desirSize = Mathf.Max(gridSize.x, gridSize.y / 2f) + 0.5f;
            mainCamera.orthographicSize = desirSize * screenSize.y / screenSize.x * 0.5f;
        }

        private void UpdateScreenSizeChange()
        {
            if(screenSize.x != Screen.width || screenSize.y != Screen.height)
            {
                screenSize = new Vector2Int(Screen.width, Screen.height);
                ZoomCameraFollowGridSize(gridSize);
            }
        }
    }
}
