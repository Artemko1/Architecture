using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    public class RotateToHero : Follow
    {
        [SerializeField] private float _speed = 1;

        private Transform _heroTransform;

        private void Update()
        {
            if (_heroTransform != null)
            {
                RotateTowardsHero();
            }
        }

        public void Construct(Transform heroTransform) =>
            _heroTransform = heroTransform;


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