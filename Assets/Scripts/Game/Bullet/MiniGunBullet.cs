using UnityEngine;

namespace Game.Bullet
{
    public sealed class MiniGunBullet : BaseBullet
    {
        [SerializeField] private LayerMask playerMask;

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);

            if(((1 << collision.gameObject.layer) & playerMask.value) != 0)
                return;

            collision.gameObject.TryGetComponent(out IDamageable damageable);

            if(damageable != null)
                damageable.TakeDamage(damage);
        }
    }
}