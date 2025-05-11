using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    public sealed class Pause : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button backToMenuBtb;

        private bool _isPaused = false;

        private TankInput _input;

        private void Awake() => backToMenuBtb.onClick.AddListener(BackToMenu);

        public void Init(TankInput input)
        {
            _input = input;

            _input.ActionMap.Pause.canceled += PauseGame;
        }

        private void PauseGame(InputAction.CallbackContext context)
        {
            if(!_isPaused)
            {
                _input.ActionMap.Fire.Disable();

                Cursor.visible = true;

                Time.timeScale = 0;

                panel.SetActive(true);

                _isPaused = true;
            }
            else
            {
                Cursor.visible = false;

                Time.timeScale = 1;

                panel.SetActive(false);

                _isPaused = false;

                _input.ActionMap.Fire.Enable();
            }
        }

        private void BackToMenu()
        {
            DOTween.KillAll();

            Time.timeScale = 1;
            SceneLoader.Instance.LoadMenuScene();
        }

        private void OnDestroy()
        {
            _input.ActionMap.Pause.canceled -= PauseGame;

            backToMenuBtb.onClick.RemoveListener(BackToMenu);
        }
    }
}