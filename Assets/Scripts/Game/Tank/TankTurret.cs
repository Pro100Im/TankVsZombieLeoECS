using System.Collections.Generic;
using UnityEngine;

namespace Game.Tank
{
    public sealed class TankTurret : MonoBehaviour, ITurretModeObservable
    {
        [SerializeField] private MiniGun miniGun;
        [SerializeField] private BigGun bigGun;
        [Space]
        [SerializeField] private float rotationSpeed = 25f;

        private const int Multiply = 100;

        private Camera _camera;
        private BaseGun _currentGun;

        private Vector3 _target;

        private void Awake() => _currentGun = miniGun;

        private void Start() => _camera = Camera.main;

        private List<ITurretModeObserver> turretModeObservers = new List<ITurretModeObserver>();

        public void AddTurretModeObserver(ITurretModeObserver observer)
        {
            if(!turretModeObservers.Contains(observer))
                turretModeObservers.Add(observer);
        }

        public void RemoveTurretModeObserver(ITurretModeObserver observer) => turretModeObservers.Remove(observer);

        public void SetTarget(Vector2 target) => _target = _camera.ScreenToWorldPoint(target);

        public void Fire() => _currentGun.Shoot();

        public void SwapTurretMode()
        {
            var isMiniGun = miniGun.isActiveAndEnabled;

            miniGun.gameObject.SetActive(!isMiniGun);
            bigGun.gameObject.SetActive(isMiniGun);

           _currentGun = isMiniGun ? bigGun : miniGun;

            NotifyTurretModeObservers();
        }

        private void NotifyTurretModeObservers()
        {
            foreach(var observer in turretModeObservers)
                observer.OnTurretModeChanged(_currentGun);
        }

        private void Update() => Aiming();

        private void Aiming()
        {
            var targetRotation = Quaternion.LookRotation(_target - transform.position, transform.TransformDirection(Vector3.back));

            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                new Quaternion(0, 0, targetRotation.z, targetRotation.w),
                rotationSpeed * Time.deltaTime * Multiply);
        }
    }
}