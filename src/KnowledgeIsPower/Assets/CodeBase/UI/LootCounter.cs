using CodeBase.Data.Loot;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class LootCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counter;
        private LootData _lootData;

        private void Start() =>
            UpdateCounter();

        public void Construct(LootData lootData)
        {
            _lootData = lootData;
            _lootData.Changed += UpdateCounter;
        }

        private void UpdateCounter() =>
            _counter.text = _lootData.Collected.ToString();
    }
}