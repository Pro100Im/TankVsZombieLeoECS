using DG.Tweening;
using UnityEngine;

namespace Game.Tank
{
    public sealed class BigGun : BaseGun
    {
        [SerializeField] private float RelodDuration = 3f;
        [SerializeField] private float HeatingDuration = 0.25f;
        [SerializeField] private float recoilPower = 50f;
        [Space]
        [SerializeField] private SpriteRenderer gun;
        [SerializeField] private Color HotCollor;
        [Space]
        [SerializeField] private Rigidbody2D rb;

        private bool isReloaded = true;

        public override void Shoot()
        {
            if(!isReloaded)
                return;

            tankAudio.PlayGun(false);

            base.Shoot();
            isReloaded = false;

            var recoilForce = rb.position - (Vector2)transform.up * recoilPower;
            rb.AddForce(recoilForce, ForceMode2D.Impulse);

            gun.DOColor(HotCollor, HeatingDuration)
                .OnComplete(() => gun.DOColor(Color.white, RelodDuration)
                .OnComplete(() => isReloaded = true));
        }
    }
}