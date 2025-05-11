using UnityEngine;

namespace Game.Tank
{
    public sealed class TankAudio : MonoBehaviour
    {
        [SerializeField] private float minSpeed = 0.2f;
        [SerializeField] private float maxSpeed = 1.2f;
        [Space]
        [SerializeField] private AudioClip bigGun, miniGun;
        [Space]
        [SerializeField] private AudioSource audioSourceEngine;
        [SerializeField] private AudioSource audioSourceGun;
        
        public void SetEngineSpeed(float value, float maxValue)
        {
            var pitch = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Abs(value) / maxValue);
            audioSourceEngine.pitch = pitch;
        }

        public void PlayGun(bool isMiniGun) => audioSourceGun.PlayOneShot(isMiniGun ? miniGun : bigGun);
    }
}