using CodeBase.Data;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Logic.Hero
{
    public class HeroMove : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 4.0f;

        private CharacterController _characterController;
        private IInputService _inputService;
        private PlayerProgress _progress;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(IInputService inputService, ISaveLoadService saveLoadService, PersistentProgressService progressService)
        {
            _inputService = inputService;
            _saveLoadService = saveLoadService;
            _progress = progressService.Progress;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            ReadFromProgress();
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

        private void ReadFromProgress()
        {
            if (GetCurrentLevel() != _progress.PlayerState.PositionOnLevel.LevelName) return;

            Vector3 savedPosition = _progress.PlayerState.PositionOnLevel.Position.AsUnityVector3();

            Warp(savedPosition);
        }

        private void WriteToProgress(PlayerProgress progress)
        {
            Debug.Log($"Saved position {transform.position}");
            progress.PlayerState.PositionOnLevel = new PositionOnLevel(GetCurrentLevel(), transform.position.AsVector3Data());
        }

        private void Warp(Vector3 to)
        {
            _characterController.enabled = false;
            transform.position = to.AddY(_characterController.height * 2f);
            _characterController.enabled = true;
        }

        private static string GetCurrentLevel() =>
            SceneManager.GetActiveScene().name;
    }
}