using ECS.Actions;
using ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECS.Systems
{
	public class TurretSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
	{
		private EcsCustomInject<TankInput> _tankInput;

		private readonly EcsFilterInject<Inc<TurretComponent>> _turretComponents = default;

		private bool _isBigGun;

		private Camera  _camera;
		private Vector3 _target;

		public void Init( IEcsSystems systems )
		{
			_isBigGun = false;

			_camera = Camera.main;
			
			_tankInput.Value.ActionMap.Point.performed += AimInput;
			_tankInput.Value.ActionMap.SwapGun.started += SwapTurretMode;
		}

		public void Run( IEcsSystems systems )
		{
			foreach( var entity in _turretComponents.Value )
			{
				ref var turretComponent = ref _turretComponents.Pools.Inc1.Get( entity );

				AimTurret( turretComponent );
				CastLaser( turretComponent );
				SwapTurret( ref turretComponent );
			}
		}

		private void AimInput( InputAction.CallbackContext context )
		{
			var target = context.ReadValue<Vector2>( );

			_target = _camera.ScreenToWorldPoint( target );
		}

		private void SwapTurretMode( InputAction.CallbackContext context )
		{
			_isBigGun = !_isBigGun;
			
			UIGameActions.OnTurretSwitched(_isBigGun);
		}

		private void AimTurret( TurretComponent turretComponent )
		{
			var transform     = turretComponent.Transform;
			var rotationSpeed = turretComponent.RotationSpeed;

			var targetRotation = Quaternion.LookRotation( _target - transform.position, transform.TransformDirection( Vector3.back ) );

			transform.rotation = Quaternion.RotateTowards( transform.rotation, new Quaternion( 0, 0, targetRotation.z, targetRotation.w ), rotationSpeed * Time.deltaTime );
		}

		private void CastLaser( TurretComponent turretComponent )
		{
			var transform     = turretComponent.Transform;
			var laserDistance = turretComponent.IsBigGun? turretComponent.BigGunLaserDistance: turretComponent.MiniGunLaserDistance;
			var laserMask     = turretComponent.LaserMask;
			var lineRenderer  = turretComponent.LineRenderer;

			var hit = Physics2D.Raycast( transform.position, transform.up, laserDistance, laserMask );

			if( hit )
				DrawLaser( transform.position, hit.point, lineRenderer );
			else
				DrawLaser( transform.position, transform.position + transform.up * laserDistance, lineRenderer );
		}

		private void DrawLaser( Vector2 startPoint, Vector2 endPoint, LineRenderer lineRenderer )
		{
			lineRenderer.SetPosition( 0, startPoint );
			lineRenderer.SetPosition( 1, endPoint );
		}

		private void SwapTurret( ref TurretComponent turretComponent )
		{
			turretComponent.IsBigGun = _isBigGun;

			if( _isBigGun && !turretComponent.BigGun.activeSelf )
			{
				turretComponent.BigGun.SetActive( true );
				turretComponent.MiniGun.SetActive( false );
			}
			else if( !_isBigGun && !turretComponent.MiniGun.activeSelf )
			{
				turretComponent.MiniGun.SetActive( true );
				turretComponent.BigGun.SetActive( false );
			}
		}

		public void Destroy( IEcsSystems systems )
		{
			_tankInput.Value.ActionMap.Point.performed -= AimInput;
			_tankInput.Value.ActionMap.SwapGun.started -= SwapTurretMode;
		}
	}
}