using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToonBlast
{
    public class SettingAmount : MonoBehaviour
    {
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;
        [SerializeField] private int maxSize = 100;

        public int Amount { get => amount; }
        private int amount;

        private void Awake()
        {
            InitOnClick();
        }

        public void Init(int amount)
        {
            this.amount = amount;

            SetButtonEnable(minusButton, amount > 1);
            SetButtonEnable(plusButton, amount < maxSize);
            SetAmountText(amount);
        }

        private void SetAmountText(int amount)
        {
            amountText.text = $"{amount}";
        }

        private void SetButtonEnable(Button button, bool isEnable)
        {
            button.enabled = isEnable;
        }

        #region OnClick
        private void InitOnClick()
        {
            minusButton.onClick.AddListener(OnClickMinusButton);
            plusButton.onClick.AddListener(OnClickPlusButton);
        }

        private void OnClickMinusButton()
        {
            if (amount < 1)
                return;

            amount--;
            SetButtonEnable(minusButton, amount > 1);
            SetButtonEnable(plusButton, amount < maxSize);
            SetAmountText(amount);
        }

        private void OnClickPlusButton()
        {
            if (amount >= maxSize )
                return;

            amount++;
            SetButtonEnable(minusButton, amount > 1);
            SetButtonEnable(plusButton, amount < maxSize);
            SetAmountText(amount);
        }
        #endregion


    }
}
