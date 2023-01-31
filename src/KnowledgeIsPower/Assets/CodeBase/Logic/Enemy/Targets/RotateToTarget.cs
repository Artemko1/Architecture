using UnityEngine;

namespace CodeBase.Logic.Enemy.Targets
{
    public class RotateToTarget : HasTargetBehaviour
    {
        [SerializeField] private float _speed = 1;

        private void Update()
        {
            if (HasTarget())
            {
                RotateTowardsHero();
            }
        }

        private void RotateTowardsHero()
        {
            Vector3 positionToLookAt = GetPositionToLookAt();

            transform.rotation = SmoothedRotation(transform.rotation, positionToLookAt);
        }

        private Vector3 GetPositionToLookAt()
        {
            Vector3 thisPosition = transform.position;
            Vector3 positionDelta = Target.position - thisPosition;
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