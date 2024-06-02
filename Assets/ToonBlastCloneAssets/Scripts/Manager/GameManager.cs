using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Update()
        {
            UpdateMouseInput();
        }

        private void UpdateMouseInput()
        {
            if(Input.GetMouseButtonDown(0))
            {
                CheckRaycast();
            }
        }

        private void CheckRaycast()
        {
            RaycastHit2D hit = Physics2D.Raycast(CameraManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if(hit.collider.TryGetComponent(out CubePanel cubePanel))
                {
                    cubePanel.Click();
                }
            }
        }
    }

}
