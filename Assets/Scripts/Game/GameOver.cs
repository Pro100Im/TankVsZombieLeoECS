using DG.Tweening;
using Game.Score;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class GameOver : MonoBehaviour
    {
        [SerializeField] private string timeInGameStr = "Time In Game: ";
        [SerializeField] private GameObject panel;
        [SerializeField] private Button backToMenuBtb;
        [SerializeField] private TextMeshProUGUI timeInGameText;
        [SerializeField] private TextMeshProUGUI killCountText;

        private bool _isGameOver;

        private float _time;

        private IGetKills _getKills;

        private void Awake() => backToMenuBtb.onClick.AddListener(BackToMenu);

        public void Init(IGetKills getKills)
        {
            _getKills = getKills;

            _time = Time.time;
        }

        public void Open()
        {
            if(_isGameOver)
                return;

            Cursor.visible = true;

            var elapsedTime = Time.time - _time;
            var timeSpan = TimeSpan.FromSeconds(elapsedTime);
            var formattedTime = timeSpan.ToString(@"hh\:mm\:ss");

            timeInGameText.text = $"{timeInGameStr}{formattedTime}";
            killCountText.text = _getKills.GetCount().ToString();

            Time.timeScale = 0;

            _isGameOver = true;

            panel.SetActive(true);
        }

        private void BackToMenu()
        {
            DOTween.KillAll();

            Time.timeScale = 1;
            SceneLoader.Instance.LoadMenuScene();
        }

        private void OnDestroy()
        {
            backToMenuBtb.onClick.RemoveListener(BackToMenu);
        }
    }
}