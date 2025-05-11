using Game.Bullet;
using UnityEngine;

namespace Game.Tank
{
    public abstract class BaseGun : MonoBehaviour
    {
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected BulletPool bulletPool;
        [SerializeField] protected ParticleSystem fireEffect;
        [SerializeField] protected TankAudio tankAudio;

        public virtual void Shoot()
        {
            fireEffect.Play();

            var bullet = bulletPool.Spawn(firePoint.position, firePoint.rotation);
            bullet.AddForce();
        }
    }
}