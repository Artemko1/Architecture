using CodeBase.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService) =>
            _saveLoadService = saveLoadService;

        private void OnDrawGizmos()
        {
            if (boxCollider == null) return;

            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<CharacterController>()) return;
            _saveLoadService.SaveProgress();
            gameObject.SetActive(false);
        }
    }
}