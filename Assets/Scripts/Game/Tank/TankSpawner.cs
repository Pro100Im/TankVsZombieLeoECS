using UnityEngine;

namespace Game.Tank
{
    public class TankSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPos;
        [SerializeField] private TankController prefab;

        public TankController Spawn()
        {
            var tank = Instantiate(prefab, spawnPos.position, spawnPos.rotation);

            return tank;
        }
    }
}