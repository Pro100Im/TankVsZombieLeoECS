using Game.Score;
using Game.Tank;
using Game.Zombie;
using Unity.Cinemachine;
using UnityEngine;

namespace Game
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;

            TankInput input = new TankInput();

            var tankSpawner = FindFirstObjectByType(typeof(TankSpawner)) as TankSpawner;
            var tank = tankSpawner.Spawn();
            tank.Init(input);

            var tankHpBar = FindFirstObjectByType(typeof(TankHpBar)) as TankHpBar;
            tankHpBar.Init(tank.CurrentHp);

            tank.OnHpChanged += tankHpBar.ChangeHp;

            var cinamachine = FindFirstObjectByType(typeof(CinemachineCamera)) as CinemachineCamera;
            cinamachine.Follow = tank.transform;

            var turretModeObservable = FindFirstObjectByType(typeof(TankTurret)) as ITurretModeObservable;
            var turretModeObserver = FindFirstObjectByType(typeof(TurretModeIndicator)) as ITurretModeObserver;
            turretModeObservable.AddTurretModeObserver(turretModeObserver);

            var killCounter = FindFirstObjectByType(typeof(KillCounter)) as KillCounter;

            var zombieSpawner = FindFirstObjectByType(typeof(ZombieSpawner)) as ZombieSpawner;
            zombieSpawner.Init(tank.transform, killCounter);

            var pause = FindFirstObjectByType(typeof(Pause)) as Pause;
            pause.Init(input);

            var gameOver = FindFirstObjectByType(typeof(GameOver)) as GameOver;
            gameOver.Init(killCounter);

            tank.OnDie += gameOver.Open;

            SceneLoader.Instance.FadeScreen(0);
        }
    }
}