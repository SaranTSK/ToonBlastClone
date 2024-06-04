using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ToonBlast
{
    public class SettingPanel : MonoBehaviour
    {
        public static SettingPanel Instance;

        [SerializeField] private SettingAmount xAmountSetting;
        [SerializeField] private SettingAmount yAmountSetting;
        [SerializeField] private Button generateButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            InitOnClick();
        }

        public void Init(Vector2Int gridSize)
        {
            xAmountSetting.Init(gridSize.x);
            yAmountSetting.Init(gridSize.y);
        }

        private void InitOnClick()
        {
            generateButton.onClick.AddListener(OnClickGenerate);
        }

        private void OnClickGenerate()
        {
            Vector2Int newGridSize = new Vector2Int(xAmountSetting.Amount, yAmountSetting.Amount);
            LevelManager.Instance.GenerateGrid(newGridSize);
        }
    }
}
