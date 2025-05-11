using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string menuScene;
    [SerializeField] private string gameScene;
    [Space]
    [SerializeField] private Image transitionScreen;
    [SerializeField] private float fadeDuration = .3f;

    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadMenuScene() => FadeScreen(1, () => LoadScene(menuScene));

    public void LoadGameScene() => FadeScreen(1, () => LoadScene(gameScene));

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

    public void FadeScreen(float endValue, Action callback = null)
    {
        DOTween.Kill(transitionScreen);

        transitionScreen.raycastTarget = endValue > 0;
        transitionScreen.DOFade(endValue, fadeDuration)
            .OnComplete(() => callback?.Invoke());
    }
}
