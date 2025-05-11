using System.Collections;
using UnityEngine;

namespace Game.Zombie
{
    public class ZombieAudio : MonoBehaviour
    {
        [SerializeField] private float minInterval = 1.5f;
        [SerializeField] private float maxInterval = 3f;
        [Space]
        [SerializeField] private AudioClip takeDamage, attack, die;
        [Space]
        [SerializeField] private AudioSource audioSourceIdle;
        [SerializeField] private AudioSource audioSourceAction;

        private void Start() => StartCoroutine(PlayWithIntervals());

        private IEnumerator PlayWithIntervals()
        {
            while(true)
            {
                audioSourceIdle.pitch = Random.Range(0.8f, 1.2f);
                audioSourceIdle.Play();

                var interval = Random.Range(minInterval, maxInterval);

                yield return new WaitForSeconds(interval);
            }
        }

        public void PlayDie() => audioSourceAction.PlayOneShot(die);

        public void Attack() => audioSourceAction.PlayOneShot(attack);

        public void TakeDamage() => audioSourceAction.PlayOneShot(takeDamage);
    }
}