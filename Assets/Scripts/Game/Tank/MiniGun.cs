using UnityEngine;

namespace Game.Tank
{
    public sealed class MiniGun : BaseGun
    {
        public override void Shoot()
        {
            tankAudio.PlayGun(true);

            base.Shoot();
        }
    }
}