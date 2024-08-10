using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // フェードインアウト用のイメージ
    [SerializeField] private float fadeTime = 1.0f; // フェードインアウトにかかる時間
    private Color fadeColor;
    string NextScene; // フェードアウト後に移動するシーン
    private bool IsFadeInNow;
    private bool IsFadeOutNow;

    private float startSize; // ズームインアウト前の大きさ
    [SerializeField] private float endSize; // ズームインアウト後の大きさ
    private Vector3 startPosition; // 移動前のカメラの位置
    [SerializeField] private Vector3 endPosition; // 移動後のカメラの位置
    [SerializeField] private float zoomTime; // ズームインアウトにかかる時間
    private bool IsZoomNow = false;

    private float elapsedTime = 0.0f;

    [SerializeField] private bool IsInStage = false;
    [SerializeField] private GameState gameState;

    void Start()
    {
        fadeColor = fadeImage.color;
        IsFadeOutNow = false;
        if (IsInStage)
            StartFadeIn();
    }

    void Update()
    {
        if (IsFadeOutNow) // だんだん暗く
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(elapsedTime / fadeTime);
            fadeImage.color = fadeColor;
            if (elapsedTime > fadeTime)
            {
                IsFadeOutNow = false;
                if (NextScene != null)
                {
                    SceneManager.LoadScene(NextScene);
                }
            }
        }

        if (IsFadeInNow) // だんだん明るく
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(1.0f - elapsedTime / fadeTime);
            fadeImage.color = fadeColor;
            if (elapsedTime > fadeTime)
            {
                IsFadeInNow = false;
                if (IsInStage)
                    StartZoom();
            }
        }

        if (IsZoomNow)
        {
            elapsedTime += Time.deltaTime; // 経過時間を更新
            Camera.main.orthographicSize = Mathf.Lerp(startSize, endSize, elapsedTime / zoomTime); // サイズを線形補間
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / zoomTime); // 位置を線形補完
            if (elapsedTime >= zoomTime)
            {
                Camera.main.orthographicSize = endSize;
                transform.position = endPosition;
                IsZoomNow = false;
                if (IsInStage)
                    gameState.GameStart();
            }
        }
    }

    public void StartFadeOut(string nextscene = null)
    {
        IsFadeOutNow = true;
        elapsedTime = 0.0f;
        fadeColor.a = 0;
        fadeImage.color = fadeColor;
        NextScene = nextscene;
    }

    public void StartFadeIn()
    {
        IsFadeInNow = true;
        elapsedTime = 0.0f;
        fadeColor.a = 1;
        fadeImage.color = fadeColor;
    }

    public void StartZoom()
    {
        elapsedTime = 0.0f;
        startSize = Camera.main.orthographicSize;
        startPosition = transform.position;
        IsZoomNow = true;
    }
}
