using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startBtn, exitButton;

        private SceneLoader _loader;

        private void Awake()
        {
            startBtn.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void Start()
        {
            if(!Cursor.visible)
                Cursor.visible = true;

            _loader = SceneLoader.Instance;
            _loader.FadeScreen(0);
        }

        private void StartGame() => _loader.LoadGameScene();

        private void ExitGame() => Application.Quit();

        private void OnDestroy()
        {
            startBtn.onClick.RemoveListener(StartGame);
            exitButton.onClick.RemoveListener(ExitGame);
        }
    }
}