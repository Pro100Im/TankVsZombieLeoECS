using UnityEngine;

namespace Game.Tank
{
    public sealed class TankMovement : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 3.5f;
        [SerializeField] private float rotationSpeed = 70f;
        [SerializeField] private float enginePower = 500f;
        [Space]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private TankAudio tankAudio;

        private float _currentSpeed = 0f;
        private float _currentRotation = 0f;

        public void Move(float value)
        {
            var targetSpeed = value * maxSpeed;
            _currentSpeed = targetSpeed;
        }

        public void Rotation(float value)
        {
            _currentRotation = value * rotationSpeed;
        }

        private void FixedUpdate()
        {
            rb.AddRelativeForceY(_currentSpeed * enginePower);
            rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -maxSpeed, maxSpeed);

            tankAudio.SetEngineSpeed(rb.linearVelocityY, maxSpeed);

            if(_currentRotation != 0)
                rb.rotation -= _currentRotation * Time.fixedDeltaTime;
        }
    }
}