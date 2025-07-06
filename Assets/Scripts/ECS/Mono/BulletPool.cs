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
                var go = EcsConverter.InstantiateAndCreateEntity(prefab, world);
                go.transform.SetParent(container);
                go.SetActive(false);
                pool.Enqueue(go);
            }
        }

        public GameObject Get()
        {
            GameObject go = pool.Count > 0 ? pool.Dequeue() : EcsConverter.InstantiateAndCreateEntity(prefab, world);
            go.transform.SetParent(container);
            go.SetActive(true);
            return go;
        }

        public void Return(GameObject go)
        {
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }
}