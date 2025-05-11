using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tank
{
    public sealed class TankHpBar : MonoBehaviour
    {
        [SerializeField] private float changeHpDuration = 0.4f;
        [SerializeField] private Slider hp;
        [SerializeField] private Slider hpShadow;

        private int _maxHp;

        public void Init(int maxHp) => _maxHp = maxHp;

        public void ChangeHp(int currentHp)
        {
            var targetValue = (float)currentHp / _maxHp;

            hp.value = targetValue;
            hpShadow.DOValue(targetValue, changeHpDuration).SetDelay(changeHpDuration);
        }
    }
}