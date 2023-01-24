using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy.Loot
{
    public class LootPiece : MonoBehaviour
    {
        [SerializeField] private GameObject _skull;
        [SerializeField] private GameObject _pickupFxPrefab;
        [SerializeField] private TMP_Text _lootText;
        [SerializeField] private GameObject _pickupPopup;

        private LootItem _loot;
        private bool _picked;
        private WorldData _worldData;

        private void OnTriggerEnter(Collider other) =>
            Pickup();

        public void Construct(WorldData worldData) =>
            _worldData = worldData;

        public void Initialize(LootItem loot) =>
            _loot = loot;

        private void Pickup()
        {
            if (_picked) return;

            _picked = true;

            _worldData.LootData.Collect(_loot);

            HideSkull();
            PlayPickupFx();
            ShowText();
            Destroy(gameObject, 1.5f);
        }

        private void HideSkull() =>
            _skull.SetActive(false);

        private void PlayPickupFx() =>
            Instantiate(_pickupFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            _lootText.text = _loot.Value.ToString();
            _pickupPopup.SetActive(true);
        }
    }
}