using DG.Tweening;
using UnityEngine;

namespace Game.Zombie
{
    public sealed class ZombieHpBar : MonoBehaviour
    {
        [SerializeField] private float changeHpDuration = 0.4f;
        [SerializeField] private Transform hp;
        [SerializeField] private Transform hpShadow;

        private Transform _cameraTransform;

        private int _maxHp;

        private void Start() => _cameraTransform = Camera.main.transform;

        public void Init(int maxHp) => _maxHp = maxHp;

        private void LateUpdate() => transform.rotation = Quaternion.LookRotation(transform.position - _cameraTransform.position);

        public void ChangeHp(int currentHp)
        {
            var targetValue = (float)currentHp / _maxHp;

            hp.localScale = new Vector3(targetValue, 1, 1);
            hpShadow.DOScaleX(targetValue, changeHpDuration).SetDelay(changeHpDuration);
        }

        private void OnDestroy() => DOTween.Kill(hpShadow);
    }
}