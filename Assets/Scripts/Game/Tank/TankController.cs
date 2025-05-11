using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace Game.Tank
{
    public sealed class TankController : MonoBehaviour, IDamageable
    {
        public event Action OnDie;
        public event Action<int> OnHpChanged;

        [SerializeField] private int maxHp = 30;
        [Space]
        [SerializeField] private TankMovement movement;
        [SerializeField] private TankTurret turret;

        public int CurrentHp { get; private set; }

        private TankInput _input;

        private void Awake() => CurrentHp = maxHp;

        public void Init(TankInput tankInput)
        {
            _input = tankInput;

            _input.ActionMap.Move.performed += MoveInput;
            _input.ActionMap.Move.canceled += MoveInput;

            _input.ActionMap.Point.performed += AimInput;
            _input.ActionMap.Fire.performed += FireInput;

            _input.ActionMap.SwapGun.started += SwapTurret;

            _input.ActionMap.Enable();
        }

        private void MoveInput(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>().normalized;

            movement.Move(input.y);
            movement.Rotation(input.x);
        }

        private void AimInput(InputAction.CallbackContext context)
        {
            var target = context.ReadValue<Vector2>();

            turret.SetTarget(target);
        }

        private void FireInput(InputAction.CallbackContext context) => turret.Fire();

        private void SwapTurret(InputAction.CallbackContext context) => turret.SwapTurretMode();

        public void TakeDamage(int damage)
        {
            if(CurrentHp <= 0)
            {
                _input.ActionMap.Disable();
                movement.Move(0);

                OnDie?.Invoke();

                return;
            }

            CurrentHp -= damage;
            CurrentHp = Math.Clamp(CurrentHp, 0, maxHp);

            OnHpChanged(CurrentHp);
        }

        private void OnDestroy()
        {
            /*_input.ActionMap.Move.performed -= MoveInput;
            _input.ActionMap.Move.canceled -= MoveInput;

            _input.ActionMap.Point.performed -= AimInput;
            _input.ActionMap.Fire.performed -= FireInput;

            _input.ActionMap.SwapGun.started -= SwapTurret;

            _input.ActionMap.Disable();*/
        }
    }
}