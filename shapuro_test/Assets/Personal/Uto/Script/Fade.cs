using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;
    private float elapsedTime = 0.0f;
    private Color fadeColor;
    private bool IsFadeOutNow;
    [SerializeField] private bool IsFadeInNow;
    [SerializeField] string NextScene;
    [SerializeField] private bool IsInStage = false;
    [SerializeField] private StageSelect select;
    [SerializeField] private CameraZoom camera;

    void Start()
    {
        fadeColor = fadeImage.color;
        IsFadeOutNow = false;
    }

    void Update()
    {
        if (IsFadeOutNow) // だんだん明るく
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            if (elapsedTime > fadeDuration)
            {
                IsFadeOutNow = false;
                if (NextScene != "")
                {
                    SceneManager.LoadScene(NextScene);
                }
            }
        }
        if (IsFadeInNow) // だんだん暗く
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(1.0f - elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            if (elapsedTime > fadeDuration)
            {
                IsFadeInNow = false;
                if (IsInStage)
                    camera.StartZoom();
                if (select != null)
                    select.Move(0);
            }
        }
    }

    public void FadeOutStart()
    {
        IsFadeOutNow = true;
        elapsedTime = 0.0f;
        fadeColor.a = 0;
        fadeImage.color = fadeColor;
    }

    public void FadeOutStart(string nextscene)
    {
        IsFadeOutNow = true;
        elapsedTime = 0.0f;
        fadeColor.a = 0;
        fadeImage.color = fadeColor;
        NextScene = nextscene;
    }

    public void FadeInStart()
    {
        IsFadeInNow = true;
        elapsedTime = 0.0f;
        fadeColor.a = 1;
        fadeImage.color = fadeColor;
    }
}
