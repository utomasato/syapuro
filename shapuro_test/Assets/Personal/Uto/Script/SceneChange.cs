using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private Image fadeImage; // フェードインアウト用のイメージ
    [SerializeField] private float fadeTime = 1.0f; // フェードインアウトにかかる時間
    private Color fadeColor;
    string NextScene; // フェードアウト後に移動するシーン
    private bool IsFadeInNow = false;
    private bool IsFadeOutNow = false;
    [SerializeField] private float zoomTime; // ズームインアウトにかかる時間

    [System.Serializable]
    public class MovementStep
    {
        [SerializeField] private Vector3 cameraPosition;
        [SerializeField] private float cameraSize;
        [SerializeField] private float restTime;
        public bool IsResting;

        public Vector3 CameraPosition => cameraPosition;
        public float CameraSize => cameraSize;
        public float RestTime => restTime;
    }
    [SerializeField] private List<MovementStep> stepList;

    private float elapsedTime = 0.0f;

    [SerializeField] private bool IsInStage = false;
    [SerializeField] private GameState gameState;
    [SerializeField] private int step;
    [SerializeField] private bool IsMoving;

    void Start()
    {
        fadeColor = fadeImage.color;
        StartFadeIn();
        step = 0;
        IsMoving = false;
        if (IsInStage)
        {
            Debug.Log("isplayd : " + SceneSelectionState.IsPlayed);
            if (!SceneSelectionState.IsPlayed)
            {
                SceneSelectionState.IsPlayed = true;
            }
            else
            {
                step = 1;
            }
            Debug.Log("step : " + step);
            transform.position = stepList[step].CameraPosition;
            Camera.main.orthographicSize = stepList[step].CameraSize;
        }
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
                //fadeCanvas.SetActive(false);
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
                fadeCanvas.SetActive(false);
                if (IsInStage)
                {
                    IsMoving = true;
                    elapsedTime = 0.0f;
                    stepList[step].IsResting = true;
                }
            }
        }

        if (IsMoving)
        {
            if (stepList[step].IsResting)
            {
                elapsedTime += Time.deltaTime; // 経過時間を更新
                if (elapsedTime > stepList[step].RestTime)
                {
                    stepList[step].IsResting = false;
                    elapsedTime = 0f;
                    if (step + 1 >= stepList.Count)
                    {
                        IsMoving = false;
                        gameState.GameStart();
                    }
                }
            }
            else
            {
                elapsedTime += Time.deltaTime; // 経過時間を更新
                Camera.main.orthographicSize = Mathf.Lerp(stepList[step].CameraSize, stepList[step + 1].CameraSize, elapsedTime / zoomTime); // サイズを線形補間
                transform.position = Vector3.Lerp(stepList[step].CameraPosition, stepList[step + 1].CameraPosition, elapsedTime / zoomTime); // 位置を線形補完
                if (elapsedTime >= zoomTime)
                {
                    Camera.main.orthographicSize = stepList[step + 1].CameraSize;
                    transform.position = stepList[step + 1].CameraPosition;
                    step += 1;
                    elapsedTime = 0f;
                    stepList[step].IsResting = true;
                }
            }
        }

    }

    public void StartFadeOut(string nextscene = null) // だんだん暗く
    {
        fadeCanvas.SetActive(true);
        IsFadeOutNow = true;
        elapsedTime = 0.0f;
        fadeColor.a = 0;
        fadeImage.color = fadeColor;
        NextScene = nextscene;
    }

    public void StartFadeIn() // だんだん明るく
    {
        fadeCanvas.SetActive(true);
        IsFadeInNow = true;
        elapsedTime = 0.0f;
        fadeColor.a = 1;
        fadeImage.color = fadeColor;
    }

    public bool IsFadeNow => IsFadeInNow || IsFadeOutNow;
}
