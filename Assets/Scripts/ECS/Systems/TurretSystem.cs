using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems
{
    public class TurretSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsPool<TurretComponent> _turretPool;
        private EcsPool<PlayerInputComponent> _inputPool;

        private EcsFilter _turretFilter;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _turretPool = _world.GetPool<TurretComponent>();
            _inputPool = _world.GetPool<PlayerInputComponent>();

            _turretFilter = _world.Filter<TurretComponent>().Inc<PlayerInputComponent>().End();

            foreach(var entity in _turretFilter)
            {
                ref var turret = ref _turretPool.Get(entity);
                turret.Camera = Camera.main;
            }
        }

        public void Run(IEcsSystems systems)
        {
            if(_turretFilter.GetEntitiesCount() == 0)
                return;

            var entity = _turretFilter.GetRawEntities()[0];

            ref var input = ref _inputPool.Get(entity);
            ref var turret = ref _turretPool.Get(entity);

            var inputPoint = input.AimingInput;
            var swapRequested = input.SwapGunRequested;

            turret.Target = turret.Camera.ScreenToWorldPoint(inputPoint);

            AimTurret(turret);
            CastLaser(turret);

            if(swapRequested)
                SwapTurret(ref turret);
        }

        private void AimTurret(TurretComponent turretComponent)
        {
            var target = turretComponent.Target;
            var transform = turretComponent.Transform;
            var rotationSpeed = turretComponent.RotationSpeed;

            var targetRotation = Quaternion.LookRotation(target - transform.position, transform.TransformDirection(Vector3.back));
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                new Quaternion(0, 0, targetRotation.z, targetRotation.w),
                rotationSpeed * Time.deltaTime
            );
        }

        private void CastLaser(TurretComponent turretComponent)
        {
            var transform = turretComponent.Transform;
            var laserDistance = turretComponent.IsBigGun ? turretComponent.BigGunLaserDistance : turretComponent.MiniGunLaserDistance;
            var laserMask = turretComponent.LaserMask;
            var lineRenderer = turretComponent.LineRenderer;

            var hit = Physics2D.Raycast(transform.position, transform.up, laserDistance, laserMask);

            if(hit.collider != null)
                DrawLaser(transform.position, hit.point, lineRenderer);
            else
                DrawLaser(transform.position, transform.position + transform.up * laserDistance, lineRenderer);
        }

        private void DrawLaser(Vector2 startPoint, Vector2 endPoint, LineRenderer lineRenderer)
        {
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }

        private void SwapTurret(ref TurretComponent turretComponent)
        {
            turretComponent.IsBigGun = !turretComponent.IsBigGun;

            bool isBigGun = turretComponent.IsBigGun;

            if(isBigGun && !turretComponent.BigGun.activeSelf)
            {
                turretComponent.BigGun.SetActive(true);
                turretComponent.MiniGun.SetActive(false);
            }
            else if(!isBigGun && !turretComponent.MiniGun.activeSelf)
            {
                turretComponent.MiniGun.SetActive(true);
                turretComponent.BigGun.SetActive(false);
            }
        }
    }
}