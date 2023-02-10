using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    internal class ShopWindow : WindowBase
    {
        [SerializeField] private TMP_Text _currencyText;

        protected override void Initialize()
        {
            base.Initialize();
            RefreshCurrencyText();
        }

        protected override void SubscribeUpdates()
        {
            base.SubscribeUpdates();
            Progress.PlayerState.LootData.Changed += RefreshCurrencyText;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            Progress.PlayerState.LootData.Changed -= RefreshCurrencyText;
        }

        private void RefreshCurrencyText() =>
            _currencyText.text = Progress.PlayerState.LootData.Collected.ToString();
    }
}