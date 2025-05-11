using UnityEngine;

namespace Game.Bullet
{
    public sealed class BigGunBullet : BaseBullet
    {
        [SerializeField] private float explosionRadius = 4f;
        [SerializeField] private ParticleSystem explosionPrefab;

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            base.OnCollisionEnter2D(collision);

            Vector2 collisionPoint = collision.GetContact(0).point;

            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(collisionPoint, explosionRadius);

            foreach(Collider2D nearbyCollider in nearbyColliders)
            {
                nearbyCollider.TryGetComponent(out IDamageable damageable);

                if(damageable != null)
                    damageable.TakeDamage(damage);
            }
        }
    }
}