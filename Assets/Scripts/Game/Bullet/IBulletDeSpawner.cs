using UnityEngine;

namespace Game.Bullet
{
    public interface IBulletDeSpawner
    {
        public void Despawn(BaseBullet bullet);
    }
}