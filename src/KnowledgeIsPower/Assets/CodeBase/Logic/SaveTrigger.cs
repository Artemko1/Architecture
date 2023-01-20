using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        private ISaveLoadService _saveLoadService;

        private void Awake() =>
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();

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