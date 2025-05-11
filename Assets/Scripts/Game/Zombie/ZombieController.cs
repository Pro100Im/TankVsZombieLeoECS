using System;
using UnityEngine;

namespace Game.Zombie
{
    public sealed class ZombieController : MonoBehaviour, IDamageable
    {
        public event Action OnDie;

        [SerializeField] private string hitAnimTrigger= "hit";
        [SerializeField] private int maxHp = 100;
        [Space]
        [SerializeField] private ZombieMovement zombieMovement;
        [SerializeField] private ZombieAttack zombieAttack;
        [SerializeField] private ZombieHpBar zombieHpBar;
        [SerializeField] private ZombieAudio zombieAudio;
        [Space]
        [SerializeField] private ParticleSystem dieEffect;
        [Space]
        [SerializeField] private Animator animator;

        public int CurrentHp { get; private set; }

        private Transform _target;

        private void Awake()
        {
            CurrentHp = maxHp;

            zombieHpBar.Init(CurrentHp);
        }

        public void SetTarget(Transform target)
        {
            _target = target;

            zombieMovement.SetTarget(target);
            zombieAttack.SetTarget(target);
        }

        private void FixedUpdate()
        {
            var distance = Vector3.Distance(transform.position, _target.position);

            zombieAttack.Attack(distance);
        }

        public void TakeDamage(int damage)
        {
            CurrentHp -= damage;
            CurrentHp = Math.Clamp(CurrentHp, 0, maxHp);

            CheckIsLife();
        }

        private void CheckIsLife()
        {
            if(CurrentHp > 0)
            {
                zombieAudio.TakeDamage();
                zombieHpBar.ChangeHp(CurrentHp);
                animator.SetTrigger(hitAnimTrigger);

                return;
            }

            zombieAudio.PlayDie();
            dieEffect.Play();

            zombieMovement.enabled = false;
            zombieAttack.enabled = false;

            OnDie?.Invoke();
;           OnDie = null;

            Destroy(gameObject, 1f);
        }
    }
}