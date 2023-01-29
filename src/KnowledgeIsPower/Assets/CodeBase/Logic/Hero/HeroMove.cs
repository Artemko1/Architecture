using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Logic.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private float _movementSpeed = 4.0f;

        private CharacterController _characterController;
        private IInputService _inputService;
        private ISaveLoadService _saveLoadService;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _characterController = GetComponent<CharacterController>();

            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _saveLoadService.OnSave += WriteToProgress;
        }

        private void Start() =>
            _saveLoadService.OnSave += WriteToProgress;

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                //Трансформируем экранныые координаты вектора в мировые
                movementVector = UnityEngine.Camera.main.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
        }

        private void OnDestroy() =>
            _saveLoadService.OnSave -= WriteToProgress;

        public void ReadFromProgress(PlayerProgress progress)
        {
            if (GetCurrentLevel() != progress.WorldData.PositionOnLevel.Level) return;

            Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
            if (savedPosition != null)
            {
                Warp(savedPosition);
            }
        }

        public void WriteToProgress(PlayerProgress progress)
        {
            Debug.Log($"Saved position {transform.position}");
            progress.WorldData.PositionOnLevel = new PositionOnLevel(GetCurrentLevel(), transform.position.AsVector3Data());
        }

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector3().AddY(_characterController.height * 2f);
            _characterController.enabled = true;
        }

        private static string GetCurrentLevel() =>
            SceneManager.GetActiveScene().name;
    }
}