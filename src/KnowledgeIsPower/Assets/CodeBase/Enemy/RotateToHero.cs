using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        [SerializeField] private float _speed = 1;

        private IGameFactory _gameFactory;

        private Transform _heroTransform;

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            if (IsHeroExist())
                InitializeHeroTransform();
            else
                _gameFactory.HeroCreated += InitializeHeroTransform;
        }

        private void Update()
        {
            if (IsInitialized())
                RotateTowardsHero();
        }

        private void OnDestroy()
        {
            if (_gameFactory != null)
                _gameFactory.HeroCreated -= InitializeHeroTransform;
        }

        private bool IsHeroExist() =>
            _gameFactory.HeroGameObject != null;

        private bool IsInitialized() =>
            _heroTransform != null;

        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private void RotateTowardsHero()
        {
            Vector3 positionToLookAt = GetPositionToLookAt();

            transform.rotation = SmoothedRotation(transform.rotation, positionToLookAt);
        }

        private Vector3 GetPositionToLookAt()
        {
            Vector3 thisPosition = transform.position;
            Vector3 positionDelta = _heroTransform.position - thisPosition;
            return new Vector3(positionDelta.x, thisPosition.y, positionDelta.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) =>
            Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

        private Quaternion TargetRotation(Vector3 position) =>
            Quaternion.LookRotation(position);

        private float SpeedFactor() =>
            _speed * Time.deltaTime;
    }
}