using UnityEngine;

namespace Game.Zombie
{
    public sealed class ZombieAttack : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private float timeBetweenAttack = 1f;
        [SerializeField] private float distanceForAttack = 1f;
        [Space]
        [SerializeField] private ZombieAudio zombieAudio;

        private float _lastAttackTime;

        private IDamageable _target;

        public void SetTarget(Transform target)
        {
            target.TryGetComponent(out IDamageable damageable);

            _target = damageable;
        }

        public void Attack(float distance)
        {
            if(distance > distanceForAttack)
                return;

            if(Time.time - _lastAttackTime < timeBetweenAttack)
                return;

            zombieAudio.Attack();

            _target.TakeDamage(damage);
            _lastAttackTime = Time.time;
        }
    }
}