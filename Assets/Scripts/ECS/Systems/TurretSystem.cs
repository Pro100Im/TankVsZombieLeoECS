using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace ECS.Systems
{
    public class TurretSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TurretComponent>> _turretComponents = default;
        private readonly EcsFilterInject<Inc<PlayerInputComponent>> _playerInputComponents = default;

        public void Init(IEcsSystems systems)
        {
            var entity = _turretComponents.Value.GetRawEntities()[0];
            ref var turretComponent = ref _turretComponents.Pools.Inc1.Get(entity);

            turretComponent.Camera = Camera.main;
        }

        public void Run(IEcsSystems systems)
        {
            if(_turretComponents.Value.GetEntitiesCount() == 0 || _playerInputComponents.Value.GetEntitiesCount() == 0)
                return;

            var entity = _turretComponents.Value.GetRawEntities()[0];
            var playerInputComponents = _playerInputComponents.Pools.Inc1.Get(entity);
            ref var turretComponent = ref _turretComponents.Pools.Inc1.Get(entity);

            var inputPoint = playerInputComponents.AimingInput;
            var swapRequested = playerInputComponents.SwapGunRequested;

            turretComponent.Target = turretComponent.Camera.ScreenToWorldPoint(inputPoint);

            AimTurret(turretComponent);
            CastLaser(turretComponent);

            if(swapRequested)
                SwapTurret(ref turretComponent);
        }

        private void AimTurret(TurretComponent turretComponent)
        {
            var target = turretComponent.Target;
            var transform = turretComponent.Transform;
            var rotationSpeed = turretComponent.RotationSpeed;

            var targetRotation = Quaternion.LookRotation(target - transform.position, transform.TransformDirection(Vector3.back));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(0, 0, targetRotation.z, targetRotation.w), rotationSpeed * Time.deltaTime);
        }

        private void CastLaser(TurretComponent turretComponent)
        {
            var transform = turretComponent.Transform;
            var laserDistance = turretComponent.IsBigGun ? turretComponent.BigGunLaserDistance : turretComponent.MiniGunLaserDistance;
            var laserMask = turretComponent.LaserMask;
            var lineRenderer = turretComponent.LineRenderer;

            var hit = Physics2D.Raycast(transform.position, transform.up, laserDistance, laserMask);

            if(hit)
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

            var isBigGun = turretComponent.IsBigGun;

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