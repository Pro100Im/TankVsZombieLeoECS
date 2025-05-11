using System.Collections.Generic;
using UnityEngine;

namespace Game.Bullet
{
    public sealed class BulletPool : MonoBehaviour, IBulletDeSpawner
    {
        [SerializeField] private BaseBullet bulletPrefab;
        [SerializeField] private int poolSize = 10;

        private Queue<BaseBullet> bulletPool = new Queue<BaseBullet>();

        private void Awake()
        {
            for(int i = 0; i < poolSize; i++)
            {
                var bullet = Instantiate(bulletPrefab, transform);
                bullet.Init(this);
                bullet.gameObject.SetActive(false);

                bulletPool.Enqueue(bullet);
            }
        }

        public BaseBullet Spawn(Vector3 position, Quaternion rotation)
        {
            if(bulletPool.Count > 0)
            {
                var bullet = bulletPool.Dequeue();
                ResetBullet(bullet, position, rotation);

                return bullet;
            }
            else
            {
                var bullet = Instantiate(bulletPrefab);
                ResetBullet(bullet, position, rotation);

                return bullet;
            }
        }

        private void ResetBullet(BaseBullet bullet, Vector3 position, Quaternion rotation)
        {
            bullet.transform.parent = null;
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;

            bullet.gameObject.SetActive(true);
        }

        public void Despawn(BaseBullet bullet)
        {
            bullet.gameObject.SetActive(false);
            bullet.transform.parent = transform;

            bulletPool.Enqueue(bullet);
        }
    }
}