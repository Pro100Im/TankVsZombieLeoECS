using AB_Utility.FromSceneToEntityConverter;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Bullet
{
    public sealed class BulletPool : MonoBehaviour
    {
        [SerializeField] private int initialSize = 10;
        [Space]
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform container;

        private Queue<GameObject> pool = new Queue<GameObject>();
        private EcsWorld world;

        public void Init(EcsWorld ecsWorld)
        {
            world = ecsWorld;

            for(int i = 0; i < initialSize; i++)
            {
                var entityRef = EcsConverter.InstantiateAndCreateEntity(prefab, world);
                entityRef.transform.SetParent(container);
                entityRef.gameObject.SetActive(false);
                pool.Enqueue(entityRef);
            }
        }

        public GameObject Spawn()
        {
            var entityRef = pool.Dequeue();
            entityRef.gameObject.SetActive(true);

            return entityRef;
        }

        public void Return(GameObject entityRef)
        {
            entityRef.gameObject.SetActive(false);
            pool.Enqueue(entityRef);
        }
    }
}