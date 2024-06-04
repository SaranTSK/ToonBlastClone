using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast
{
    public enum GameplayState
    {
        Idle = 0,
        Check = 1,
        Spawn = 2,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameplayState GameplayState { get => gameplayState; }
        private GameplayState gameplayState;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            AudioManager.Instance.Play(Audio.BGM_Gameplay);
            ChangeState(GameplayState.Idle);
            DOTween.SetTweensCapacity(2000, 100);
        }

        private void Update()
        {
            UpdateInput();
        }

        public void ChangeState(GameplayState gameplayState)
        {
            this.gameplayState = gameplayState;
        }

        private void UpdateInput()
        {
            if(Input.GetMouseButtonDown(0))
            {
                CheckRaycast();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                LevelManager.Instance.ResetCubes();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void CheckRaycast()
        {
            RaycastHit2D hit = Physics2D.Raycast(CameraManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if(GameplayState == GameplayState.Idle)
                {
                    if (hit.collider.TryGetComponent(out CubePanel cubePanel))
                    {
                        cubePanel.Click();
                    }
                }
            }
        }
    }

}
