using TMPro;
using UnityEngine;

namespace Game.Score
{
    public sealed class KillCounter : MonoBehaviour, IKillCounter, IGetKills
    {
        [SerializeField] private TextMeshProUGUI countText;

        private int _count;

        private void Awake() => countText.text = _count.ToString();

        public void KillCountIncrement()
        {
            _count++;
            countText.text = _count.ToString();
        }

        public int GetCount() => _count;
    }
}